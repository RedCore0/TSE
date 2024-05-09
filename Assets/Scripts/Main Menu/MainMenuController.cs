using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject controlsPanel;
    public GameObject volumePanel;
    public GameObject creditsPanel;
    public GameObject difficultyPanel;
    public GameObject mapPanel;
    public Slider musicSlider;
    public Slider sfxSlider;

    AudioSource[] audio1;


    public void Start()
    {
        optionsMenu.SetActive(false);
        controlsPanel.SetActive(false);
        volumePanel.SetActive(false);
        creditsPanel.SetActive(false);
        difficultyPanel.SetActive(false);
        mapPanel.SetActive(false);
        audio1 = GetComponents<AudioSource>();
        musicSlider.value = 100f;
        sfxSlider.value = 100f;

    }
    public void SelectDifficulty()
    {
        audio1[0].Play();
        mainMenu.SetActive(false);
        difficultyPanel.SetActive(true);
    }
    public void SelectDifficultyBack()
    {

        audio1[0].Play();
        mainMenu.SetActive(true);
       difficultyPanel.SetActive(false);
    }
    public void MapsBack() 
    {
        audio1[0].Play();
        difficultyPanel.SetActive(true);
        mapPanel.SetActive(false);
    }
    public void Options()
    {
        audio1[0].Play();
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);

    }


    public void OptionsBack()
    {

        audio1[0].Play();
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void ExitGame()
    {
        audio1[0].Play();
        Application.Quit();
    }

    public void Easy()
    {
        audio1[0].Play();
        Globals.difficulty = 1;
        difficultyPanel.SetActive(false);
        mapPanel.SetActive(true);
        
    }
    public void Medium()
    {
        audio1[0].Play();
        Globals.difficulty = 2;
        difficultyPanel.SetActive(false);
        mapPanel.SetActive(true);

    }
    public void Hard()
    {
        audio1[0].Play();
        Globals.difficulty = 3;
        difficultyPanel.SetActive(false);
        mapPanel.SetActive(true);

    }
    public void ApeGarden()
    {
        audio1[0].Play();
        Globals.curMap = Globals.Maps.ApeGarden;
        SceneManager.LoadSceneAsync(sceneName: "Ape Garden");
    }
    public void AbyssalCarverns()
    {
        audio1[0].Play();
        Globals.curMap = Globals.Maps.AbyssalCaverns;
        SceneManager.LoadSceneAsync(sceneName: "Abyssal Caverns");//Replace Ape Garden with different map name once maps are created
    }
    public void VolcanicHillside()
    {
        audio1[0].Play();
        Globals.curMap = Globals.Maps.VolcanicHillside;
        SceneManager.LoadSceneAsync(sceneName: "Volcanic Hillside");//Replace Ape Garden with different map name once maps are created
    }
    public void Volume()
    {
        audio1[0].Play();
        optionsMenu.SetActive(false);
        volumePanel.SetActive(true);
    }
    public void Controls()
    {
        audio1[0].Play();
        optionsMenu.SetActive(false);
        controlsPanel.SetActive(true);
    }
    public void VolumeBack()
    {
        audio1[0].Play();
        volumePanel.SetActive(false);
        optionsMenu.SetActive(true);

    }
    public void ControlsBack()
    {
        audio1[0].Play();
        controlsPanel.SetActive(false);
        optionsMenu.SetActive(true);

    }
    public void Credits()
    {
        audio1[0].Play();
        optionsMenu.SetActive(false);
        creditsPanel.SetActive(true);
    }
    public void CreditsBack()
    {
        audio1[0].Play();
        creditsPanel.SetActive(false);
        optionsMenu.SetActive(true);

    }
    public void MusicControl()
    {
        audio1[1].volume = musicSlider.value;
        Globals.musicVol = musicSlider.value;
    }
    public void SFXControl()
    {
        audio1[0].volume = sfxSlider.value;
        Globals.buttonVol = sfxSlider.value;
    }
}
