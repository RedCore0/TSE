using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    [Header("Attributes")]
    [SerializeField] private int hitPoints = 2; // Stores a default value for hp of enemy
    [SerializeField] private int currencyWorth  = 20; // Stores a default value for the currency to be earned upon this enemy being destroyed

    private bool isDestroyed = false; // Resolves issue where two (or more) bullets could collide at once and both would cause the onEnemyDestroy to be called
    public void TakeDamage(int damage) // Controls the enemy taking damage on hits
    {
        hitPoints -= damage; // Lowers the hp value of the enemy
        if (hitPoints <= 0 && !isDestroyed) // Checks if the enemy should be dead
        {
            EnemySpawner.onEnemyDestroy.Invoke(); // Tell the EnemySpawner that the enemy has died
            LevelManager.main.IncreaseCurrency(currencyWorth);
            isDestroyed = true;
            Destroy(gameObject); // Destroy the gameObject for the enemy
        }
    }



}
