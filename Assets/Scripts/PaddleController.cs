using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleController : MonoBehaviour
{
    Rigidbody2D paddleRB;
    public bool okForTouchScreen = false, gravControlInUse = false;
    public float cooldownTimer = 0, gravDurationTimer = 0, howLongToGravPull = 0.5f;
    public GameMaster gM;
    private GameObject gravityIndicator;
    void Start()
    {
        gravityIndicator = GameObject.FindGameObjectWithTag("Gravity Indicator");
        paddleRB = GetComponent<Rigidbody2D>();
        gM = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        //check if device supports touch or not
        if (Input.touchSupported == true)
        {
            print("touch screen support detected");
            okForTouchScreen = true;
        }
        else
        {
            print("touch screen support NOT detected");
        }
    }

    void Update()
    {
        PaddleMovement();
        gravControl();
    }
    //make the paddle follow the mouse or touch up and down
    private void PaddleMovement()
    {
        Vector2 playerInputPosition;
        //for device with no touch capability
        if (okForTouchScreen == false)
        {
            playerInputPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector2 paddlePositioner = paddleRB.position;

            paddlePositioner.y = Mathf.Lerp(paddlePositioner.y, playerInputPosition.y, 10);
            paddlePositioner.y = Mathf.Clamp(paddlePositioner.y, -3.7f, 3.7f);
            paddleRB.position = paddlePositioner;
        }
        //for device with touch capbility
        else
        {
            if (Input.touchCount > 0)
            {
                playerInputPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

                Vector2 paddlePositioner = paddleRB.position;

                paddlePositioner.y = Mathf.Lerp(paddlePositioner.y, playerInputPosition.y, 10);
                paddlePositioner.y = Mathf.Clamp(paddlePositioner.y, -3.7f, 3.7f);
                paddleRB.position = paddlePositioner;
            }
            //windows devices that support touch but don't have an active touch device in testing had some input recognition issues, so adding mouse input here ended up not being redundant
            else
            {
                playerInputPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                Vector2 paddlePositioner = paddleRB.position;

                paddlePositioner.y = Mathf.Lerp(paddlePositioner.y, playerInputPosition.y, 10);
                paddlePositioner.y = Mathf.Clamp(paddlePositioner.y, -3.7f, 3.7f);
                paddleRB.position = paddlePositioner;
            }
        }
    }

    //apply gravity in a direction indicated by paddle position and use if the cooldown timer has finished
    private void gravControl()
    {
        if (gravControlInUse == false)
        {
            //if the player right clicks / uses two fingers, turn on the grav control bool and gravity turns on toward the left for a set amount of time
            if (Input.touchCount > 1 || Input.GetMouseButtonUp(1))
            {
                Debug.Log("grav control input detected");
                gravControlInUse = true;
                gM.gravityChange(true, false, false, false, false);
                gravDurationTimer = howLongToGravPull;
                cooldownTimer = 8f;
                gM.balls = GameObject.FindGameObjectsWithTag("Ball");
                foreach (GameObject b in gM.balls)
                {
                    b.GetComponent<Rigidbody2D>().gravityScale = 3;
                }
            }
        }
        // turn off the function if the timer hits zero, and run the timer down per second to zero
        if (gravDurationTimer <= 0)
        {
            gravDurationTimer = 0;
            gM.gravityChange(false, false, false, false, false);
            gM.balls = GameObject.FindGameObjectsWithTag("Ball");
            foreach (GameObject b in gM.balls)
            {
                b.GetComponent<Rigidbody2D>().gravityScale = 0;
            }
        }
        if (gravDurationTimer > 0)
        {
            gravDurationTimer -= Time.deltaTime;
        }
        if (cooldownTimer <= 0)
        {
            cooldownTimer = 0;
            gravControlInUse = false;
        }
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
        PaddleGlow();
    }
    //control paddle graphic to let player know they can use Gravity pulse
    private void PaddleGlow()
    {
        if (cooldownTimer == 0 && gravityIndicator.activeSelf == false)
        {
            gravityIndicator.SetActive(true);
        }
        if (cooldownTimer != 0 && gravityIndicator.activeSelf == true)
        {
            gravityIndicator.SetActive(false);
        }
    }
    //shrink the paddle by the given percent on the y axis
    public void ShrinkPaddle(float newSizePercent)
    {
        Vector3 newSize;
        newSize.x = this.transform.localScale.x;
        newSize.y = this.transform.localScale.y * newSizePercent;
        newSize.z = this.transform.localScale.z;
        this.transform.localScale = newSize;
    }
}
