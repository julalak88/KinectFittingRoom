using UnityEngine;
using System.Collections;
using System;

public class FittingTest : MonoBehaviour {

    public string mainPath;
    public string[] path;

    KinectManager km;
    bool detected = false;
    ModelControl[] curShirt;

    void Start() {
        curShirt = new ModelControl[path.Length];
        km = KinectManager.Instance;
    }

    void Update() {

        if (km && km.IsInitialized()) {
            if (km.IsUserDetected()) {
                if (!detected) {
                    detected = true;
                    createCloth();
                }
            } else {
                if (detected) {
                    detected = false;
                    for(int i=0; i<curShirt.Length; i++)
                        curShirt[i].StopModel();
                    Array.Clear(curShirt, 0, curShirt.Length);
                }
            }
        }
    }

    public void createCloth() {
        for(int i=0; i< path.Length; i++) {
            GameObject cloth = (GameObject)Instantiate((GameObject)Resources.Load(mainPath+path[i], typeof(GameObject)));
            curShirt[i] = cloth.GetComponent<ModelControl>();
            //curShirt.startModel();
        }
    }
}
