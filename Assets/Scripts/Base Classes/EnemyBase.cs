using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;

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

    //public float range        // range of enemy, if we use it

    //  public string element;  // if we use it, will probably increase damage by up to 2x if weak, and decrease by 0.5x if resistent
                                // could be implemented in one of two ways:
                                // a system where the selected element(s) are what the enemy is weak to
                                // or a system where the selected element is what the enemy is 'attuned to', similar to pokemon, and this element is weak/strong against others


    private GameObject enemyPath = GameObject.Find("Path");     // finds and stores a reference to the path object
    private GameObject[] pathNodes;
    private int nextNode = 0;

    // Start is called before the first frame update
    void Awake()
    {
        pathNodes = enemyPath.GetComponents<GameObject>();      // places all nodes on the path into an array
    }

    // Fixed Update is called once per fixed interval
    void FixedUpdate()
    {
        if(Vector2.Distance(this.transform.position, pathNodes[nextNode].transform.position) > 0)
        {
            if(Vector2.SignedAngle(this.transform.position, pathNodes[nextNode].transform.position) != 0)
            this.transform.rotation = Vector2.SignedAngle(this.transform.position, pathNodes[nextNode].transform.position);
                       
            // move by x * speed
        }
        else if(nextNode++ < pathNodes.Length-1) 
        {
            nextNode++;
        }
        else //if range > distance to endnode
        {
            Attack();
        }
    }

    public virtual void Attack()
    {
        // basic attack script if necessary, but will probably be overwritten per enemy
    }
}
