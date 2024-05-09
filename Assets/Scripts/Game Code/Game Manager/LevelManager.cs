using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;
    public Transform startPoint; // The start point enemies spawn at.
    public Transform[] paths; // The array of points that enemies follow.
    [SerializeField] private int playerCurrency; // The player's current currency, how much they can spend.
    
    public List<float> placedTowersFireRate = new List<float>();
    public List<float> placedTowersAerialCapability = new List<float>();


    public int startingPlayerCurrency = 100; // How much currency the player should start with. 100 by default.
    public int structureHealth; // The health of the objective the player is defending.

    public void AddBuiltTowerFireRate(float fireRate)
    {
        placedTowersFireRate.Add(fireRate);
    }

    public void AddBuiltTowerAerialCapability(int aerialCapability)
    {
        placedTowersAerialCapability.Add(aerialCapability);
    }
    
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
        //structureHealth -= incomingDamage;
        //Debug.Log("Structure remaining health: " + structureHealth); 
        Globals.playerHealth -= incomingDamage;
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
        Globals.playerHealth = structureHealth;
        playerCurrency = startingPlayerCurrency; // Set player currency to the starting value.
        paths = GameObject.FindGameObjectsWithTag("Path").Select(x => x.transform).ToArray(); 
    }
}