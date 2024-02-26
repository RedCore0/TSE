using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BaseEnemy : MonoBehaviour
{
    private Rigidbody2D rb; // The enemy's collision hitbox.
    private bool isDestroyed; // Resolves an issue where multiple bullets could collide at once and both would cause the onEnemyDestroy to be called.
    private bool isAttacking; // Whether or not the enemy has started attacking the objective.
    private float distanceTravelled; // Tracks how far the enemy is along the track - allows for 'first'/'last' targetting.
    private float nextAttack; // The next time the enemy can attack.
    
    private Transform targetLocation;
    private int pathIndex = 0;
    // Enemies follow a sequence of points. The index allows traversal of the array of points.

    // [Header("References")]
    // but we don't need any yet.

    [Header("Stats")]
    [SerializeField] private int enemyHealth; // How much health the enemy has, the damage it can take before it dies.
    [SerializeField] private int enemyDefense; // The enemy's defense - this value is subtracted from all incoming damage.

    [SerializeField] private int attackDamage; // The enemy's attack - the damage it deals to the 'objective' the player defends.
    [SerializeField] private float attackCooldown; // The delay between each of the enemy's attacks to the objective.
    [SerializeField] private int attackLimit; // The limit to the enemy's attacks - how many times it can attack in its lifespan.

    [SerializeField] private float enemySpeed; // How fast the enemy moves along the track. Makes it harder for towers to hit it.
    [SerializeField] private bool isAerial; // Whether or not the enemy is 'aerial' (flying). Influences which towers can target it.

    [SerializeField] private int enemyCost; // How much the enemy 'costs' for the AI to spawn it. Stronger enemies have higher values.
    [SerializeField] private int killReward; // How much currency the player is rewarded for killing the enemy.

    [Header("Info")]
    public string enemyName; // The enemy's name.
    [Space(20)]
    [TextArea]
    public string enemyDescription; // The enemy's description.

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Gets the enemy's attached rigidbody component.
        targetLocation = LevelManager.main.path[pathIndex]; // Set the initial index to 0.
        isDestroyed = false; // Initialises isDestroyed to false.
        isAttacking = false; // Initialises isAttacking to false.
        distanceTravelled = 0; // Sets initial distance travelled to 0 .
        nextAttack = Time.time + attackCooldown; // Sets next attack time. (the current time + the attack cooldown)
    }

    private void Update()
    {
        if (isAttacking) // If the enemy is attacking,
        {
            if (Time.time >= nextAttack) // and the next attack is due,
            {
                DoAttack(); // do an attack,
                nextAttack = Time.time + attackCooldown; // and reset the cooldown.
            }
            return; // Return as we are no longer moving.
        }

        if (Vector2.Distance(targetLocation.position, transform.position) <= 0.1f) // Checks to see if the enemy is at the point.
        {
            pathIndex++; // Increments the path index by one.

            if (pathIndex == LevelManager.main.path.Length) // Checks to see if the enemy has reached the end of the path.
            {
                rb.velocity = Vector2.zero; // If it has then stop moving,
                isAttacking = true; // and start attacking...
            }

            else // If it is not at the end of the path,
            {
                targetLocation = LevelManager.main.path[pathIndex]; // set the next target location.    
            }
        }
    }

    private void FixedUpdate()
    {
        if (!isAttacking) // If the enemy is not attacking already,
        {
            Vector2 direction = (targetLocation.position - transform.position).normalized; // get the direction of the next target,
            rb.velocity = direction * enemySpeed; // move towards the target,
            distanceTravelled += enemySpeed; // add to the enemy's distance travelled.
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
            LevelManager.main.AddCurrency(killReward); // Increases the player's currency by the enemy's kill reward.
            DestroyEnemy(); // Destroys the enemy.
        }
    }

    private void DestroyEnemy() // To be called when the enemy is to be destroyed.
    {
        EnemySpawner.onEnemyDestroy.Invoke(); // Tells the EnemySpawner that the enemy has been destroyed.
        isDestroyed = true; // Sets isDestroyed to true to resolve a certain conflict.
        Destroy(gameObject); // Destroys the gameObject for the enemy.
    }

    private void DoAttack() // Have the enemy attack the objective.
    {
        if (attackLimit > 0) // If the enemy has attacks remaining,
        {
            attackLimit -= 1; // decrease them by one,
            LevelManager.main.DamageStructure(attackDamage); // and damage the structure.
        }
        else // Otherwise, it has exhausted its attacks,
        {
            DestroyEnemy(); // so destroy the enemy.
        }
    }
}