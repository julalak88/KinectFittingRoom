using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using DG.Tweening;
using System;

public class ClothCreator : MonoBehaviour {

    public PostProcessVolume postprocess;
    public int index = -1;
    public ClothSet[] set;

    ModelControl curCloth;
    ModelHatController curHat;
    GlassesController curGlasses;
    Bloom bloomLayer = null;
    float bloom = 0;
    Tweener tweenBloom;
    List<int> randomList = new List<int>();

    private void Awake() {
        postprocess.profile.TryGetSettings(out bloomLayer);
        for (int i = 0; i < set.Length; i++) {
            randomList.Add(i);
        }
    }

    private void onUpdateBloom(float value) {
        bloomLayer.intensity.value = value;
    }

    public void CreateCloth() {
        ClearCloth();

        int ind = index;
        if (index == -1) {
            //rnd = UnityEngine.Random.Range(0, set.Length);
            int rnd = UnityEngine.Random.Range(0, randomList.Count);
            ind = randomList[rnd];
            randomList.RemoveAt(rnd);
            if (randomList.Count == 0) {
                for (int i = 0; i < set.Length; i++)
                    randomList.Add(i);
            }
        }

        ClothSet _set = set[ind];
        GameObject obj = null;
        if (_set.cloth != null) {
            obj = Instantiate(_set.cloth);
            curCloth = obj.GetComponent<ModelControl>();
        }
        if (_set.hat != null) {
            obj = Instantiate(_set.hat);
            curHat = obj.GetComponent<ModelHatController>();
        }
        if (_set.glasses != null) {
            obj = Instantiate(_set.glasses);
            curGlasses = obj.GetComponent<GlassesController>();
        }
        if (tweenBloom != null)
            tweenBloom.Kill();
        tweenBloom = null;
        bloom = 0;
        bloomLayer.intensity.value = bloom;
        tweenBloom = DOVirtual.Float(bloom, 10, 2, onUpdateBloom).SetLoops(2, LoopType.Yoyo);
    }
    
    public void ClearCloth() {
        if (curCloth) curCloth.StopModel();
        curCloth = null;

        if (curHat) curHat.StopModel();
        curHat = null;

        if (curGlasses) curGlasses.StopModel();
        curGlasses = null;
    }
}

[Serializable]
public class ClothSet {
    public GameObject cloth;
    public GameObject hat;
    public GameObject glasses;
}
