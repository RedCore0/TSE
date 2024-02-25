using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Menu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI currencyUI;
    [SerializeField] Animator anim;

    private bool isMenuOpen = true;

    public void ToggleMenu() // Setting the menu to either be open or closed
    {
        isMenuOpen = !isMenuOpen; 
        anim.SetBool("MenuOpen", isMenuOpen); // Changing the boolean value in the animator
    }

    private void OnGUI ()
    {
        currencyUI.text = LevelManager.main.currency.ToString();
    }

    public void setSelected()
    {
        
    }
}
