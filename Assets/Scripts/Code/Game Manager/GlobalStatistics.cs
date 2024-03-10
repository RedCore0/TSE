using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalStatistics : MonoBehaviour
{

    [Header("Statistics")]
    [SerializeField] public int playerDamageInflicted; // Global total for how much damage has been dealt by the player
    [SerializeField] public int playerCurrencySpent; // Global total for how much currency has been spent by the player
    [SerializeField] public int lastPlayerCurrencySpent; // Global total for how much currency was spent by the player last round (same counting as above just reset at the start/end of each round)

    [SerializeField] public int enemyDamageInflicted; // Global total for how much damage has been dealt by the enemy
    [SerializeField] public int lastEnemyDamageInflicted; // Global total for how much damage was dealt by the enemy last round (same counting as above just reset at the star/end of each round)



    [Header("Booleans")] // A list of booleans to asses what type of defence the player has (default to false)
    [SerializeField] public bool hasArialDefence = false; // Does the player have something that can target arial units
    [SerializeField] public bool hasGroundDefence = false; // Does the player have something that can target ground units
    [SerializeField] public bool hasOnTrackObstacle = false; // Does the player have anything on the track
    [SerializeField] public bool hasMeleeDefence = false; // Does the player have something that is melee
    [SerializeField] public bool hasProjectileDefence = false; // Does the player have something that is projectile
    [SerializeField] public bool hasHitscanDefence =  false; // Does the player have something that is hitscan
    // These will need to be updated any time a tower is placed / destroyed / upgraded

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
