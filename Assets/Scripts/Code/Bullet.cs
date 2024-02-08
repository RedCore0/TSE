using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Rigidbody2D rb; // Collision / hitbox for projectile

    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 5f; // Projectile Speed
    [SerializeField] private int bulletDamage = 1; // The damage the projectile will do
    [SerializeField] private float lifeSpan = 10f; // How long the bullet can be alive

    private Transform target; // The projectiles target

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    private void OnCollisionEnter2D(Collision2D other) // Once the projectile hits the targte destroy the projectile
    {
        other.gameObject.GetComponent<Health>().TakeDamage(bulletDamage);
        Destroy(gameObject);
    } 

    private void CheckBulletTimeout() // If the bullet has existed for too long it has likely missed
    {
        lifeSpan -= Time.deltaTime;
        if (lifeSpan <= 0f)
        {
            Destroy(gameObject);
        }
    }

    //Is executed based on a Fixed Timestep (by default 50 times per second)
    void FixedUpdate()
    {
        if (!target)
        {
            return;
        }
        Vector2 direction = (target.position - transform.position).normalized; // Tracking so the projectile follows the enemy

        rb.velocity =  direction * bulletSpeed; // Accelerate the projectile towards the enemy
    }

    //Is executed every frame
    void Update()
    {
        CheckBulletTimeout();
    }

}
