using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opening : MonoBehaviour {

    public GameObject screenSaver;//, opening;
    public Animator titleAnim;

    private void Awake() {
        screenSaver.SetActive(false);
    }

    private void Start() {
        StartScreenSaver();
    }

    public void StartScreenSaver() {
        screenSaver.SetActive(true);
        titleAnim.Play("TitleFadein", 0);
        //opening.SetActive(false);
    }

    public void StartOpening() {
        //screenSaver.SetActive(false);
        //opening.SetActive(true);
        titleAnim.Play("TitleFadeout", 0);
        //SManager.Ins.PlayOpen();
        DOVirtual.DelayedCall(2, () => screenSaver.SetActive(false), false);
    }
}
