using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandTracker: MonoBehaviour {

    public Camera foregroundCamera;
    public int playerIndex = 0;

    public RectTransform handL, handR;

    GameObject handLImg, handRImg;

    private void Awake() {
        handLImg = handL.Find("Image").gameObject;
        handRImg = handR.Find("Image").gameObject;
        HideHands();
    }

    public void ShowHands() {
        handLImg.SetActive(true);
        handRImg.SetActive(true);
    }

    public void HideHands() {
        handLImg.SetActive(false);
        handRImg.SetActive(false);
    }

    void Update() {
        KinectManager manager = KinectManager.Instance;

        if (manager && manager.IsInitialized() && foregroundCamera) {

            Rect backgroundRect = foregroundCamera.pixelRect;
            PortraitBackground portraitBack = PortraitBackground.Instance;

            if (portraitBack && portraitBack.enabled) {
                backgroundRect = portraitBack.GetBackgroundRect();
            }
            
            long userId = manager.GetUserIdByIndex(playerIndex);

            KinectInterop.JointType trackedJoint = KinectInterop.JointType.HandLeft;
            int iJointIndex = (int)trackedJoint;

            if (manager.IsJointTracked(userId, iJointIndex)) {

                Vector3 posJoint = manager.GetJointPosColorOverlay(userId, iJointIndex, foregroundCamera, backgroundRect);

                if (posJoint != Vector3.zero) {
                    
                    handL.anchoredPosition = foregroundCamera.WorldToScreenPoint(posJoint); ;

                }
            }

            trackedJoint = KinectInterop.JointType.HandRight;
            iJointIndex = (int)trackedJoint;

            if (manager.IsJointTracked(userId, iJointIndex)) {

                Vector3 posJoint = manager.GetJointPosColorOverlay(userId, iJointIndex, foregroundCamera, backgroundRect);

                if (posJoint != Vector3.zero) {

                    handR.anchoredPosition = foregroundCamera.WorldToScreenPoint(posJoint); ;

                }
            }
        }
    }
}
