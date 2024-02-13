using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [Header("Stats")]
    [Tooltip("Health of enemy")]
    public int health;

    [Tooltip("Defense of enemy (damage dealt is subtracted by this)")]
    public int defense;

    [Tooltip("Speed of enemy (tiles/s)")]
    public float speed;

    [Tooltip ("Damage dealt to structure")]
    public int damage;  
//  public string element;  // if we use it, will probably increase damage by up to 2x if weak, and decrease by 0.5x if resistent
                            // could be implemented in one of two ways:
                            // a system where the selected element(s) are what the enemy is weak to
                            // or a system where the selected element is what the enemy is 'attuned to', similar to pokemon, and this element is weak/strong against others

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Fixed Update is called once per fixed interval
    void FixedUpdate()
    {
        // move by x * speed
    }
}
