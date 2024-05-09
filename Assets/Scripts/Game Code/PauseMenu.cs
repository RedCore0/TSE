using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject shop;
    public GameObject deadPanel;
    public KeyCode pauseKey;
    public GameObject pauseMenu;
    public GameObject pauseButton;
    public GameObject controlsPanel;
    public GameObject volumePanel;
    public GameObject creditsPanel;
    bool paused = false;
    public Slider towerSlider;
    public Slider enemySlider;
    public Slider menuSlider;
    public Slider musicSlider;
    public Slider playerHealth;
    AudioSource[] audio1;
    IEnumerator WaitForDead()
    {
        while (Globals.playerHealth>0)
        {
            yield return new WaitForSeconds(1);
        }
        deadPanel.SetActive(true);
        yield return new WaitForSeconds(5);
        MainMenu();
    }
    // Start is called before the first frame update
    void Start()
    {
        deadPanel.SetActive(false);
        pauseMenu.SetActive(false);
        controlsPanel.SetActive(false);
        volumePanel.SetActive(false);
        creditsPanel.SetActive(false);
        audio1 = GetComponents<AudioSource>();
        audio1[0].volume = Globals.buttonVol;
        audio1[1].volume = Globals.musicVol;
        towerSlider.value = 100f;
        enemySlider.value = 100f;
        menuSlider.value = 100f;
        musicSlider.value = 100f;
        playerHealth.value = Globals.playerHealth; 
        StartCoroutine(WaitForDead());
    }
    public void Update()
    {
        playerHealth.value = Globals.playerHealth;
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
        audio1[0].Play();
        shop.SetActive(false);
        paused = true;
        pauseButton.SetActive(false);
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
    }
    public void Resume()
    {
        audio1[0].Play();
        shop.SetActive(true);
        paused = false;
        pauseButton.SetActive(true);
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }
    public void Restart()
    {
        Globals.playerHealth = 100;
        audio1[0].Play();
        shop.SetActive(true);
        pauseButton.SetActive(true);
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;

        if(Globals.curMap==Globals.Maps.ApeGarden)
        {
            SceneManager.LoadSceneAsync(sceneName: "Ape Garden");
        }
        else if (Globals.curMap == Globals.Maps.AbyssalCaverns)
        {
            SceneManager.LoadSceneAsync(sceneName: "Abyssal Caverns");
        }
        else if (Globals.curMap == Globals.Maps.VolcanicHillside)
        {
            SceneManager.LoadSceneAsync(sceneName: "Volcanic Hillside");
        }

    }
    public void MainMenu()
    {
        Globals.playerHealth = 100;
        audio1[0].Play();
        shop.SetActive(true);
        pauseButton.SetActive(true);
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(sceneName: "Main Menu");
    }
    public void Volume()
    {
        audio1[0].Play();
        pauseMenu.SetActive(false);
        volumePanel.SetActive(true);
    }
    public void Controls()
    {
        audio1[0].Play();
        pauseMenu.SetActive(false);
        controlsPanel.SetActive(true);
    }
    public void VolumeBack()
    {
        audio1[0].Play();
        volumePanel.SetActive(false);
        pauseMenu.SetActive(true);

    }
    public void ControlsBack()
    {
        audio1[0].Play();
        controlsPanel.SetActive(false);
        pauseMenu.SetActive(true);

    }
    public void Credits()
    {
        audio1[0].Play();
        pauseMenu.SetActive(false);
        creditsPanel.SetActive(true);
    }
    public void CreditsBack()
    {
        audio1[0].Play();
        creditsPanel.SetActive(false);
        pauseMenu.SetActive(true);

    }
    public void MusicControl()
    {
        audio1[0].Play();
        audio1[1].volume = musicSlider.value;
        Globals.musicVol = musicSlider.value;
    }
    public void EnemyVolumeControl()
    {
        audio1[0].Play();
        Globals.enemyVol = enemySlider.value;
    }
    public void TowerVolumeControl()
    {
        audio1[0].Play();
        Globals.towerVol = towerSlider.value;
    }
    public void ButtonVolumeControl()
    {
        audio1[0].Play();
        audio1[0].volume = menuSlider.value;
        Globals.buttonVol = menuSlider.value;
    }
}
