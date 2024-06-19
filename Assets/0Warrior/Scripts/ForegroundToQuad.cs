using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForegroundToQuad : MonoBehaviour {

    private Renderer thisRenderer;

    void Start() {
        thisRenderer = GetComponent<Renderer>();
        
    }

    void Update() {
        if (thisRenderer && thisRenderer.sharedMaterial.mainTexture == null) {

            KinectManager kinectManager = KinectManager.Instance;

            if (kinectManager) {
                thisRenderer.sharedMaterial.mainTexture = kinectManager.GetUsersClrTex();
            }
        }
    }


    void OnApplicationPause(bool isPaused) {
        // fix for app pause & restore (UWP)
        if (isPaused && thisRenderer && thisRenderer.sharedMaterial.mainTexture != null) {
            thisRenderer.sharedMaterial.mainTexture = null;
        }
    }
}
