using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBase : MonoBehaviour
{
    [Header("Stats")]

    [Tooltip("Range of unit (measurement unit currently undefined)")]
    public float range;             // my initial idea is for both sight and projectiles to expire at limit, but this can be changed at any time. for better measurement maybe tile widths?

    [Tooltip("Attack speed of unit (attack/s)")]
    public float attackSpeed;

    [Tooltip("Damage of unit")]
    public int damage;              // can be adjusted to floats if we need to

    [Tooltip("Speed of projectiles (measurement unit currently undefined)")]
    public float projectileSpeed;   // can probably be overrode/ignored if unit doesnt use projectiles, tiles/s for measurement?

    [Tooltip("Amount of targets a units projectile can damage before expiring")]
    public float pierce;

    //  [Space(20)]

    //  public string element;          // will refine into a list thats more easiliy usable later if we do go with the element idea

    [Space(20)]
    [TextArea]
    public string description;      // unit description for menus


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Fixed Update is called once per fixed interval
    void FixedUpdate()
    {
        if(Vector3.Distance(this.transform.position, /*enemy.*/transform.position) < range) 
        {
            //turn towards enemy
            //shoot();
        }
    }

    public virtual void shoot()
    {

    }
}
