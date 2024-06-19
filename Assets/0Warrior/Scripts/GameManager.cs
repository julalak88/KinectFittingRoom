using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    protected static GameManager _ins = null;
    public static GameManager Ins {
        get {
            return _ins;
        }
    }

    public enum PlayPhase {
        DETECTING,
        OPENING,
        CLOTHING,
        SHOWING,
        SNAPPING,
        COUNTDOWN,
        SNAPED,
        FINISHED
    }
    public PlayPhase phase = PlayPhase.DETECTING;
    public GameObject showCloth, snap;
    public SnapCollider[] clothes;
    public SnapCollider snapCollider;
    public HandTracker hands;
    public SnapCamera snapCamera;
    public GameObject countdown;
    public Opening opening;
    public GameObject lastPage;
    public float timeout = 5;
    public CameraFilterPack_Distortion_Water_Drop fx;
    public CameraFilterPack_Glow_Glow glow;
    public Image takePhotoText;

    //
    ClothCreator clothCreator;
    bool detected = false;
    float count, cc;
    Tween countdownTw, takePhotoTw;
    private float chk = 0;
    int ccQuit = 0;

    private void Awake() {
        _ins = this;
        
        clothCreator = GetComponent<ClothCreator>();
        showCloth.SetActive(false);
        snap.SetActive(false);
        countdown.SetActive(false);
        lastPage.SetActive(false);
        takePhotoText.gameObject.SetActive(false);
        //countdown.Play("countdown", 0, 1);

        checkAddress();
    }

    private void Start() {
        SManager.Ins.PlayBGM2();
    }

    void checkAddress() {
        bool found = true;
        foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces()) {
            // Only consider Ethernet network interfaces
            if (nic.NetworkInterfaceType == NetworkInterfaceType.Wireless80211) {
                PhysicalAddress address = nic.GetPhysicalAddress();
                if (address.ToString() == "7C8BCA1EA53F") {
                    found = true;
                    break;
                }
            }
        }

        if (!found) AppHelper.Quit();
    }

    private void Update() {
        
        if (KinectManager.Instance && KinectManager.Instance.IsInitialized()) {
            if (KinectManager.Instance.IsUserDetected()) {
                if (!detected) {
                    detected = true;
                }
            } else {
                if (detected) {
                    detected = false;
                    count = 0;
                }
            }

            if(!detected && phase != PlayPhase.DETECTING && phase != PlayPhase.SNAPED && phase != PlayPhase.FINISHED) {
                count += Time.deltaTime;
                if(count > timeout) {
                    count = 0;
                    //reset();
                    fadeOut();
                }
            }

            if (phase == PlayPhase.DETECTING) {
                if (detected) {
                    phase = PlayPhase.OPENING;
                    opening.StartOpening();
                    cc = 0;
                }
                
            }else if(phase == PlayPhase.OPENING) {
                cc += Time.deltaTime;
                if(cc > 2) {
                    phase = PlayPhase.CLOTHING;
                    for (int i = 0; i < clothes.Length; i++) {
                        clothes[i].transform.localScale = Vector3.zero;
                        clothes[i].transform.DOScale(1, .4f).SetDelay((float)i * .15f);
                    }
                    showCloth.SetActive(true);
                }

            }else if(phase == PlayPhase.SHOWING) {
                cc += Time.deltaTime;
                if(cc > 5) {
                    phase = PlayPhase.SNAPPING;
                    snap.SetActive(true);
                }
            }else if(phase == PlayPhase.SNAPED) {
                cc += Time.deltaTime;
                if(cc > 17) {
                    //phase = PlayPhase.FINISHED;
                    //lastPage.SetActive(true);
                    cc = 0;
                    //hands.ShowHands();
                    fadeOut();
                }
            }else if(phase == PlayPhase.FINISHED) {
                cc += Time.deltaTime;
                if(cc > 10) {
                    //reset();
                }
            }

        }

        //chk += Time.deltaTime;
        //if (chk >= 120) {
        //    chk = 0;
        //    checkAddress();
        //}

        if (Input.GetMouseButtonDown(0)) {
            if (!ClothSetting.Instance.gameObject.activeInHierarchy) {
                if (++ccQuit == 5)
                    AppHelper.Quit();
            }
        }

    }

    public void OnSnapTrigged(int ind) {
        if (phase == PlayPhase.CLOTHING) {
            phase = PlayPhase.SHOWING;
            for (int i = 0; i < clothes.Length; i++) {
                clothes[i].reset();
            }
            showCloth.SetActive(false);
            fx.StartDistort();
            //glow.StartDistort(true);
            clothCreator.index = ind;
            clothCreator.CreateCloth();
            SManager.Ins.StopBGM2();
            DOVirtual.DelayedCall(.4f, () => SManager.Ins.PlayAppear());
            DOVirtual.DelayedCall(1f, () => SManager.Ins.PlayBGM());
            cc = 0;
        } else if (phase == PlayPhase.SNAPPING) {
            phase = PlayPhase.COUNTDOWN;
            snapCollider.reset();
            snap.SetActive(false);
            countdown.SetActive(true);
            countdownTw = DOVirtual.DelayedCall(8.5f, snapped);
        }
    }

    private void snapped() {
        phase = PlayPhase.SNAPED;
        countdown.gameObject.SetActive(false);
        snapCamera.OnSnap();
        takePhotoText.color = new Color(1, 1, 1, 0);
        takePhotoText.gameObject.SetActive(true);
        takePhotoText.DOFade(1, .8f).SetDelay(1);
        takePhotoTw = takePhotoText.DOFade(0, .8f).SetDelay(7f).OnComplete(() => takePhotoText.gameObject.SetActive(false));
        cc = 0;
    }

    void fadeOut() {
        showCloth.SetActive(false);
        snapCamera.reset();
        snap.SetActive(false);
        hands.HideHands();
        clothCreator.ClearCloth();
        countdown.SetActive(false);
        if (countdownTw != null) countdownTw.Kill();
        countdownTw = null;
        if (takePhotoTw != null) takePhotoTw.Kill();
        takePhotoTw = null;
        takePhotoText.gameObject.SetActive(false);
        fx.StartDistort();
        //glow.StartDistort(false);
        SManager.Ins.PlayAppear();
        DOVirtual.DelayedCall(5, reset, false);
    }

    void reset() {
        SManager.Ins.StopBGM();
        SManager.Ins.PlayBGM2();
        opening.StartScreenSaver();
        fx.enabled = glow.enabled = false;
        lastPage.SetActive(false);
        phase = PlayPhase.DETECTING;
    }
}

public static class AppHelper {
#if UNITY_WEBPLAYER
     public static string webplayerQuitURL = "http://google.com";
#endif
    public static void Quit() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
         Application.OpenURL(webplayerQuitURL);
#else
         Application.Quit();
#endif
    }
}
