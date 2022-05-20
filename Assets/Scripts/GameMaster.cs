using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    private GameObject gameOverScreen, victoryScreen, life1, life2, life3, blockMaster, resetBallL, resetBallR, firstBall, paddle, fireworks;
    public GameObject[] balls;
    public GameObject blockBurst;
    private BlockBehavior blockBhv;
    private BallBehavior ballBhv;
    private ScreenShakeController scrnShk;
    private PaddleController paddleCtrl;
    public int playerDeathCount = 0, points = 0;
    public bool levelStarted = false;
    void Start()
    {
        gameOverScreen = GameObject.FindGameObjectWithTag("GameOverPanel");
        victoryScreen = GameObject.FindGameObjectWithTag("VictoryPanel");
        blockMaster = GameObject.FindGameObjectWithTag("BlockMaster");
        blockBhv = blockMaster.GetComponent<BlockBehavior>();
        scrnShk = Camera.main.GetComponent<ScreenShakeController>();
        firstBall = GameObject.FindGameObjectWithTag("Ball");
        ballBhv = firstBall.GetComponent<BallBehavior>();
        paddle = GameObject.FindGameObjectWithTag("Player");
        paddleCtrl = paddle.GetComponent<PaddleController>();
        fireworks = GameObject.FindGameObjectWithTag("FireWorks");
        life1 = GameObject.FindGameObjectWithTag("Life1");
        life2 = GameObject.FindGameObjectWithTag("Life2");
        life3 = GameObject.FindGameObjectWithTag("Life3");
        resetBallL = GameObject.FindGameObjectWithTag("ResetBallLeft");
        resetBallR = GameObject.FindGameObjectWithTag("ResetBallRight");
        blockBurst = GameObject.FindGameObjectWithTag("BlockBurst");
        victoryScreen.SetActive(false);
        gameOverScreen.SetActive(false);
        life1.SetActive(false);
        life2.SetActive(false);
        life3.SetActive(false);
        fireworks.SetActive(false);
        levelStarted = false;

        //turn off gravity for later functionality
        Physics2D.gravity = new Vector2(0, 0);
        firstBall.GetComponent<Rigidbody2D>().gravityScale = 1;

    }
    //track how many lives the player lost. lose the game if more than limit
    public void playerDeath(int death)
    {
        ShakeTheScreen(.5f, 1f);
        playerDeathCount += 1;
        if (playerDeathCount == 1)
        {
            life1.SetActive(true);
        }
        if (playerDeathCount == 2)
        {
            life2.SetActive(true);
        }
        if (playerDeathCount == 3)
        {
            life3.SetActive(true);
        }
        if (playerDeathCount > 3)
        {
            //remove all game objects and clear UI. Show Game Over screen
            GameWipe();
            gameOverScreen.SetActive(true);
            life3.SetActive(false);
            life2.SetActive(false);
            life1.SetActive(false);
            //resetBallL.SetActive(false);
            //resetBallR.SetActive(false);
        }
    }
    //set gravity to a certain direction. Left, diagonal up, or diagonal down.Only gleft and gdown are currently used.
    public void gravityChange(bool gleft, bool gright, bool gleftUp, bool gleftDown, bool gdown)
    {
        if (gleft == true)
        {
            Physics2D.gravity = new Vector2(-9.8f, 0);
        }
        if (gleftUp == true)
        {
            Physics2D.gravity = new Vector2(-4.9f, 4.9f);
        }
        if (gleftDown == true)
        {
            Physics2D.gravity = new Vector2(-4.9f, 4.9f);
        }
        if (gright == true)
        {
            Physics2D.gravity = new Vector2(4.9f, 0);
        }
        if (gdown == true)
        {
            Physics2D.gravity = new Vector2(0, -9.8f);
        }
        if (gleft == false && gright == false && gleftDown == false && gleftUp == false && gdown == false)
        {
            Physics2D.gravity = new Vector2(0, 0);
        }
    }
    //reset the target object to the center of the screen and launch it after a short time
    public IEnumerator ObjectReset(GameObject b, int waitTime)
    {
        Vector2 zeroPosition = Vector2.zero;
        b.transform.position = zeroPosition;
        b.SetActive(false);
        b.SetActive(true);
        yield return new WaitForSeconds(waitTime);
        ballBhv.LaunchObject(b);

    }
    //Shake the screen at designated power and length
    public void ShakeTheScreen(float shakeLength, float shakePower)
    {
        scrnShk.StartShake(shakeLength, shakePower);
    }
    //add a point to the player's score, BOSS RELEASE and PLAYER WINS moved to BlockBehavior.destroyBlock bc point system was redundant. more efficient to just track how many blocks are left.
    //check if special functions can be activated
    public void GetAPoint(int getPoint)
    {
        points += getPoint;
        //activate the following features only if challenge level has been chosen
        if (SceneManager.GetActiveScene().name == "Challenge Level 1")
        {
            //shrink the paddle
            if (points == 2)
            {
                paddle.GetComponent<PaddleController>().ShrinkPaddle(0.75f);
            }
            //add health to remaining active blocks and change the color to gray
            if (points == 4)
            {
                foreach (GameObject blck in blockBhv.blocks)
                {
                    if (blck.activeSelf == true)
                    {
                        blockBhv.addHealth(blck, 1);
                        blck.GetComponent<IndividualBlockScript>().colorCheck();
                    }
                }
            }
            //speed up the ball
            if (points == 6)
            {
                firstBall.SetActive(false);
                firstBall.SetActive(true);
                ballBhv.ballSpeed = 65;
                ballBhv.LaunchObject(firstBall);
            }
            //make a new ball
            if (points == 7 || points == 10)
            {
                //refresh ball object because aspect ratio may have changed to adjust for screen size.
                firstBall = GameObject.FindGameObjectWithTag("Ball");
                GameObject bonusBall = Instantiate(firstBall, firstBall.transform.position, firstBall.transform.rotation, GameObject.FindGameObjectWithTag("Sprite Holder").transform);
                bonusBall.SetActive(false);
                bonusBall.SetActive(true);
                ballBhv.LaunchObject(bonusBall);
            }
        }
        //editing level 2 of normal game
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            //add a ball
            if (points == 1)
            {
                //refresh ball object because aspect ratio may have changed to adjust for screen size.
                firstBall = GameObject.FindGameObjectWithTag("Ball");
                GameObject bonusBall = Instantiate(firstBall, firstBall.transform.position, firstBall.transform.rotation, GameObject.FindGameObjectWithTag("Sprite Holder").transform);
                bonusBall.SetActive(false);
                bonusBall.SetActive(true);
                ballBhv.LaunchObject(bonusBall);
            }
        }
        //editing level 3 of normal game
        if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            //add 2 balls
            if (points == 1 || points == 2)
            {
                //refresh ball object because aspect ratio may have changed to adjust for screen size.
                firstBall = GameObject.FindGameObjectWithTag("Ball");
                GameObject bonusBall = Instantiate(firstBall, firstBall.transform.position, firstBall.transform.rotation, GameObject.FindGameObjectWithTag("Sprite Holder").transform);
                bonusBall.SetActive(false);
                bonusBall.SetActive(true);
                ballBhv.LaunchObject(bonusBall);
            }
            //add health to blocks
            if (points == 1)
            {
                foreach (GameObject blck in blockBhv.blocks)
                    if (blck.activeSelf == true)
                    {
                        blockBhv.addHealth(blck, 1);
                        blck.GetComponent<IndividualBlockScript>().colorCheck();
                    }
            }
        }

    }
    //used to start the ball moving from a place designated by player
    public void InitialBallLauncher()
    {
        if (levelStarted == false)
        {
            //tell the game gravity control is already active, else it will activate when the player launches the ball
            paddleCtrl.gravControlInUse = true;
            firstBall.transform.position = new Vector2(paddle.transform.position.x + 1, paddle.transform.position.y);

            if (Input.touchCount > 1)
            {
                levelStarted = true;
                ballBhv.LaunchObject(firstBall);
                paddleCtrl.cooldownTimer = 6f;
            }
            if (Input.GetMouseButtonUp(1))
            {
                levelStarted = true;
                ballBhv.LaunchObject(firstBall);
                paddleCtrl.cooldownTimer = 6f;
            }
        }
    }
    //clear all game objects, turn off unnecessary UI buttons, show the victory screen and start the fireworks
    public void PlayerWins()
    {
        GameWipe();
        victoryScreen.SetActive(true);
        fireworks.SetActive(true);
        //resetBallL.SetActive(false);
        //resetBallR.SetActive(false);
    }
    public void toMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public void restartCurrentScene()
    {
        int thisScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(thisScene);
    }
    public void nextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    //deactivate all active game objects
    public void GameWipe()
    {
        foreach (GameObject block in blockBhv.blocks)
        {
            block.SetActive(false);
        }
        blockBhv.bossBlock.SetActive(false);
        balls = GameObject.FindGameObjectsWithTag("Ball");
        foreach (GameObject b in balls)
        {
            b.SetActive(false);
        }
        paddle.SetActive(false);
    }

    private void Update()
    {
        InitialBallLauncher();
    }
}
