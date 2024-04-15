using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public GameObject[] enemyPrefabs;  // Stores the enemy types which can be spawned.

    [Header("Attributes")]
    [SerializeField] private int baseEnemies; // Controls the amount of enemies in a given wave.
    [SerializeField] private float enemiesPerSecond; // Controls the rate at which enemies spawn.
    [SerializeField] private float timeBetweenWaves; // Controls the time between waves of enemies.
    [SerializeField] private float difficultyScalingFactor; // Controls the scaling of enemies between waves.
    [SerializeField] public GameObject[] enemiesWave; // The enemies that are going to be spawned this wave 

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();

    private int currentWave = 1; // The current wave - initialised as 1. Eventually starting wave may be something we change.
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
            ; // Spawns the enemy.
            enemiesLeftToSpawn--; // Subtracts 1 from the enemies left in the wave.
            enemiesAlive++; // Adds 1 to the enemies alive as one has just spawned.
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
        enemiesLeftToSpawn = EnemiesPerWave(); // Calculates the wave size for this wave.
    }

    private void SpawnNextWaveEnemy(GameObject enemies[])
    {
        Instantiate(enemies[0], LevelManager.main.startPoint.position, Quaternion.identity);
    }

    private void SpawnEnemy() // Spawns a random enemy.
    {
        int toSpawn = UnityEngine.Random.Range(0, enemyPrefabs.Length -1); // Randomly selects an enemy to spawn until the ai does this in future/a better temporary method is designed.
        GameObject prefabToSpawn = enemyPrefabs[toSpawn]; // Selects the type of enemy to spawn from the available enemy prefabs.
        Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity); // Spawns the enemy at the level's start point.
    }
    
    private void SpawnEnemyDev() // Temporary spawn method for testing new creatures.
    {
        GameObject prefabToSpawn = enemyPrefabs[0]; // Selects the type of enemy to spawn from the available enemy prefabs.
        Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity); // Spawns the enemy at the level's start point.
    }

    private void GetCheapestEnemies()
    {
        GameObject cheapest = enemyPrefabs[0];
        for (int i = 1 ; i < enemyPrefabs.Length -1 ; i++)
        {
            if cheapest.enemyCost > enemyPrefabs[i].enemyCost && 
            {
                cheapest = enemyPrefabs[i];
            }
        }
    }

    private void SpawnDearestEnemies()
    {

    }

    private void EnemyDestroyed() // When an enemy is destroyed,
    {
        enemiesAlive--; // decreases the number of enemies alive.
    }

    private int EnemiesPerWave() // Calculates the enemies to be spawned per wave.
    {
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScalingFactor));
        // This could eventually be repurposed to calculate the number of credits the AI gets per wave.
    }

    private void EndWave() // Ends the current wave.
    {
        isSpawning = false; // Enemies should no longer be spawning.
        timeSinceLastSpawn = 0f; // Reset last spawn to 0.
        currentWave++; // Increment t he current wave.
        StartCoroutine(StartWave()); // Start another wave.
    }
}