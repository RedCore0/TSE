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
    //public bool isTheSplit; // Whether or not the slime is a split slime (used to resolve an issue where the new slime wouldn't move to the correct place)
    
    private float waitTime; // The amount of time since the last jump
    private float jumpSpeed; // The speed of the jump
    private bool isJumping; // Whether or not the slime is currently jumping
    [SerializeField] private GameObject gameManager; // Reference to the game manager

    public override void Start()
    {
        int tempIndex = 0; 
        if (pathIndex > 0) { tempIndex = pathIndex; }
        
        base.Start();
        
        if (tempIndex > 0) { pathIndex = tempIndex; }  // Resolves an issue where the start procedure would overwrite the parameters set by the parent slime

        waitTime = Time.time;
        waitTime += Random.Range(0, jumpTime*1.5f);
        jumpSpeed = enemySpeed;
        enemySpeed = 0;
        if (pathIndex >= pathPoints.Length) { pathIndex = pathPoints.Length - 1; }  // Resolves an out of bounds error
        targetLocation = pathPoints[pathIndex];
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
                
                newSlime.GetComponent<Slime>().followedPath = followedPath;
                newSlime.GetComponent<Slime>().pathIndex = pathIndex >= pathPoints.Length ? pathIndex-- : pathIndex;
                newSlime.GetComponent<Slime>().transform.position += new Vector3(Random.Range(-0.5f,0.5f), Random.Range(-0.5f,0.5f), 0);
            }
        }
        base.DestroyEnemy();
    }
}
