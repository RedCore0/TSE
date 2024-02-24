using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BaseEnemy : MonoBehaviour
{
    private Rigidbody2D rb; // The enemy's collision hitbox.
    private bool isDestroyed; // Resolves an issue where multiple bullets could collide at once and both would cause the onEnemyDestroy to be called.
    private float distanceTravelled; // Tracks how far the enemy is along the track - allows for 'first'/'last' targetting.
    
    private Transform targetLocation;
    private int pathIndex = 0;
    private bool attacking = false;
    // Enemies follow a sequence of points. The index allows traversal of the array of points.

    // [Header("References")]
    // but we don't need any yet.

    [Header("Stats")]
    [SerializeField] private int enemyHealth; // How much health the enemy has, the damage it can take before it dies.
    [SerializeField] private int enemyDefense; // The enemy's defense - this value is subtracted from all incoming damage.

    [SerializeField] private int enemyDamage; // The enemy's attack - the damage it deals to the 'objective' the player defends.
    [SerializeField] private float attackCooldown; // The delay between each of the enemy's attacks to the objective.
    [SerializeField] private int attackLimit; // The limit to the enemy's attacks - how many times it can attack in its lifespan.

    [SerializeField] private float enemySpeed; // How fast the enemy moves along the track. Makes it harder for towers to hit it.
    [SerializeField] private bool isAerial; // Whether or not the enemy is 'aerial' (flying). Influences which towers can target it.

    [Tooltip("How much it 'costs' for the ai to use")]
    [SerializeField] private int enemyCost; // How much the enemy 'costs' for the AI to spawn it. Stronger enemies have higher values.
    [Tooltip("Currency rewarded to player for defeating it")]
    [SerializeField] private int killReward; // How much currency the player is rewarded for killing the enemy.

    [Space(20)]
    [TextArea]
    public string enemyDescription; // The enemy's description.

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Gets the enemy's attached rigidbody component.
        targetLocation = LevelManager.main.path[pathIndex]; // Set the initial index to 0.
        isDestroyed = false;
        distanceTravelled = 0;
    }

    private void Update()
    {
        if (Vector2.Distance(targetLocation.position, transform.position) <= 0.1f) // Checks to see if the enemy is at the point.
        {
            if (!attacking) { pathIndex++; } // Increments the path index by one.


                if (pathIndex == LevelManager.main.path.Length) // Checks to see if the enemy has reached the end of the path.
            {
                StartCoroutine(attack());   // enemy starts attacking

                if (attackLimit <= 0)
                {
                    StopCoroutine(attack());    // enemy is dead, so can no longer attack 
                    EnemySpawner.onEnemyDestroy.Invoke(); // Tells the EnemySpawner that the enemy has been destroyed.
                    Destroy(gameObject); // Destroys the gameObject for the enemy.
                }
                return;

            }
            else
            {
                if (pathIndex > LevelManager.main.path.Length-1) { pathIndex = LevelManager.main.path.Length - 1; }
                targetLocation = LevelManager.main.path[pathIndex]; // Sets the target location.
                StopCoroutine(attack());    // if structure is destroyed, enemies are no longer at the end and hence stop attacking
            }
        }
    }

    private void FixedUpdate()
    {
        if (!attacking)
        {
            Vector2 direction = (targetLocation.position - transform.position).normalized; // Get the direction of the next target.
            rb.velocity = direction * enemySpeed; // Moves towards the target.
            distanceTravelled += enemySpeed; // Adds to the enemy's distance travelled.
        }
    }

    public bool IsAerial() // Returns whether the enemy is aerial or not.
    {
        return isAerial;
    }

    public float GetDistance() // Returns the enemy's distance travelled.
    {
        return distanceTravelled;
    }

    public int GetHealth() // Returns the enemy's current health.
    {
        return enemyHealth;
    }

    public int GetDefense() // Returns the enemy's defense.
    {
        return enemyDefense;
    }

    public float GetSpeed() // Returns the enemy's speed.
    {
        return enemySpeed;
    }

    public void TakeDamage(int incomingDamage) // Controls the enemy taking damage on hits.
    {
        incomingDamage -= enemyDefense; // Reduce incoming damage by the enemy's defense.
        
        if (incomingDamage <= 0) // If damage has become zero or less,
        {
            incomingDamage = 1; // it is set to a minimum of one damage.
        }

        enemyHealth -= incomingDamage; // Lowers the enemy's health accordingly.

        if (enemyHealth <= 0 && !isDestroyed) // Checks if the enemy should be dead.
        {
            EnemySpawner.onEnemyDestroy.Invoke(); // Tells the EnemySpawner that the enemy has been destroyed.
            LevelManager.main.IncreaseCurrency(killReward); // Increases the player's currency by the enemy's kill reward.
            isDestroyed = true; // Sets isDestroyedd to true to resolve a certain conflict.
            Destroy(gameObject); // Destroys the gameObject for the enemy.
        }
    }

    public IEnumerator attack()
    {
        //play animation
        Debug.Log("Attacking");
        attacking = true;
        LevelManager.main.BroadcastMessage(string.Format("structureDamage({0})", enemyDamage));     // structure takes damage
        attackLimit--;  // reduces remaining attacks
        yield return new WaitForSeconds(attackCooldown);    // waits for cooldown to finish
    }
}