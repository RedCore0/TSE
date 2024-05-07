using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BaseProjectile : MonoBehaviour
{
    protected Rigidbody2D rb; // The projectile's collision hitbox.
    protected float deathTime; // The time the projectile will 'die' at - set to Time.time + myLife.
    protected Vector2 direction; // The direction the projectile travels in.
    protected LayerMask enemyMask; // The mask that enemies are on - what the projectile can target.

    protected BaseTower myOwner; // The tower which fired this projectile.
    protected Transform myTarget; // The projectile's target location.
    protected int myDamage; // The incoming damage this projectile will deal to an enemy.
    protected bool amIAerial; // Whether or not the projectile is aerial - if it can hit aerial enemies.
    protected float myLife; // How long the projectile's lifespan is. Influences range.
    protected float mySpeed; // How fast the projectile moves. Influences range and accuracy.
    protected int myPierce; // How many seperate enemies the projectile can pass through and damage in its lifespan.

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Gets the projectile's attached rigidbody component.
    }

    protected virtual void Update()
    {
        if (Time.time >= deathTime) // If the projectile's death time is due,
        {
            Destroy(gameObject); // it should be destroyed.
        }
    }

    protected virtual void FixedUpdate()
    {
        if (!myTarget)
        {
            return;
        }
        rb.velocity = direction * mySpeed; // Accelerate the projectile in the given direction.
    }

    public virtual void SetUp(BaseTower owner, Transform target, int damage, bool aerial, float life, float speed, int pierce) // Called by towers to set up a projectile.
    {
        myOwner = owner;
        myTarget = target;
        myDamage = damage;
        amIAerial = aerial;
        myLife = life;
        mySpeed = speed;
        myPierce = pierce;

        deathTime = Time.time + myLife; // Set the death time to current time + the projectile life span.
        direction = (myTarget.position - transform.position).normalized; // Set the projectile's direction to the enemy.

        float angle = Mathf.Atan2(myTarget.position.y - transform.position.y, myTarget.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        transform.rotation = targetRotation; // Aim the projetile at the targetted enemy.


        enemyMask = myTarget.gameObject.layer; // Set the enemy mask to the layer the target is on.
    }

    protected virtual void OnTriggerEnter2D(Collider2D other) // When the projectile collides with another object,
    {
        if (other.gameObject.layer == enemyMask) // if the object is on the target (enemy) layer,
        {
            BaseEnemy enemy = other.gameObject.GetComponent<BaseEnemy>(); // we know it's an enemy, so get the enemy script.

            if (enemy.IsAerial()) // If the enemy is aerial,
            {
                if (amIAerial) // and the projectile is also aerial,
                {
                    HitEnemy(enemy); // the projectile can hit the enemy.
                }
                else
                {
                    return; // otherwise, the projectile misses the enemy.
                }
            }

            else // otherwise, if the enemy is not aerial,
            {
                HitEnemy(enemy); // all projectiles will hit it.
            }
        }
    }

    protected virtual void HitEnemy(BaseEnemy enemy) // When a projectile 'hits' an enemy.
    {
        myOwner.damageDealt += enemy.TakeDamage(myDamage); // The projectile will damage the enemy,
        myPierce -= 1; // decrease remaining pierce by one,
        if (myPierce == 0) // and if remaining pierce hits zero,
        {
            Destroy(gameObject); // the projectile should be destroyed.
        }
        // this is '== 0' instead of '<= 0' to allow -1 to represent infinite pierce.
    }
}