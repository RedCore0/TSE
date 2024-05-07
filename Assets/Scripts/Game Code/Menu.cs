using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class Menu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI currencyUI;
    [SerializeField] TextMeshProUGUI buttonUI;
    [SerializeField] Animator anim;

    private bool shopIsOpen = true;

    public void ToggleMenu() // Setting the menu to either be open or closed.
    {
        shopIsOpen = !shopIsOpen; 
        if(shopIsOpen)
        {
            buttonUI.text = "Close Shop";
        }
        else
        {
            buttonUI.text = "Open Shop";
        }
        anim.SetBool("MenuOpen", shopIsOpen); // Changing the boolean value in the animator.
    }

    private void OnGUI()
    {
        currencyUI.text = LevelManager.main.GetCurrency().ToString();
    }

    public void SetSelected()
    {
        
    }
}