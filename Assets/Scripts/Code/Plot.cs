using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;


public class Plot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr; // Stores the sprite renderer for the plot GameObject
    [SerializeField] private Color hoverColor; // Stores the color to change the plots to once it is selected

    private GameObject tower; // Stores the tower within a given plot
    private Color startColor; // Stores the default color of the plot

    

    private void OnMouseEnter() //Once cursor enters the plot change it's color
    {
        if(EventSystem.current.IsPointerOverGameObject()) // Prevents the player "selecting" a tile through the menu ui 
        {
            return;
        }

        sr.color = hoverColor; // Make the plots color the selected plot color
    }

    private void OnMouseExit() // Once cursor leaves the plot reset the color
    {
        sr.color =  startColor; // Make the plots color the default plot color
    }

    private void OnMouseDown()
    {

        if(EventSystem.current.IsPointerOverGameObject()) // Stop the player being able to place a tower through the menu UI
        {
            return;
        }

        if (tower != null) // Check to see if the plot is empty
        {
            return; // If it isn't empty do nothing for now
        }

        // The plot is empty

        Tower towerToBuild = BuildManager.main.GetSelectedTower(); // Select the tower that is going to be built (eventually multiple types)

        if (towerToBuild.cost > LevelManager.main.currency) // Check to see if the player can afford the tower
        {
            Debug.Log("You can not afford this."); // This would likely be some UI event / pop up
            return;
        }

        // The player can afford the tower

        LevelManager.main.SpendCurrency(towerToBuild.cost); // Take the currency for the tower
        tower = Instantiate(towerToBuild.prefab, transform.position, Quaternion.identity); // Build the selected tower
    }

    // Start is called before the first frame update
    void Start()
    {   
        startColor = sr.color; // Ensures the plot starts with the correct color
    }
}
