using UnityEngine;
using System.Collections;

public class FittingController : MonoBehaviour {

    protected static FittingController instance = null;
    public static FittingController Instance {
        get {
            return instance;
        }
    }
    

    ClothSetting setting;

    void Awake() {
        instance = this;

        setting = ClothSetting.Instance;
    }

	void Start () {
        Cursor.visible = false;
    }

    void Update() {

        if (Input.GetMouseButtonUp(1) && setting) {
            if (setting.gameObject.activeInHierarchy) {
                setting.gameObject.SetActive(false);
                Cursor.visible = false;
            } else {
                setting.gameObject.SetActive(true);
                Cursor.visible = true;
            }
        }
        if(!setting && ClothSetting.Instance) setting = ClothSetting.Instance;

    }
}
