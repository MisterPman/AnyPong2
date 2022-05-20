using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehavior : MonoBehaviour
{
    private GameMaster gM;
    [SerializeField]
    float currentXVelocity, currentYVelocity;
    public int ballSpeed;
    private BlockBehavior blockBhv;
    private GameObject blockMaster;
    private Rigidbody2D ballRB;
    private bool courseCorrectStarted = false;

    void Start()
    {
        gM = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        blockMaster = GameObject.FindGameObjectWithTag("BlockMaster");
        blockBhv = blockMaster.GetComponent<BlockBehavior>();
        ballRB = gameObject.GetComponent<Rigidbody2D>();
    }

    //if the ball stays too close to the top or bottom of the screen for too long, activate gravity and set the scale of the ball appropriately to smoothly move it away from the edge of the screen.
    public IEnumerator CourseCorrection()
    {
        if (courseCorrectStarted == false)
        {
            if (gameObject.transform.position.y > 2.2)
            {
                courseCorrectStarted = true;
                yield return new WaitForSeconds(1.5f);
                if (gameObject.transform.position.y > 2.2)
                {
                    ballRB.gravityScale = 1;
                    gM.gravityChange(false, false, false, false, true);
                    yield return new WaitForSeconds(1f);
                    gM.gravityChange(false, false, false, false, false);
                    ballRB.gravityScale = 0;
                }
                courseCorrectStarted = false;
            }
            if (gameObject.transform.position.y < -2.2)
            {
                courseCorrectStarted = true;
                yield return new WaitForSeconds(1.5f);
                if (gameObject.transform.position.y < -2.2)
                {
                    ballRB.gravityScale = -1;
                    gM.gravityChange(false, false, false, false, true);
                    yield return new WaitForSeconds(1f);
                    gM.gravityChange(false, false, false, false, false);
                    ballRB.gravityScale = 0;
                }
                courseCorrectStarted = false;
            }
        }
    }
    //check if the ball is too high or low before activating the coroutine.
    private void Update()
    {
        if (gameObject.transform.position.y > 2.2 || gameObject.transform.position.y < -2.2)
        {
            StartCoroutine(nameof(CourseCorrection));
        }
    }
    //set an object moving toward the bumper at a predetermined speed
    public void LaunchObject(GameObject target)
    {
        ballRB = target.GetComponent<Rigidbody2D>();
        ballRB.AddForce(new Vector2(-8, 0) * ballSpeed, ForceMode2D.Force);
    }
    //check whether the collided object is a block. if so, activate the block collision script and give the player a point
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Block") == true || other.gameObject.CompareTag("BossBlock") == true)
        {
            GameObject collidedBlock = other.gameObject;
            blockBhv.BlockCollision(collidedBlock);
            gM.GetAPoint(1);
        }
    }
}

