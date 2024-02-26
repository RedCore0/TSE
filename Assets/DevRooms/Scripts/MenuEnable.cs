using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuEnable : MonoBehaviour
{
    public GameObject Menu;
    public GameObject CurrentMenu;

    public void enableMenu()
    {
        Menu.SetActive(true);
        CurrentMenu.SetActive(false);
    }
}
