using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : BaseEnemy
{
    [Header("Slime specific stats")]
    public int splitCount; // The number of slimes spawned when the slime dies.
    public float jumpTime; // The time between each jump.
    [Tooltip("Length of time the slime is jumping for.")]
    public float jumpLength; // The length of time the slime is jumping for. 
    public bool canSplit; // Whether or not the slime can split.
    public GameObject splitSlime; // The Creature that the original slime will split into. (maybe multiple types?)

    private float waitTime; // The amount of time since the last jump
    private float jumpSpeed; // The speed of the jump
    private bool isJumping; // Whether or not the slime is currently jumping
    [SerializeField] private GameObject gameManager; // Reference to the game manager

    public override void Start()
    {
        base.Start();
        waitTime = Time.time;
        jumpSpeed = enemySpeed;
        enemySpeed = 0;
    }

    // Update is called once per frame
    public override void Update()
    {
        if ((Time.time - waitTime >= jumpTime) && !isJumping)
        {
            enemySpeed = jumpSpeed;
            isJumping = true;
            isAerial = true;
            waitTime = Time.time;
        }

        if ((Time.time - waitTime >= jumpLength) && isJumping)
        {
            enemySpeed = 0;
            isJumping = false;
            isAerial = false;
            waitTime = Time.time;
        }

        base.Update();
    }
    
    public override void DestroyEnemy()
    {
        if (canSplit)
        {
            for (int i = 0; i < splitCount; i++)
            {
                gameManager.GetComponent<EnemySpawner>().incrementCount();
                GameObject newSlime = Instantiate(splitSlime, transform.position, transform.rotation);
                
                newSlime.GetComponent<Slime>().pathIndex = pathIndex;
                newSlime.GetComponent<Slime>().waitTime += Random.Range(0, jumpTime);
            }
        }
        base.DestroyEnemy();
    }
}
