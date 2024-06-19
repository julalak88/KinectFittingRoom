using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ClothSetting : MonoBehaviour {

    protected static ClothSetting instance = null;
    public static ClothSetting Instance {
        get {
            return instance;
        }
    }

    [HideInInspector]
    public ModelControl curModel;
    [HideInInspector]
    public GlassesController curGlasses;
    [HideInInspector]
    public Slider bodySlider, armSlider, legSlider, xSlider, ySlider, zSlider, xSliderG, ySliderG, zSliderG;
    [HideInInspector]
    public string nameModel, nameGlasses;

    string desktopPath;
    Text clothText, glassesText;
    
    void Awake() {
        desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
        instance = this;

        clothText = ((RectTransform)transform).Find("ClothName").GetComponent<Text>();
        glassesText = ((RectTransform)transform).Find("GlassesName").GetComponent<Text>();

        bodySlider = ((RectTransform)transform).Find("Body Scale Factor/Slider").GetComponent<Slider>();
        armSlider = ((RectTransform)transform).Find("Arm Scale Factor/Slider").GetComponent<Slider>();
        legSlider = ((RectTransform)transform).Find("Leg Scale Factor/Slider").GetComponent<Slider>();

        xSlider = ((RectTransform)transform).Find("Position Offset XYZ/SliderX").GetComponent<Slider>();
        ySlider = ((RectTransform)transform).Find("Position Offset XYZ/SliderY").GetComponent<Slider>();
        zSlider = ((RectTransform)transform).Find("Position Offset XYZ/SliderZ").GetComponent<Slider>();

        xSliderG = ((RectTransform)transform).Find("Position Offset XYZ Glasses/SliderX").GetComponent<Slider>();
        ySliderG = ((RectTransform)transform).Find("Position Offset XYZ Glasses/SliderY").GetComponent<Slider>();
        zSliderG = ((RectTransform)transform).Find("Position Offset XYZ Glasses/SliderZ").GetComponent<Slider>();
    }

    void Start() {
        gameObject.SetActive(false);
    }
    
    public void onBodyChanged() {
        if (curModel) {
            curModel.asc.bodyScaleFactor = bodySlider.value;
            ES2.Save(bodySlider.value, desktopPath+"/SaveFittingRoom.txt?tag=cloth_data_body_" + nameModel);
        }
    }

    public void onArmChanged() {
        if (curModel) {
            curModel.asc.armScaleFactor = armSlider.value;
            ES2.Save(armSlider.value, desktopPath + "/SaveFittingRoom.txt?tag=cloth_data_arm_" + nameModel);
        }
    }

    public void onLegChanged() {
        if (curModel) {
            curModel.asc.legScaleFactor = legSlider.value;
            ES2.Save(legSlider.value, desktopPath + "/SaveFittingRoom.txt?tag=cloth_data_leg_" + nameModel);
        }
    }

    public void onXOffsetChanged() {
        if (curModel) {
            ES2.Save(xSlider.value, desktopPath + "/SaveFittingRoom.txt?tag=cloth_data_xOffset_" + nameModel);
            curModel.setOffsetX(xSlider.value);
        }
    }

    public void onYOffsetChanged() {
        if (curModel) {
            ES2.Save(ySlider.value, desktopPath + "/SaveFittingRoom.txt?tag=cloth_data_yOffset_" + nameModel);
            curModel.setOffsetY(ySlider.value);
        }
    }

    public void onZOffsetChanged() {
        if (curModel) {
            ES2.Save(zSlider.value, desktopPath + "/SaveFittingRoom.txt?tag=cloth_data_zOffset_" + nameModel);
            curModel.setOffsetZ(zSlider.value);
        }
    }

    public void onGlassesXOffsetChanged() {
        if (curGlasses) {
            ES2.Save(xSliderG.value, desktopPath + "/SaveFittingRoom.txt?tag=glasses_data_xOffset_" + nameGlasses);
            curGlasses.setOffsetX(xSliderG.value);
        }
    }

    public void onGlassesYOffsetChanged() {
        if (curGlasses) {
            ES2.Save(ySliderG.value, desktopPath + "/SaveFittingRoom.txt?tag=glasses_data_yOffset_" + nameGlasses);
            curGlasses.setOffsetY(ySliderG.value);
        }
    }

    public void onGlassesZOffsetChanged() {
        if (curGlasses) {
            ES2.Save(zSliderG.value, desktopPath + "/SaveFittingRoom.txt?tag=glasses_data_zOffset_" + nameGlasses);
            curGlasses.setOffsetZ(zSliderG.value);
        }
    }

    public void reset() {
        if (curModel) curModel.reset();
        if (curGlasses) curGlasses.reset();
    }

    public void setModel(ModelControl model) {
        curModel = model;
        nameModel = curModel.name;
        clothText.text = nameModel;
    }

    public void setGlassesModel(GlassesController model) {
        curGlasses = model;
        nameGlasses = curGlasses.name;
        glassesText.text = nameGlasses;
    }

    public void loadCloth(string _name, float body, float arm, float leg) {
        nameModel = _name;
        if (ES2.Exists(desktopPath + "/SaveFittingRoom.txt?tag=cloth_data_body_" + nameModel)) {
            bodySlider.value = ES2.Load<float>(desktopPath + "/SaveFittingRoom.txt?tag=cloth_data_body_" + nameModel);
        }else bodySlider.value = body;

        if (ES2.Exists(desktopPath + "/SaveFittingRoom.txt?tag=cloth_data_arm_" + nameModel)) {
            armSlider.value = ES2.Load<float>(desktopPath + "/SaveFittingRoom.txt?tag=cloth_data_arm_" + nameModel);
        }else armSlider.value = arm;

        if (ES2.Exists(desktopPath + "/SaveFittingRoom.txt?tag=cloth_data_leg_" + nameModel)) {
            legSlider.value = ES2.Load<float>(desktopPath + "/SaveFittingRoom.txt?tag=cloth_data_leg_" + nameModel);
        }else legSlider.value = leg;

        if (ES2.Exists(desktopPath + "/SaveFittingRoom.txt?tag=cloth_data_xOffset_" + nameModel)) {
            xSlider.value = ES2.Load<float>(desktopPath + "/SaveFittingRoom.txt?tag=cloth_data_xOffset_" + nameModel);
        } else xSlider.value = 0;

        if (ES2.Exists(desktopPath + "/SaveFittingRoom.txt?tag=cloth_data_yOffset_" + nameModel)) {
            ySlider.value = ES2.Load<float>(desktopPath + "/SaveFittingRoom.txt?tag=cloth_data_yOffset_" + nameModel);
        }else ySlider.value = 0;

        if (ES2.Exists(desktopPath + "/SaveFittingRoom.txt?tag=cloth_data_zOffset_" + nameModel)) {
            zSlider.value = ES2.Load<float>(desktopPath + "/SaveFittingRoom.txt?tag=cloth_data_zOffset_" + nameModel);
        } else zSlider.value = 0;
    }

    public void loadGlasses(string _name) {
        nameGlasses = _name;
        
        if (ES2.Exists(desktopPath + "/SaveFittingRoom.txt?tag=glasses_data_xOffset_" + nameGlasses)) {
            xSliderG.value = ES2.Load<float>(desktopPath + "/SaveFittingRoom.txt?tag=glasses_data_xOffset_" + nameGlasses);
        } else xSliderG.value = 0;

        if (ES2.Exists(desktopPath + "/SaveFittingRoom.txt?tag=glasses_data_yOffset_" + nameGlasses)) {
            ySliderG.value = ES2.Load<float>(desktopPath + "/SaveFittingRoom.txt?tag=glasses_data_yOffset_" + nameGlasses);
        } else ySliderG.value = 0;

        if (ES2.Exists(desktopPath + "/SaveFittingRoom.txt?tag=glasses_data_zOffset_" + nameGlasses)) {
            zSliderG.value = ES2.Load<float>(desktopPath + "/SaveFittingRoom.txt?tag=glasses_data_zOffset_" + nameGlasses);
        } else zSliderG.value = 0;
    }

    public void setSlider() {
        if (curModel) {
            bodySlider.value = curModel.asc.bodyScaleFactor;
            armSlider.value = curModel.asc.armScaleFactor;
            legSlider.value = curModel.asc.legScaleFactor;
            xSlider.value = (curModel.offset != null) ? curModel.offset.x : 0;
            ySlider.value = (curModel.offset != null) ? curModel.offset.y : 0;
            zSlider.value = (curModel.offset != null) ? curModel.offset.z : 0;
        }
    }

    public void setSliderGlasses() {
        xSliderG.value = (curGlasses != null) ? curGlasses.transform.localPosition.x : 0;
        ySliderG.value = (curGlasses != null) ? curGlasses.transform.localPosition.y : 0;
        zSliderG.value = (curGlasses != null) ? curGlasses.transform.localPosition.z : 0;
    }
}
