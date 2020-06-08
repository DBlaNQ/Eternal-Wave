using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    [SerializeField] PlayerController playerController = null;
    [SerializeField] GameObject pauseMenu = null;
    [SerializeField] GameObject optionsMenu = null;

    //Resume
    public void Resume()
    {
        playerController.paused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    //Options
    public void Options()
    {
        optionsMenu.SetActive(true);
        pauseMenu.SetActive(false);
    }

    //Back
    public void Back()
    {
        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    //Main Menu
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    //Play
    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    //Close Game
    public void ExitGame()
    {
        Application.Quit();
    }
}
