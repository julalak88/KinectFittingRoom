using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SnapCollider : MonoBehaviour {

    public Image fillCir;
    public int index;
    public float delay = 3f;

    float fill = 0;
    bool canFill = true;

    private void Awake() {
        fillCir.fillAmount = fill;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
         SManager.Ins.PlayTouch();
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (!canFill) return;

        fill += Time.deltaTime;
        fillCir.fillAmount = fill / delay;
        if(fill > delay) {
            canFill = false;
            GameManager.Ins.OnSnapTrigged(index);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        fill = 0;
        fillCir.fillAmount = fill;
        canFill = true;
    }

    public void reset() {
        fill = 0;
        fillCir.fillAmount = fill;
        canFill = true;
    }

    //public void Hide() {
    //    gameObject.SetActive(false);
    //}

    //public void Show() {
    //    gameObject.SetActive(true);
    //    transform.localScale = Vector3.zero;
    //    transform.DOScale(1, 1).SetEase(Ease.OutBack);
    //}
}
