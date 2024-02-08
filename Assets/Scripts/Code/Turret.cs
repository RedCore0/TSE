using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Turret : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Transform turretRotationPoint; //Stores the point about which the turret will rotate
    [SerializeField] private LayerMask enemyMask; //Stores the rendering layer that the enemies will be on
    [SerializeField] private GameObject bulletPrefab; //Stores the prefab for the projectile
    [SerializeField] private Transform firingPoint; //Stores the location to be fired at

    [Header("Attributes")]
    [SerializeField] private float targettingRange = 2.5f; //Controls the range of the turret
    [SerializeField] private float rotationSpeed = 200f; //Controls speed of rotation (if used) very slow
    [SerializeField] private float fireRate = 1f; // Controls fire rate (shots per second)

    private Transform target; //Target location variable
    private float timeUntilFire; // Shot cooldown

    private void OnDrawGizmosSelected() //Function that shows the turret range once selected in editor
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, transform.forward, targettingRange);
    }

    private void FindTarget() //Function to find a target
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targettingRange, (Vector2)transform.position, 0f, enemyMask); //Raycast to see if any object in range
        
        if (hits.Length > 0) //Checks to see if any hits were made
        {
            target = hits[0].transform;
        }
    }

    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
         //Math to find the rotation for the turret based on target location


        turretRotationPoint.rotation = targetRotation; //Snappy turret rotation
        //turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationSpeed*Time.deltaTime); //Smooth turret rotation
    }

    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= targettingRange; //Makes sure turret doesn't try to target an enemy that has left it's range
    }

    private void Shoot()
    {
        //Debug.Log("Pew"); //Check to see if code is working
        GameObject BulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        Bullet bulletScript = BulletObj.GetComponent<Bullet>();
        bulletScript.SetTarget(target);
    }


    // Update is called once per frame
    void Update()
    {
        if (target == null) //If the turret isn't currently targetting something -> look for a target
        {
            FindTarget();
            return;
        }

        RotateTowardsTarget();

        if (!CheckTargetIsInRange())
        {
            target = null;
        }
        else
        {
            timeUntilFire += Time.deltaTime;
            if (timeUntilFire >= 1f/ fireRate)
            {
                Shoot();
                timeUntilFire = 0f;
            }
        }
    }
}
