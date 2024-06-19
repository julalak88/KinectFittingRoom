using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CaptureCamera : MonoBehaviour {

    public int resWidth, resHeight;
    public RawImage image;
    public Text codeText, nameText, secretText;

    //Camera cam;
    string desktopPath, directoryPath;
    string fileName;

    private void Awake() {

        //cam = GetComponent<Camera>();

        desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
        directoryPath = desktopPath + "/Warrior IMAGES";
        if (!Directory.Exists(directoryPath)) {
            Directory.CreateDirectory(directoryPath);
        }
    }

    public void SetFrame(Texture2D texture) {
        if (image.texture) Destroy(image.texture);
        image.texture = null;
        image.texture = texture;
        //codeText.text = code;
        //nameText.text = name;
        //secretText.text = secret;
        fileName = DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss");
    }

    public void SaveJpeg(Texture2D screenShot) {
        //RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        //cam.targetTexture = rt;
        //Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.ARGB32, true);
        //cam.Render();
        //RenderTexture.active = rt;
        //screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        //screenShot.Apply();

        string folder = directoryPath + "/" + System.DateTime.Now.ToString("yyyy-MM-dd");
        if (!Directory.Exists(folder)) {
            Directory.CreateDirectory(folder);
        }
        byte[] bytes = screenShot.EncodeToPNG();
        File.WriteAllBytes(folder+"/"+fileName+".png", bytes);

        //cam.targetTexture = null;
        //RenderTexture.active = null; // JC: added to avoid errors
        //Destroy(rt);
    }
}
