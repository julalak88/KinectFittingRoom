using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassesController : MonoBehaviour {

    public Material[] materials;
    public float dissolveTime = 4f;
    public Vector3 scale;

    ClothSetting setting;

    private void Awake() {

        setting = ClothSetting.Instance;

        for (int i = 0; i < materials.Length; i++) {
            materials[i].SetFloat("Vector1_68C5C62B", 1);
        }
        transform.parent = GameObject.Find("FaceRigged/Glasses").transform;
        transform.localEulerAngles = Vector3.zero;
        transform.localScale = scale;

        if (setting) {
            setting.curGlasses = null;
            setting.loadGlasses(name);
        }

    }

    void Start() {

        StartCoroutine(setupModel());
    }

    IEnumerator setupModel() {
        yield return new WaitForSeconds(0.3f);

        if (setting) {

            setting.setGlassesModel(this);

            transform.localPosition = new Vector3(setting.xSliderG.value,
               setting.ySliderG.value,
               setting.zSliderG.value);

            for (int i = 0; i < materials.Length; i++) {
                materials[i].DOFloat(0, "Vector1_68C5C62B", dissolveTime);
            }


        }
    }

    public void setOffsetX(float val) {
        transform.localPosition = new Vector3(val,
                transform.localPosition.y,
                transform.localPosition.z);
    }

    public void setOffsetY(float val) {
        transform.localPosition = new Vector3(transform.localPosition.x,
                val, transform.localPosition.z);
    }

    public void setOffsetZ(float val) {
        transform.localPosition = new Vector3(transform.localPosition.x,
                transform.localPosition.y, val);
    }

    public void reset() {
        transform.localPosition = Vector3.zero; ;
        if (setting)
            setting.setSliderGlasses();
    }

    public void StopModel() {
        Destroy(gameObject);
    }
}
