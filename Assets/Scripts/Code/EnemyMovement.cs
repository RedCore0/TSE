using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb; // Collision / hitbox

    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 2f; // Speed of enemy

    private Transform target; // Where the enemy is going
    private int pathIndex = 0;
    // Enemy follows a sequence of points the index allows traversal of the array of points

//Start is called before the first frame update
    private void Start()
    {
        target = LevelManager.main.path[pathIndex]; // Set the inital index to 0
    }

//Is executed every frame
    private void Update()
    {
        if (Vector2.Distance(target.position, transform.position) <= 0.1f) // Checks to see if the enemy is at the point
        {
            pathIndex++; // Increment pathIndex

            if (pathIndex == LevelManager.main.path.Length) // Checks to see if the enemy has reached the end of the path
            {
                EnemySpawner.onEnemyDestroy.Invoke(); // Lets EnemySpawner know that the enemy is being destroyed
                Destroy(gameObject); // Destroys the enemy
                return;
            }
            else 
            {
                target = LevelManager.main.path[pathIndex]; // Set target location
            }
        }
    }

//Is executed based on the Fixed Timestep (by default 50 times per second)
    private void FixedUpdate()
    {
        Vector2 direction = (target.position - transform.position).normalized; // Get the direction of the next target
        rb.velocity = direction * moveSpeed; // Move towards the target
    }
}
