using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;  // Stores the enemy types available to spawn


    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 8; //Controls the ammount of enemies in a given wave
    [SerializeField] private float enemiesPerSecond = 0.5f; //Controls the rate at which enemies spawn
    [SerializeField] private float timeBetweenWaves = 5f; //Controls the time between waves of enemies
    [SerializeField] private float difficultyScalingFactor = 0.75f; //Controls the scaling of enemies between waves

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();

    private int currentWave = 1;
    private float timeSinceLastSpawn; //Checked against timeBetweenWaves to ensure enemies are spawning at correct speed
    private int enemiesAlive; //How many enemies are currently alive
    private int enemiesLeftToSpawn; //How many enemies are left to spawn
    private bool isSpawning  = false; //Determines if the enemies should be spawning



    // Start is called before the first frame update
    public void Start()
    {
        enemiesLeftToSpawn = baseEnemies; //Sets the size of the wave to the default wave size
        StartCoroutine(StartWave()); //Starts the wave
    }

    private void Awake()
    {
        onEnemyDestroy.AddListener(EnemyDestroyed);
    }


    // Update is called once per frame
    public void Update()
    {
        if (!isSpawning)  return; // Control variable means if spawning is disabled then no further code runs

        timeSinceLastSpawn += Time.deltaTime; //Increments the timeSinceLastSpawn variable by deltaTime to act as a clock

        if (timeSinceLastSpawn >= (1f / enemiesPerSecond) && enemiesLeftToSpawn > 0) //Checks to ensure enough time has passed to spawn another enemy and that there are enough enemies left in the wave to be spawned
        {
            SpawnEnemy(); //Spawns the enemy
            enemiesLeftToSpawn--; //Subtracts 1 from the enemies left in the wave
            enemiesAlive++; //Adds 1 to the enemies alive as one has just spawned
            timeSinceLastSpawn= 0f; //Resets the time since a spawn event occured to 0
        }

        if (enemiesAlive == 0 && enemiesLeftToSpawn == 0) //Once there are no more enmies alive or to spawn the wave is over
        {
            EndWave(); //End the wave
        }
    }

    public IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves); //Delay between waves

        isSpawning = true; //Ensures the spwaning code will run
        enemiesLeftToSpawn = EnemiesPerWave(); //Calculates wave size based on wave number
    }

    private void SpawnEnemy()
    {
        Debug.Log("[ + ] Enemy"); //Log message to prove the rest of the code is working
        GameObject prefabToSpawn = enemyPrefabs[0]; //Sets the type of enemy to spawn from the available prefabs
        Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity); //Spawns the enemy at the startPoint (Quaternion.identity i think controls rotation of enemy)
    }

    private void EnemyDestroyed()
    {
        enemiesAlive--; //Decreases enemiesAlive whenever an enemy is destroyed
    }

    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScalingFactor)); //Makes rounds progressively harder as they go on
    }

    private void EndWave()
    {
        isSpawning = false;
        timeSinceLastSpawn = 0f;
        currentWave++;
        StartCoroutine(StartWave()); //Starts the wave
    }
}
