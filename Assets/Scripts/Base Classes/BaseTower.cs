using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BaseTower : MonoBehaviour
{
    protected Transform targetLocation; // The location of the tower's current target.
    protected GameObject targettedEnemy; // The enemy being targetted by the tower.
    protected List<GameObject> targetsInRange; // A list of targets that are in the tower's range.
    protected string targettingMode; // The targetting mode of the tower.
    protected bool prioritiseAerial; // Whether the tower should prioritise aerial targets.
    protected float nextAttack; // The next time the tower can attack.

    [Header("References")]
    [SerializeField] protected LayerMask enemyMask; // The mask enemies are on - where the tower targets.
    [SerializeField] protected GameObject projectilePrefab; // The projectile the tower will shoot with every attack.
    [SerializeField] protected BaseUpgrade[] towerUpgrades; // A list of upgrades accessible by this tower.


    [Header("Attributes")]
    [SerializeField] protected float attackRange; // The range the tower can attack enemies within.
    [SerializeField] protected float attackDelay; // The delay between each tower attack.
    [SerializeField] protected int attackDamage; // The damage each of the tower's attacks does.
    [SerializeField] protected bool aerialTargetting; // Whether or not the tower can target aerial enemies.

    [SerializeField] protected float projectileLife; // How long the projectile's lifespan is - influences range.
    [SerializeField] protected float projectileSpeed; // How fast the projectile moves. Influences range and accuracy.
    [SerializeField] protected int projectilePierce; // How many enemies the projectile can pass through and damage in its lifespan.

    [Header("Statistics")]
    [SerializeField] public int damageDealt; // Total damage dealt by this tower, across its whole lifetime.
    [SerializeField] public int lastDamageDealt; // Damage dealt by this tower in just the previous round.

    [Header("Info")]
    public string towerName; // The tower's name.
    public int towerCost; // The tower's cost to build.
    [Space(20)]
    [TextArea]
    public string towerDescription; // The tower's description.

    public int GetTowerCost() // Getter for tower cost
    {
        return towerCost;
    }

    public string GetTowerName() // Getter for tower name
    {
        return towerName;
    }

    public float GetTowerFireRate() // Getter for tower attack delay
    {
        return 1 / attackDelay;
    }

    public int GetTowerDamage() // Getter for tower damage
    {
        return attackDamage;
    }

    public int GetTowerAerial()
    {
        if (aerialTargetting)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    protected virtual void OnDrawGizmosSelected() // Shows the tower's range while selected in the editor.
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, transform.forward, attackRange);
    }

    protected virtual void Start() // Called when the object is instantiated.
    {
        targettingMode = "FIRST"; // Set targetting mode to the initial mode. (FIRST is the default)
        prioritiseAerial = aerialTargetting; // By default, towers with aerial targetting will prioritise aerial units.
        nextAttack = Time.time + attackDelay; // Sets next attack time. (the current time + the delay between attacks)
    }

    protected virtual void Update() // Called every frame.
    {
        targetsInRange = FindTargets(); // Gets the targets (enemies) in the tower's range.
        targettedEnemy = SelectTarget(); // Selects a single target to fire at based on tower priorities.

        if (targettedEnemy != null && Time.time >= nextAttack) // If an enemy to target has been found, and the next attack is due,
        {
            targetLocation = targettedEnemy.transform; // then get the selected target's position,
            AimAtTarget(targetLocation); // aim the tower at the target,
            DoAttack(targetLocation); // attack in that direction,
            nextAttack = Time.time + attackDelay; // and reset the attack cooldown.
        }
    }

    protected virtual List<GameObject> FindTargets() // Find all targets in range of the tower, return as a list.
    {
        List<GameObject> targets = new List<GameObject>(); // A list of targets found in the tower's range.
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, attackRange, (Vector2)transform.position, 0f, enemyMask); // Raycast to see enemies in range.

        if (hits.Length > 0) // If any hits were made,
        {
            foreach (RaycastHit2D hit in hits) // for each hit,
            {
                if (!targets.Contains(hit.transform.gameObject)) // if the target is not already found,
                {
                    targets.Add(hit.transform.gameObject); // add it to the list of valid targets.
                }
            }
        }

        return targets; // Return the list of targets in range.
    }

    protected virtual GameObject SelectTarget() // Select a target from the targets in the tower's range.
    {
        GameObject target = null;
        List<GameObject> validTargets = new List<GameObject>();

        if (prioritiseAerial && aerialTargetting) // If aerial priority is enabled (and we have aerial targetting),
        {
            foreach (GameObject checkTarget in targetsInRange) // for each target in range,
            {
                BaseEnemy checkScript = checkTarget.GetComponent<BaseEnemy>();
                if (checkScript.IsAerial()) // if the target is aerial,
                {
                    validTargets.Add(checkTarget); // add it to the list of valid targets.
                }
            }
        }

        if (!aerialTargetting) // If we do not have aerial targetting,
        {
            foreach (GameObject checkTarget in targetsInRange) // for each target in range,
            {
                BaseEnemy checkScript = checkTarget.GetComponent<BaseEnemy>();
                if (!checkScript.IsAerial()) // if the target is not aerial,
                {
                    validTargets.Add(checkTarget); // add it to the list of valid targets.
                }
            }
        }

        else // Otherwise,
        {
            validTargets = targetsInRange; // simply add all targets in range as valid targets.
        }

        if (validTargets.Count <= 0) // If there are still no valid targets in range at this point,
        {
            return target; // return target as null.
        }

        target = validTargets[0];
        switch (targettingMode)
        {
            case "FIRST": // Will find the target furthest ahead on the track.
                foreach (GameObject checkTarget in validTargets)
                {
                    BaseEnemy currentTarget = target.GetComponent<BaseEnemy>();
                    BaseEnemy possibleTarget = checkTarget.GetComponent<BaseEnemy>();
                    if (possibleTarget.GetDistance() > currentTarget.GetDistance())
                    {
                        target = checkTarget;
                    }
                }
                break;

            case "LAST": // Will find the target furthest back on the track.
                foreach (GameObject checkTarget in validTargets)
                {
                    BaseEnemy currentTarget = target.GetComponent<BaseEnemy>();
                    BaseEnemy possibleTarget = checkTarget.GetComponent<BaseEnemy>();
                    if (possibleTarget.GetDistance() < currentTarget.GetDistance())
                    {
                        target = checkTarget;
                    }
                }
                break;

            case "HIGH HP": // Will find the target with the highest (current) health.
                foreach (GameObject checkTarget in validTargets)
                {
                    BaseEnemy currentTarget = target.GetComponent<BaseEnemy>();
                    BaseEnemy possibleTarget = checkTarget.GetComponent<BaseEnemy>();
                    if (possibleTarget.GetHealth() > currentTarget.GetHealth())
                    {
                        target = checkTarget;
                    }
                }
                break;

            case "HIGH DEF": // Will find the target with the highest defense.
                foreach (GameObject checkTarget in validTargets)
                {
                    BaseEnemy currentTarget = target.GetComponent<BaseEnemy>();
                    BaseEnemy possibleTarget = checkTarget.GetComponent<BaseEnemy>();
                    if (possibleTarget.GetDefense() > currentTarget.GetDefense())
                    {
                        target = checkTarget;
                    }
                }
                break;

            case "FASTEST": // Will find the target with the highest speed.
                foreach (GameObject checkTarget in validTargets)
                {
                    BaseEnemy currentTarget = target.GetComponent<BaseEnemy>();
                    BaseEnemy possibleTarget = checkTarget.GetComponent<BaseEnemy>();
                    if (possibleTarget.GetSpeed() > currentTarget.GetSpeed())
                    {
                        target = checkTarget;
                    }
                }
                break;
        }

        return target; // Return the selected target.
    }

    protected virtual void AimAtTarget(Transform target) // Aims at a given target based on its transform.
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        transform.rotation = targetRotation; // 'Snappy' tower rotation, immediately snaps aim to the target.
    }

    protected virtual void DoAttack(Transform target) // Attacks at a given target position.
    {
        GameObject projectileObj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        BaseProjectile projectileScript = projectileObj.GetComponent<BaseProjectile>();
        projectileScript.SetUp(this, target, attackDamage, aerialTargetting, projectileLife, projectileSpeed, projectilePierce);
        if (GetComponent<AudioSource>() != null) { GetComponent<AudioSource>().Play(); }
    }

    protected virtual bool CheckTargetIsInRange(Transform target)  // Ensures the tower doesn't attack a target that has left its range.
    {
        return Vector2.Distance(target.position, transform.position) <= attackRange;
    }
}