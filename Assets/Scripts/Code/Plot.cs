using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;


public class Plot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr; // Stores the sprite renderer for the plot game object.
    [SerializeField] private Color hoverColor; // Stores the color to change the plot to once it is selected.

    private GameObject tower; // Stores the tower placed on this plot.
    private Color startColor; // Stores the default color of the plot.

    private void OnMouseEnter() // Once cursor enters the plot change its color.
    {
        if(EventSystem.current.IsPointerOverGameObject()) // Prevents the player 'selecting' a tile through the in-game UI.
        {
            return;
        }

        sr.color = hoverColor; // Change the plot's color to the hover color.
    }

    private void OnMouseExit() // Once cursor leaves the plot reset the color.
    {
        sr.color =  startColor; // Change the plot's color to the default plot color.
    }

    private void OnMouseDown()
    {

        if(EventSystem.current.IsPointerOverGameObject()) // Stops the player being able to place a tower through the in-game UI.
        {
            return;
        }

        if (tower != null) // Checks to see if the plot is empty.
        {
            return; // If it isn't empty do nothing for now.
            // In future, this is where the player will be able to access upgrades and selling the tower.
        }

        // The plot is empty...

        BaseTower towerToBuild = BuildManager.main.GetSelectedTower(); // Gets the currently selected tower to build.

        if (towerToBuild.towerCost > LevelManager.main.GetCurrency()) // Checks to see if the player can afford the tower.
        {
            Debug.Log("You can not afford this.");
            // Eventually to be replaced with some kind of UI pop-up.
            return; // They can't, do nothing for now.
        }

        // The player can afford the tower...

        LevelManager.main.SpendCurrency(towerToBuild.towerCost); // Spend the currency for the tower.
        tower = Instantiate(towerToBuild.gameObject, transform.position, Quaternion.identity); // Build the selected tower.
    }

    void Start()
    {   
        startColor = sr.color; // Ensures the plot starts with the correct color.
    }
}