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
   // public GameObject usernamePanel;
    public GameObject controlsPanel;
    public GameObject volumePanel;
    public GameObject creditsPanel;
    public GameObject difficultyPanel;
    public GameObject mapPanel;
    //public TextMeshProUGUI username_text;
    // public Slider slider;

    // AudioSource[] audio1;


    public void Start()
    {
        /*
        if (Globals.can_enter_username == true)
        {
            mainMenu.SetActive(false);
            Globals.can_enter_username = false;
        }
        else
        {
            mainMenu.SetActive(true);
            usernamepanel.SetActive(false);
        }
        */

        optionsMenu.SetActive(false);
        controlsPanel.SetActive(false);
        volumePanel.SetActive(false);
        creditsPanel.SetActive(false);
        difficultyPanel.SetActive(false);
        mapPanel.SetActive(false);
        //audio1 = GetComponents<AudioSource>();
        //slider.value = 10f;

    }
    /*
    public void PlayGame()
    {

        Play_sound();
        SceneManager.LoadSceneAsync(sceneName: "Game Scene 1");
        SceneManager.UnloadSceneAsync(sceneName: "Main Menu");



    }
    */
    public void SelectDifficulty()
    {
        //Play_sound();
        mainMenu.SetActive(false);
        difficultyPanel.SetActive(true);
    }
    public void SelectDifficultyBack()
    {

        //Play_sound();
        mainMenu.SetActive(true);
       difficultyPanel.SetActive(false);
    }
    public void MapsBack() 
    {
        difficultyPanel.SetActive(true);
        mapPanel.SetActive(false);
    }
    public void Options()
    {
        //Play_sound();
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);

    }


    public void OptionsBack()
    {

        //Play_sound();
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void ExitGame()
    {
        //Play_exit_sound();
        Application.Quit();
    }


    /*
    public void SubmitOnClick()
    {
        Play_sound();
        Globals.current_user = username_text.text;
        usernamepanel.SetActive(false);
        mainMenu.SetActive(true);

    }
    */
    public void Easy()
    {
        Globals.difficulty = 1;
        difficultyPanel.SetActive(false);
        mapPanel.SetActive(true);
        
    }
    public void Medium()
    {
        Globals.difficulty = 2;
        difficultyPanel.SetActive(false);
        mapPanel.SetActive(true);

    }
    public void Hard()
    {
        Globals.difficulty = 3;
        difficultyPanel.SetActive(false);
        mapPanel.SetActive(true);

    }
    public void ApeGarden()
    {
        SceneManager.LoadSceneAsync(sceneName: "Ape Garden");
        SceneManager.UnloadSceneAsync(sceneName: "Main Menu");
    }
    public void Map2()
    {
        SceneManager.LoadSceneAsync(sceneName: "Ape Garden");//Replace Ape Garden with different map name once maps are created
        SceneManager.UnloadSceneAsync(sceneName: "Main Menu");
    }
    public void Map3()
    {
        SceneManager.LoadSceneAsync(sceneName: "Ape Garden");//Replace Ape Garden with different map name once maps are created
        SceneManager.UnloadSceneAsync(sceneName: "Main Menu");
    }
    public void Volume()
    {
        //Play_sound();
        optionsMenu.SetActive(false);
        volumePanel.SetActive(true);
    }
    public void Controls()
    {
        //Play_sound();
        optionsMenu.SetActive(false);
        controlsPanel.SetActive(true);
    }
    public void VolumeBack()
    {
        //Play_sound();
        volumePanel.SetActive(false);
        optionsMenu.SetActive(true);

    }
    public void ControlsBack()
    {
        //Play_sound();
        controlsPanel.SetActive(false);
        optionsMenu.SetActive(true);

    }
    public void Credits()
    {
        //Play_sound();
        optionsMenu.SetActive(false);
        creditsPanel.SetActive(true);
    }
    public void CreditsBack()
    {
        //Play_sound();
        creditsPanel.SetActive(false);
        optionsMenu.SetActive(true);

    }
    /*
    public void VolumeControl()
    {
        Play_sound();
        audio1[1].volume = slider.value;
    }
    
    void Play_sound()
    {
        if (audio1[0].isPlaying)
        {
            audio1[0].Stop();
        }

        audio1[0].Play();
    }
    void Play_exit_sound()
    {
        if (audio1[2].isPlaying)
        {
            audio1[2].Stop();
        }

        audio1[2].Play();
    }
    */
}
