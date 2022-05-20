using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathWall : MonoBehaviour
{
    private GameObject uIOverlay;
    private GameMaster gM;

    private void Start()
    {
        uIOverlay = GameObject.FindGameObjectWithTag("GameMaster");
        gM = uIOverlay.GetComponent<GameMaster>();
    }

    //reset any object that touches the death wall and have the player lose a life
    private void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(gM.ObjectReset(collision.gameObject, 1));
        gM.playerDeath(1);
    }
}
