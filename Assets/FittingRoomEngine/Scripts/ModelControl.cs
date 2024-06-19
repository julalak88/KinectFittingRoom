using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class ModelControl : MonoBehaviour {

    public Material[] materials;
    public float dissolveTime = 2f;

    [HideInInspector]
    public AvatarController ac;
    [HideInInspector]
    public AvatarScaler asc;

    public Vector3 offset;

    [Range(0.8f, 1.3f)]
    public float bodyScale = 1f;
    [Range(0.8f, 1.3f)]
    public float armScale = 1f;
    [Range(0.8f, 1.3f)]
    public float legScale = 1f;
    [HideInInspector]
    public float oriBody, oriArm, oriLeg;

    Camera bgCam, forCam;
	KinectManager km;

    Vector3 oriOffset;
    
	GameObject offsetObj;
    ClothSetting setting;

    void Awake() {

        for (int i = 0; i < materials.Length; i++) {
            materials[i].SetFloat("_DissolveAmount", 1);
        }

        transform.position = Vector3.zero;
        transform.rotation = Quaternion.Euler(0, 180f, 0);

        setting = ClothSetting.Instance;
        km = KinectManager.Instance;
        bgCam = GameObject.Find("Kinect/BackgroundCamera1").GetComponent<Camera>();
        forCam = GameObject.Find("Main Camera").GetComponent<Camera>();

        if (offset != null) oriOffset = offset;

        oriBody = bodyScale;
        oriArm = armScale;
        oriLeg = legScale;

        offsetObj = new GameObject();
        offsetObj.transform.rotation = Quaternion.Euler(0, 180, 0);

        if (setting) {
            setting.curModel = null;
            setting.loadCloth(this.name, oriBody, oriArm, oriLeg);
        }
            
    }

    void Start() {
        ac = gameObject.AddComponent<AvatarController>();
        ac.posRelativeToCamera = bgCam;
        ac.posRelOverlayColor = (forCam != null);
        ac.mirroredMovement = true;
        ac.verticalMovement = true;
        ac.smoothFactor = 0f;
        
        //ac.offsetNode = offsetObj;
        //ac.Awake();

        km.avatarControllers.Add(ac);
        ac.SuccessfulCalibration(km.GetPrimaryUserID(), false);
        

        asc = gameObject.AddComponent<AvatarScaler>();
        asc.mirroredAvatar = true;
        asc.continuousScaling = true;
        asc.foregroundCamera = forCam;
        asc.Start();

        StartCoroutine(setupModel());
    }

    IEnumerator setupModel() {
        yield return new WaitForSeconds(0.3f);

        if (setting) {
            setting.setModel(this);

            asc.bodyScaleFactor = setting.bodySlider.value;
            asc.armScaleFactor = setting.armSlider.value;
            asc.legScaleFactor = setting.legSlider.value;

            offsetObj.transform.position = new Vector3(setting.xSlider.value,
               setting.ySlider.value,
               setting.zSlider.value);

            ac.offsetNode = offsetObj;

            for (int i = 0; i < materials.Length; i++) {
                materials[i].DOFloat(0, "_DissolveAmount", dissolveTime);
            }
        }
        
    }

    public void setOffsetX(float val) {
        if (offsetObj)
            offsetObj.transform.position = new Vector3(val,
                offsetObj.transform.position.y,
                offsetObj.transform.position.z);
    }

    public void setOffsetY(float val) {
        if(offsetObj)
        offsetObj.transform.position = new Vector3(offsetObj.transform.position.x,
            val, offsetObj.transform.position.z);
    }

    public void setOffsetZ(float val) {
        if (offsetObj)
            offsetObj.transform.position = new Vector3(offsetObj.transform.position.x,
                offsetObj.transform.position.y, val);
    }

    public void reset() {
        asc.bodyScaleFactor = oriBody;
        asc.armScaleFactor = oriArm;
        asc.legScaleFactor = oriLeg;
        offsetObj.transform.position = oriOffset;
        if(setting)
            setting.setSlider();
    }

	public void StopModel() {
        if (km.avatarControllers.Contains(ac))
            km.avatarControllers.Remove(ac);
        if (setting)
            setting.curModel = null;
        Destroy(gameObject);
		Destroy (offsetObj);
	}
}
