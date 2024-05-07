using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject shop;
    public KeyCode pauseKey;
    public GameObject pauseMenu;
    public GameObject pauseButton;
    public GameObject controlsPanel;
    public GameObject volumePanel;
    public GameObject creditsPanel;
    bool paused = false;
    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        controlsPanel.SetActive(false);
        volumePanel.SetActive(false);
        creditsPanel.SetActive(false);

    }
    public void Update()
    {
        if (Input.GetKey(pauseKey) && !paused)
        {
            Pause();
        }
        else if (Input.GetKey(pauseKey) && paused)
        {
            Resume();
        }
    }
    public void Pause()
    {
        shop.SetActive(false);
        paused = true;
        pauseButton.SetActive(false);
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
    }
    public void Resume()
    {
        shop.SetActive(true);
        paused = false;
        pauseButton.SetActive(true);
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }
    public void Restart()
    {

        //Play_sound();
        shop.SetActive(true);
        pauseButton.SetActive(true);
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;

        if(Globals.curMap==Globals.Maps.ApeGarden)
        {
            SceneManager.LoadSceneAsync(sceneName: "Ape Garden");
        }
        else if (Globals.curMap == Globals.Maps.MapTwo)
        {
            SceneManager.LoadSceneAsync(sceneName: "Ape Garden");
        }
        else if (Globals.curMap == Globals.Maps.MapOne)
        {
            SceneManager.LoadSceneAsync(sceneName: "Ape Garden");
        }

        //SceneManager.UnloadSceneAsync(sceneName: "Main Menu");
    }
    public void MainMenu()
    {
        shop.SetActive(true);
        pauseButton.SetActive(true);
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        //SceneManager.UnloadSceneAsync(sceneName: "Ape Garden");
        SceneManager.LoadSceneAsync(sceneName: "Main Menu");
    }
    public void Volume()
    {
        //Play_sound();
        pauseMenu.SetActive(false);
        volumePanel.SetActive(true);
    }
    public void Controls()
    {
        //Play_sound();
        pauseMenu.SetActive(false);
        controlsPanel.SetActive(true);
    }
    public void VolumeBack()
    {
        //Play_sound();
        volumePanel.SetActive(false);
        pauseMenu.SetActive(true);

    }
    public void ControlsBack()
    {
        //Play_sound();
        controlsPanel.SetActive(false);
        pauseMenu.SetActive(true);

    }
    public void Credits()
    {
        //Play_sound();
        pauseMenu.SetActive(false);
        creditsPanel.SetActive(true);
    }
    public void CreditsBack()
    {
        //Play_sound();
        creditsPanel.SetActive(false);
        pauseMenu.SetActive(true);

    }
}
