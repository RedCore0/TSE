using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;
    public Transform startPoint; // The start point enemies spawn at.
    public Transform[] path; // The array of points that enemies follow.
    private int playerCurrency; // The player's current currency, how much they can spend.
    
    public int startingCurrency = 100; // How much currency the player should start with. 100 by default.
    public int structureHealth; // The health of the objective the player is defending.
    
    public int GetCurrency() // Gets the player's currency.
    {
        return playerCurrency;
    }

    public void AddCurrency(int amount) // Adds to the player's currency.
    {
        playerCurrency += amount;
    }

    public bool SpendCurrency(int amount) // Spends from the player's currency.
    {
        if (amount <= playerCurrency)
        {
            playerCurrency -= amount;
            return true;
        }

        else
        {
            return false;
        }
    }

    public void DamageStructure(int incomingDamage) // Damages the structure the player is defending.
    {
        structureHealth -= incomingDamage;
        Debug.Log("Structure remaining health: " + structureHealth); 
        // Eventually the UI should show how much health the structure has left.

        if (structureHealth <= 0)
        {
            // Game over would happen here.
            Time.timeScale = 0; // For now, we just pause the game.
        }
    }

    public void Awake()
    {
        main = this;
    }

    void Start()
    {
        playerCurrency = startingCurrency; // Set player currency to the starting value.
        path = GameObject.Find("Path").GetComponentsInChildren<Transform>(); // Get the path points from the path object.
    }
}