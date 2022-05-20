using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScaler : MonoBehaviour
{
    public GameObject allParent;
    public Camera mainCam;
    void Start()
    {
        mainCam = Camera.main;
        allParent = GameObject.FindGameObjectWithTag("Sprite Holder");
        ScaleSpritesToFitResolution();
    }

    private void ScaleSpritesToFitResolution()
    {
        //1. grab the screen aspect of current device
        Vector2 deviceScrnRes = new Vector2(Screen.width, Screen.height);
        //print(deviceScrnRes);

        float scrnht = Screen.height;
        float scrnwth = Screen.width;

        float DEVICE_SCREEN_ASPECT = scrnwth / scrnht;
        // print("DEVICE_SCREEN_ASPECT: " + DEVICE_SCREEN_ASPECT.ToString());

        //2. Set Main Cam aspect = current device aspect

        mainCam.aspect = DEVICE_SCREEN_ASPECT;

        //3. Scale sprites to match the camera's size
        float camH = 100f * mainCam.orthographicSize * 2.0f;
        float camW = camH * DEVICE_SCREEN_ASPECT;
        // print("camH: " + camH.ToString());
        // print("camW: " + camW.ToString());

        //Grab the Sprite Holder size;
        SpriteRenderer SpriteHolderSR = allParent.GetComponent<SpriteRenderer>();
        float allParH = SpriteHolderSR.sprite.rect.height;
        float allParW = SpriteHolderSR.sprite.rect.width;

        // print("SpriteHolderH: " + allParH.ToString());
        // print("SpriteHolderW: " + allParW.ToString());

        //calculate Ratio for scaling...

        float allPar_scale_ratio_h = camH / allParH;
        float allPar_scale_ratio_w = camW / allParW;

        allParent.transform.localScale = new Vector3(allPar_scale_ratio_w, allPar_scale_ratio_h, 1);
    }
}

