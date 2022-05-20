using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndividualBlockScript : MonoBehaviour
{
    //how many ball collisions it will take a particular block to be destroyed
    public int blockHealth = 1;
    private SpriteRenderer sR;
    GameObject crackedBlock;

    private void Start()
    {
        crackedBlock = gameObject.transform.GetChild(0).gameObject;
        sR = gameObject.GetComponent<SpriteRenderer>();
    }
    //boss is inactive when start script runs, so sprite renderer isn't found. will refresh it upon activation.
    public void refreshSR()
    {
        sR = this.gameObject.GetComponent<SpriteRenderer>();
    }

    //turn the sprite renderer for block and cracks off
    public void makeInvisible()
    {
        sR.enabled = false;
        crackedBlock.GetComponent<SpriteRenderer>().enabled = false;
    }
    //set an appropriate color or graphic to match the health of the block
    public void colorCheck()
    {
        sR = gameObject.GetComponent<SpriteRenderer>();
        if (blockHealth == 0)
        {
            crackedBlock.SetActive(true);
        }
        if (blockHealth == 1)
        {
            sR.color = Color.white;
        }
        if (blockHealth == 2)
        {
            sR.color = Color.gray;
        }
        if (blockHealth == 3)
        {
            sR.color = Color.blue;
        }
        if (blockHealth > 3)
        {
            sR.color = Color.red;
        }
    }
}
