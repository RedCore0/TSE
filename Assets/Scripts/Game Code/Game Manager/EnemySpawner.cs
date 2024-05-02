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
    [SerializeField] private float timeBetweenWaves; // Controls the time between waves of enemies.

    //Old
    [SerializeField] private int baseEnemies; // Controls the amount of enemies in a given wave. (Shouldn't be used)
    [SerializeField] private float difficultyScalingFactor; // Controls the scaling of enemies between waves. (Shouldn't be used)

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();

    List<int> currentWave = new List<int>(); // Stores the prefabs of the enemies that will be spawned this wave

    private int waveNumber = 1; // The current wave - initialised as 1. Eventually starting wave may be something we change.
    private float timeSinceLastSpawn; // Checked against timeBetweenWaves to ensure enemies are spawning at the correct speed.
    private int enemiesAlive; // How many enemies are currently still alive.
    private int enemiesLeftToSpawn; // How many enemies are yet to spawn this wave.
    private bool isSpawning = false; // Determines if the enemies should currently be spawning.

    public bool enemyTesting = false; // Changes spawning method for testing.
    
    public void Start()
    {
        enemiesLeftToSpawn = baseEnemies; // Sets the size of the first wave to the default wave size.
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
        
        if (timeSinceLastSpawn >= (1f / enemiesPerSecond) && enemiesLeftToSpawn > 0) // Checks to see if another enemy should spawn.
        {
            SpawnNextEnemy(); // Spawns the enemy.
            timeSinceLastSpawn = 0f; // Resets the time since a spawn event occured to 0.
        }

        if (enemiesAlive == 0 && enemiesLeftToSpawn == 0) // Once there are no enemies left alive, or to spawn,
        {
            EndWave(); // the wave is over and can be ended.
        }
    }

    public IEnumerator StartWave() // Starts a wave.
    {
        yield return new WaitForSeconds(timeBetweenWaves); // Wait for the delay between waves.

        isSpawning = true; // Ensures the spawning code will run.
        GenerateWave();
        enemiesLeftToSpawn = currentWave.Count - 1; // Calculates the wave size for this wave.
    }

    private void EndWave() // Ends the current wave.
    {
        isSpawning = false; // Enemies should no longer be spawning.
        timeSinceLastSpawn = 0f; // Reset last spawn to 0.
        waveNumber++; // Increment the current wave.
        StartCoroutine(StartWave()); // Start another wave.
    }    
    

    private void GenerateWave() // Picking which enemies to send (Always misses the last enemy for some reason <- likely list index error somewhere)
    {
        currentWave.Add(4);
        currentWave.Add(5);
        currentWave.Add(4);
        currentWave.Add(5);
        currentWave.Add(4);
        currentWave.Add(5);
    }


    private void SpawnNextEnemy()
    {
        if (enemiesLeftToSpawn > 0) // Check there are still enemies left to spawn
        {
            GameObject prefabToSpawn = enemyPrefabs[currentWave[0]]; // Select the prefab for the next spawning enemy
            Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity); // Spawn the enemy
            currentWave.RemoveAt(0); // Remove the enemy that has just spawned from the wave

            enemiesLeftToSpawn--; // Subtracts 1 from the enemies left to spawn as one has just spawned
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


//Old Code


    private int EnemiesPerWave() // Calculates the enemies to be spawned per wave. (Shouldn't be needed)
    {
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(waveNumber, difficultyScalingFactor));
        // This could eventually be repurposed to calculate the number of credits the AI gets per wave.
    }
}