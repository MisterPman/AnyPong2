using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBehavior : MonoBehaviour
{
    public GameMaster gM;
    public GameObject[] blocks, blockCracks;
    public GameObject bossBlock;
    public int bossSpeed;
    void Start()
    {
        gM = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        blocks = GameObject.FindGameObjectsWithTag("Block");
        blockCracks = GameObject.FindGameObjectsWithTag("Block Cracks");
        bossBlock = GameObject.FindGameObjectWithTag("BossBlock");
        //turn off the boss block so it can wait for the end
        foreach (GameObject bC in blockCracks)
        {
            bC.SetActive(false);
        }
        bossBlock.SetActive(false);
    }
    //get the script for the block in question. reduce it's health. change the color. check if it should be destroyed.
    public void BlockCollision(GameObject block)
    {
        IndividualBlockScript blockStats = block.GetComponent<IndividualBlockScript>();
        blockStats.blockHealth -= 1;
        blockStats.colorCheck();
        if (blockStats.blockHealth == 0)
        {
            //check if this is the final block being destroyed. if so, disable all balls.
            if (block == bossBlock)
            {
                gM.balls = GameObject.FindGameObjectsWithTag("Ball");
                foreach (GameObject b in gM.balls)
                {
                    //freeze all motion from balls
                    b.SetActive(false);
                    b.SetActive(true);
                    b.GetComponent<Rigidbody2D>().gravityScale = 0;
                }

            }
            //turn off the collider so the block cannot be interacted with anymore
            block.GetComponent<BoxCollider2D>().enabled = false;
            StartCoroutine(DestroyBlock(block.gameObject, 0.5f));
        }
    }
    //wait, spawn an explosion, turn off the block renderer, wait again, turn off the block. check if boss can be released. check if player has won
    public IEnumerator DestroyBlock(GameObject block, float delayTime)
    {
        //stop the block(in case of bossblock) from spinning and flying around.
        block.SetActive(false);
        block.SetActive(true);
        yield return new WaitForSeconds(delayTime);
        //generate explosion. force the scale to be equal on x and y, as it may have been warped when adjusting for screen size;
        GameObject blockExplosion = Instantiate(gM.blockBurst, block.transform.position, block.transform.rotation, block.transform);
        blockExplosion.transform.localScale = new Vector3(blockExplosion.transform.localScale.y, blockExplosion.transform.localScale.y, 1);
        blockExplosion.SetActive(true);
        //make the block invisible, but leave it active for 2 more seconds so the explosion can run
        block.GetComponent<IndividualBlockScript>().makeInvisible();
        yield return new WaitForSeconds(2);
        //check if this is the final block or not, then if it is the boss block or not
        if (block.CompareTag("Block") == true)
        {
            block.SetActive(false);
            //if all blocks are marked inactive, boss can come out
            bool bossCanBeReleased = true;
            foreach (GameObject blck in blocks)
            {
                if (blck.activeSelf == true)
                {
                    bossCanBeReleased = false;
                }
            }
            if (bossCanBeReleased == true)
            {
                //spawn the boss block with 3 extra health points
                SpawnBossBlock(3);
            }
        }
        else
        {
            block.SetActive(false);
            //if all the blocks have been destroyed, player can win
            bool playerCanWin = true;
            foreach (GameObject blck in blocks)
            {
                if (blck.activeSelf == true)
                {
                    playerCanWin = false;
                }
            }
            if (bossBlock.activeSelf == true)
            {
                playerCanWin = false;
            }
            if (playerCanWin == true)
            {
                gM.PlayerWins();
            }
        }
    }
    //turn on the boss block, give it an appropriate health boost, and set it moving
    public void SpawnBossBlock(int bossBlockHealthBonus)
    {
        bossBlock.SetActive(true);
        Rigidbody2D bossRB = bossBlock.GetComponent<Rigidbody2D>();
        bossBlock.GetComponent<IndividualBlockScript>().refreshSR();
        addHealth(bossBlock, bossBlockHealthBonus);
        bossRB.AddForce(new Vector2(15, 0) * bossSpeed, ForceMode2D.Force);
    }
    //add specified health to a block and change its color
    public void addHealth(GameObject targetBlock, int healthAdd)
    {
        targetBlock.GetComponent<IndividualBlockScript>().blockHealth += healthAdd;
        targetBlock.GetComponent<IndividualBlockScript>().colorCheck();
    }
}