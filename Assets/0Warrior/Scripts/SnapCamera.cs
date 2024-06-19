using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.IO;

public class SnapCamera : MonoBehaviour {

    public int resWidth = 500;
    public int resHeight = 600;
    public RawImage rawImage;
    //public Text codeText, nameText, secretText;
    public Image flash;
    public Ease ease = Ease.OutFlash;
    public CaptureCamera capture;

    [Space(10)]
    [Header("random name")]
    public List<string> names;

    Camera cam;
    //string[] Alphabet = new string[26] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
    Texture2D screenShot;
    
    private void Awake() {

        cam = GetComponent<Camera>();

        reset();
    }

    public void OnSnap() {
        //cam.enabled = true;

        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        cam.targetTexture = rt;
        screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        cam.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        screenShot.Apply();
        if (rawImage.texture)
            Destroy(rawImage.texture);
        rawImage.texture = screenShot;
        cam.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors
        Destroy(rt);

        capture.SetFrame(screenShot);

        flash.color = new Color(1, 1, 1, 0);
        flash.DOFade(1f, .2f).SetEase(ease).SetLoops(2, LoopType.Yoyo);
        DOVirtual.DelayedCall(.2f, () => rawImage.gameObject.SetActive(true));
        SManager.Ins.PlayShutter();

        StartCoroutine(stopSnap());
    }

    IEnumerator stopSnap() {
        yield return new WaitForEndOfFrame();
        
        capture.SaveJpeg(screenShot);
        //cam.enabled = false;
        //rawImage.transform.parent.parent.gameObject.SetActive(true);
    }

    public void reset() {
        //cam.enabled = false;
        if(screenShot != null)
            Destroy(screenShot);
        screenShot = null;
        rawImage.gameObject.SetActive(false);
        //rawImage.transform.parent.parent.gameObject.SetActive(false);
    }
}
