using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public GameObject[] enemyPrefabs;  // Stores the enemy types which can be spawned.
    [SerializeField] public GameObject testEnemyPrefab; // Stores one specific enemy prefab for testing purposes

    [Header("Attributes")]

    [SerializeField] private float enemiesPerSecond; // Controls the rate at which enemies spawn.
    [SerializeField] private float scaleEnemiesPerSecond; // Scalar to control the increase of enemies per second
    [SerializeField] private float timeBetweenWaves; // Controls the time between waves of enemies.

    [SerializeField] private float fireRatePoint; // Arbitrary value to determine if player is using high fire rate towers
    [SerializeField] private float aerialCapabilityPoint; // Arbitrary value to determine if player is using arial capable towers

    [SerializeField] private float baseEnemyCurrency; // The enemy starting currency
    [SerializeField] private float scaleEnemyCurrency; // Scaling value for enemy currency 
    [SerializeField] private float enemyCurrency; // The enemy's current curreny


    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();

    [SerializeField] List<int> currentWave = new List<int>(); // Stores the prefabs of the enemies that will be spawned this wave

    [SerializeField] private int waveNumber = 1; // The current wave - initialised as 1. Eventually starting wave may be something we change.
    private float timeSinceLastSpawn; // Checked against timeBetweenWaves to ensure enemies are spawning at the correct speed.
    private int enemiesAlive; // How many enemies are currently still alive.

    private bool isSpawning = false; // Determines if the enemies should currently be spawning.

    List<int> enemyCosts = new List<int>(); // Stores the cost of each enemy (I could not get the enemy cost to be accessible from baseEnemy / the prefabs so this is my fix)

    public bool enemyTesting = false; // Changes spawning method for testing.
    
    public void Start()
    {
        enemyCurrency = baseEnemyCurrency; // Sets the enemy currency to the starting value

        enemyCosts.Add(120); // Adds the cost of the enemies (again this is not ideal)
        enemyCosts.Add(20);
        enemyCosts.Add(7);
        enemyCosts.Add(180);
        enemyCosts.Add(30);
        enemyCosts.Add(5);
        enemyCosts.Add(80);
        
        StartCoroutine(StartWave()); // Starts the first wave.
    }

    private void Awake()
    {
        onEnemyDestroy.AddListener(EnemyDestroyed);
    }

    public void Update()
    {
        if (!isSpawning) // If enemies should not be spawning,
        {
            return; // run no further code.
        }

        timeSinceLastSpawn += Time.deltaTime; // Increments the timeSinceLastSpawn variable by deltaTime to act as a clock.

        
        // The method below is only here for me to test new enemies, it will be either deleted or obsolete in the final product.
        // It will overwrite the normal spawning procedure to instead spawn one single enemy at a time and respawn it when it dies.
        // Element 0 in the array will be the enemy that is spawned.if (enemyTesting)
        if (enemyTesting)
        {
            if (enemiesAlive < 1)
            {
                SpawnEnemyDev(); // spawn an enemy.
                enemiesAlive++; // Adds 1 to the enemies alive as one has just spawned.
            }

            return;
        }
        
        if (timeSinceLastSpawn >= (1f / enemiesPerSecond) && currentWave.Count > 0) // Checks to see if another enemy should spawn.
        {
            SpawnNextEnemy(); // Spawns the enemy.
            timeSinceLastSpawn = 0f; // Resets the time since a spawn event occured to 0.
        }

        if (enemiesAlive == 0 && currentWave.Count == 0) // Once there are no enemies left alive, or to spawn,
        {
            EndWave(); // the wave is over and can be ended.
        }
    }

    public IEnumerator StartWave() // Starts a wave.
    {
        yield return new WaitForSeconds(timeBetweenWaves); // Wait for the delay between waves.

        GenerateWave();
        isSpawning = true; // Ensures the spawning code will run.
        
        //enemiesLeftToSpawn = currentWave.Count - 1; // Calculates the wave size for this wave.

    }

    private void EndWave() // Ends the current wave.
    {
        isSpawning = false; // Enemies should no longer be spawning.
        timeSinceLastSpawn = 0f; // Reset last spawn to 0.
        waveNumber++; // Increment the current wave.
        enemyCurrency += (baseEnemyCurrency * scaleEnemyCurrency);
        StartCoroutine(StartWave()); // Start another wave.
    }    
    

    private float calcAverageFirerate(List<float> placedTowersFireRate) // Takes a list of floats and returns the average
    {
        if (placedTowersFireRate.Count == 1)
        {
            return 0;
        }

        float totalFireRate = 0.0f;
        for (int i = 0; i < placedTowersFireRate.Count; i++)
        {
            totalFireRate += placedTowersFireRate[i];
        }
        float averageFireRate = totalFireRate / placedTowersFireRate.Count;
        return averageFireRate;
    }

    private float calcAverageAerialCapability(List<float> placedTowersAerialCapability) // Takes a list of floats and returns the average
    {
        if (placedTowersAerialCapability.Count == 1)
        {
            return 0;
        }

        float totalAerialCapability = 0.0f;
        for (int i = 0; i < placedTowersAerialCapability.Count; i++)
        {
            totalAerialCapability += placedTowersAerialCapability[i];
        }
        float averageAerialCapability = totalAerialCapability / placedTowersAerialCapability.Count;
        return averageAerialCapability;
    }

    private void GenerateWave() // Picking which enemies to send (Always misses the last enemy for some reason <- likely list index error somewhere)
    {
        while (enemyCurrency > 4) // Loop until no more towers can be afforded (cheeapest tower is 5 (again this isn't great))
        {
            if (calcAverageFirerate(LevelManager.main.placedTowersFireRate) > fireRatePoint) // Player is mainly using high fire rate towers
            {// use high hp enemies
                if (calcAverageAerialCapability(LevelManager.main.placedTowersAerialCapability) > aerialCapabilityPoint)
                {// use high hp and don't prioritise aerial enemies
                    if (enemyCurrency > enemyCosts[1])
                    {
                        currentWave.Add(1);
                        enemyCurrency -= enemyCosts[1];
                    }

                    else if (enemyCurrency > enemyCosts[2])
                    {
                        currentWave.Add(2);
                        enemyCurrency -= enemyCosts[2];
                    }

                    else if (enemyCurrency > enemyCosts[0])
                    {
                        currentWave.Add(0);
                        enemyCurrency -= enemyCosts[0];
                    }

                    else if (enemyCurrency > enemyCosts[4])
                    {
                        currentWave.Add(4);
                        enemyCurrency -= enemyCosts[4];
                    }

                    else if (enemyCurrency > enemyCosts[5])
                    {
                        currentWave.Add(5);
                        enemyCurrency -= enemyCosts[5];
                    }

                    else if (enemyCurrency > enemyCosts[6])
                    {
                        currentWave.Add(6);
                        enemyCurrency -= enemyCosts[6];
                    }

                    else if (enemyCurrency > enemyCosts[3])
                    {
                        currentWave.Add(3);
                        enemyCurrency -= enemyCosts[3];
                    }
                }
                else
                {// use high hp and prioritise aerial enemies
                    if (enemyCurrency > enemyCosts[0])
                    {
                        currentWave.Add(0);
                        enemyCurrency -= enemyCosts[0];
                    }

                    else if (enemyCurrency > enemyCosts[4])
                    {
                        currentWave.Add(4);
                        enemyCurrency -= enemyCosts[4];
                    }

                    else if (enemyCurrency > enemyCosts[1])
                    {
                        currentWave.Add(1);
                        enemyCurrency -= enemyCosts[1];
                    }

                    else if (enemyCurrency > enemyCosts[2])
                    {
                        currentWave.Add(2);
                        enemyCurrency -= enemyCosts[2];
                    }

                    else if (enemyCurrency > enemyCosts[3])
                    {
                        currentWave.Add(3);
                        enemyCurrency -= enemyCosts[3];
                    }

                    else if (enemyCurrency > enemyCosts[5])
                    {
                        currentWave.Add(5);
                        enemyCurrency -= enemyCosts[5];
                    }

                    else if (enemyCurrency > enemyCosts[6])
                    {
                        currentWave.Add(6);
                        enemyCurrency -= enemyCosts[6];
                    }
                }
            }
            else // Player is mainly using low fire rate towers
            {// use low hp enemies
                if (calcAverageAerialCapability(LevelManager.main.placedTowersAerialCapability) > aerialCapabilityPoint)
                {// use low hp and don't prioritise aerial enemies
                    if (enemyCurrency > enemyCosts[5])
                    {
                        currentWave.Add(5);
                        enemyCurrency -= enemyCosts[5];
                    }

                    else if (enemyCurrency > enemyCosts[6])
                    {
                        currentWave.Add(6);
                        enemyCurrency -= enemyCosts[6];
                    }

                    else if (enemyCurrency > enemyCosts[3])
                    {
                        currentWave.Add(3);
                        enemyCurrency -= enemyCosts[3];
                    }

                    else if (enemyCurrency > enemyCosts[1])
                    {
                        currentWave.Add(1);
                        enemyCurrency -= enemyCosts[1];
                    }

                    else if (enemyCurrency > enemyCosts[2])
                    {
                        currentWave.Add(2);
                        enemyCurrency -= enemyCosts[2];
                    }

                    else if (enemyCurrency > enemyCosts[4])
                    {
                        currentWave.Add(4);
                        enemyCurrency -= enemyCosts[4] ;
                    }

                    else if (enemyCurrency > enemyCosts[0])
                    {
                        currentWave.Add(0);
                        enemyCurrency -= enemyCosts[0];
                    }
                }
                else
                {// use low hp and prioritise aerial enemies
                    if (enemyCurrency > enemyCosts[3])
                    {
                        currentWave.Add(3);
                        enemyCurrency -= enemyCosts[3];
                    }

                    else if (enemyCurrency > enemyCosts[5])
                    {
                        currentWave.Add(5);
                        enemyCurrency -= enemyCosts[5];
                    }

                    else if (enemyCurrency > enemyCosts[6])
                    {
                        currentWave.Add(6);
                        enemyCurrency -= enemyCosts[6];
                    }

                    else if (enemyCurrency > enemyCosts[0])
                    {
                        currentWave.Add(0);
                        enemyCurrency -= enemyCosts[0];
                    }

                    else if (enemyCurrency > enemyCosts[4])
                    {
                        currentWave.Add(4);
                        enemyCurrency -= enemyCosts[4];
                    }

                    else if (enemyCurrency > enemyCosts[1])
                    {
                        currentWave.Add(1);
                        enemyCurrency -= enemyCosts[1];
                    }

                    else if (enemyCurrency > enemyCosts[2])
                    {
                        currentWave.Add(2);
                        enemyCurrency -= enemyCosts[2];
                    }
                }
            }
        }
    }
    // High Hp:
    // 0 - Large Slime
    // 1 - Corrupted Knight
    // 2 - Witch
    // 4 - Dragon

    //Low Hp:
    // 3 - Poltergeist
    // 5 - Skeleton
    // 6 - Goblin


    private void SpawnNextEnemy()
    {
        if (currentWave.Count > 0) // Check there are still enemies left to spawn
        {
            GameObject prefabToSpawn = enemyPrefabs[currentWave[0]]; // Select the prefab for the next spawning enemy
            Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity); // Spawn the enemy
            currentWave.RemoveAt(0); // Remove the enemy that has just spawned from the wave

            enemiesAlive++; // Adds 1 to the enemies alive as one has just spawned.
        }
    }

    private void EnemyDestroyed() // When an enemy is destroyed,
    {
        enemiesAlive--; // decreases the number of enemies alive.
    }

    private void SpawnRandomEnemy() // Spawns a random enemy.
    {
        int toSpawn = UnityEngine.Random.Range(0, enemyPrefabs.Length); // Randomly selects an enemy to spawn until the ai does this in future/a better temporary method is designed.
        GameObject prefabToSpawn = enemyPrefabs[toSpawn]; // Selects the type of enemy to spawn from the available enemy prefabs.
        Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity); // Spawns the enemy at the level's start point.
    }

    private void SpawnEnemyDev() // Temporary spawn method for testing new creatures.
    {
        GameObject prefabToSpawn = testEnemyPrefab; // Selects the type of enemy to spawn from the available enemy prefabs.
        Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity); // Spawns the enemy at the level's start point.
    }

    public void incrementCount()
    {
        enemiesAlive++;
        Debug.Log("Enemies alive: " + enemiesAlive);
    }

}