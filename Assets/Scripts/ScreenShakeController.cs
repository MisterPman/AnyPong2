using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeController : MonoBehaviour
{
    private float shakeTimeRemaining, ShakePower, ShakeFadeTime;
    Vector3 camerapos;

    private void Start()
    {
        //grab the starting camera position. it won't change throughout gameplay so just once is fine.
        camerapos.x = 0;
        camerapos.y = 0;
        camerapos.z = -10;
    }
    private void Update()
    {
        //necessary to continuously update camera position to recenter after a shake
        this.transform.position = camerapos;
    }
    private void LateUpdate()
    {
        //smoothly level off the shake instead of sudden start/stop
        if (shakeTimeRemaining > 0)
        {
            shakeTimeRemaining -= Time.deltaTime;

            float xShake = Random.Range(-1f, 1f) * ShakePower;
            float yShake = Random.Range(-1f, 1f) * ShakePower;

            transform.position += new Vector3(xShake, yShake, 0f);

            ShakePower = Mathf.MoveTowards(ShakePower, 0f, ShakeFadeTime * Time.deltaTime);

        }
    }
    //shake the screen for prescribed length of time and power
    public void StartShake(float lengthOFShake, float powerOfShake)
    {
        shakeTimeRemaining = lengthOFShake;
        ShakePower = powerOfShake;
        ShakeFadeTime = powerOfShake / lengthOFShake;
    }
}
