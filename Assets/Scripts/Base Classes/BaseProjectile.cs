using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BaseProjectile : MonoBehaviour
{
    private Rigidbody2D rb; // The projectile's collision hitbox.
    private Transform myTarget; // The projectile's target location.
    private int myDamage; // The damage each of the tower's attacks does.
    private bool amIAerial; // Whether or not the tower can target aerial enemies.
    private float myLife; // How long the projectile's lifespan is - influences range.
    private float mySpeed; // How fast the projectile moves. Influences range and accuracy.
    private int myPierce; // How many enemies the projectile can pass through and damage in its lifespan.
    private float deathTime; // The time the projectile will 'die' at - Time.time + myLife.
    private Vector2 direction; // The direction the projectile travels in.

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Gets the projectile's attached rigidbody component.
    }

    private void Update()
    {
        if (Time.time >= deathTime) // If the projectile's death time is due,
        {
            Destroy(gameObject); // it should be destroyed.
        }
    }

    private void FixedUpdate()
    {
        if (!myTarget)
        {
            return;
        }
        rb.velocity = direction * mySpeed; // Accelerate the projectile in the given direction.
    }

    public void SetAttributes(Transform target, int damage, bool aerial, float life, float speed, int pierce) // Called by towers to set up the attributes of a projectile.
    {
        myTarget = target;
        myDamage = damage;
        amIAerial = aerial;
        myLife = life;
        mySpeed = speed;
        myPierce = pierce;

        deathTime = Time.time + myLife; // Set the death time to current time + the projectile life span.
        direction = (myTarget.position - transform.position).normalized; // Set the projectile's direction to the enemy.
    }

    private void OnTriggerEnter2D(Collider2D other) // When the projectile collides with another object,
    {
        if (other.gameObject.layer == myTarget.gameObject.layer) // if the object is on the target (enemy) layer,
        {
            other.gameObject.GetComponent<BaseEnemy>().TakeDamage(myDamage); // the object (enemy) should take damage,
            myPierce -= 1; // the remaining pierce should decrease by one,
            if (myPierce == 0) // and if remaining pierce hits zero,
            {
                Destroy(gameObject); // the projectile should be destroyed.
            }
        }
    }
}