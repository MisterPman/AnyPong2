using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenController : MonoBehaviour
{
    public GameObject titlePanel, startMenuPanel, instructionScreen;
    private void Start()
    {
        //ensure the title screen is showing on load
        if (titlePanel.activeSelf == false)
        {
            titlePanel.SetActive(true);
        }
        if (startMenuPanel.activeSelf == true)
        {
            startMenuPanel.SetActive(false);
        }
        if (instructionScreen.activeSelf == true)
        {
            instructionScreen.SetActive(false);
        }
    }
    public void TitleButton()
    {
        titlePanel.SetActive(false);
        if (startMenuPanel.activeSelf == false)
        {
            startMenuPanel.SetActive(true);
        }

    }

    public void NormalGameStart()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void ChallengeGameStart()
    {
        SceneManager.LoadScene("Challenge Level 1");
    }
    public void ShowInstructions()
    {
        instructionScreen.SetActive(true);
    }
    public void HideInstrictions()
    {
        instructionScreen.SetActive(false);
    }
    public void quitGame()
    {
        Application.Quit();
    }
}
