using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BaseTower : MonoBehaviour
{
    private Transform targetLocation; // The location of the tower's current target.
    private GameObject targettedEnemy; // The enemy being targetted by the tower.
    private List<GameObject> targetsInRange; // A list of targets that are in the tower's range.
    private string targettingMode; // The targetting mode of the tower.
    private bool prioritiseAerial; // Whether the tower should prioritise aerial targets.
    private float nextAttack; // The next time the tower can attack.

    [Header("References")]
    [SerializeField] private LayerMask enemyMask; // The mask enemies are on - where the tower targets. - side idea, we could have non-enemies in here to distract towers?
    [SerializeField] private GameObject projectilePrefab; // The projectile the tower will shoot with every attack. Empty means it is a melee/support/special tower.


    [Header("Stats")]
    [SerializeField] private float attackRange; // The range the tower can attack enemies within.
    [SerializeField] private float attackDelay; // The delay between each tower attack.
    [SerializeField] private int attackDamage; // The damage each of the tower's attacks does.
    [SerializeField] private bool aerialTargetting; // Whether or not the tower can target aerial enemies.

    [SerializeField] private float projectileLife; // How long the projectile's lifespan is - influences range.
    [SerializeField] private float projectileSpeed; // How fast the projectile moves. Influences range and accuracy.
    [SerializeField] private int projectilePierce; // How many enemies the projectile can pass through and damage in its lifespan.
    [SerializeField] private int projectileCount; // How many projectiles are shot per attack from the tower.

    [Space(20)]
    [TextArea]
    public string towerDescription; // The tower's description.

    private void OnDrawGizmosSelected() // Shows the tower's range while selected in the editor.
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, transform.forward, attackRange);
    }

    private void Start() // Called when the object is instantiated.
    {
        targettingMode = "FIRST"; // Set targetting mode to the initial mode. (FIRST is the default)
        nextAttack = Time.time + attackDelay; // Sets next attack time. (the current time + the delay between attacks)
    }

    private void Update() // Called every frame.
    {
        targetsInRange = FindTargets(); // Gets the targets (enemies) in the tower's range.
        targettedEnemy = SelectTarget(); // Selects a single target to fire at based on tower priorities.
        if (targettedEnemy != null) // If an enemy to target has been found,
        {
            targetLocation = targettedEnemy.transform; // get the selected target's location.
            AimAtTarget(targetLocation); // and aim at the target's location.
            if (Time.time >= nextAttack) // If the next attack is due,
            {
                DoAttack(targetLocation); // the tower should attack the target,
                nextAttack = Time.time + attackDelay; // and the cooldown should reset.
            }
        }
    }

    private List<GameObject> FindTargets() // Find all targets in range of the tower, return as a list.
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

        return targets;
    }

    private GameObject SelectTarget() // Select a target from the targets in the tower's range.
    {
        GameObject target = null;
        List<GameObject> validTargets = new List<GameObject>();

        if (prioritiseAerial) // If aerial priority is enabled,
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

        if (validTargets.Count <= 0) // If, after priority filters, there are no valid targets,
        {
            validTargets = targetsInRange; // simply add all targets in range as valid targets.
        }

        if (validTargets.Count <= 0) // If there are still no valid targets in range at this point,
        {
            return target; // return target as null.
        }

        switch (targettingMode)
        {
            case "FIRST": // Will find the target furthest ahead on the track.
                target = validTargets[0]; // FIX: Placeholder for now, actual implementation needed.
                break;
            case "LAST": // Will find the target furthest back on the track.
                break;
            case "HIGH HP": // Will find the target with the highest health.
                break;
            case "HIGH DEF": // Will find the target with the highest defense.
                break;
            case "FASTEST": // Will find the target with the highest speed.
                break;
        }

        return target;
    }

    private void AimAtTarget(Transform target) // Aims at a given target based on its transform.
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        transform.rotation = targetRotation; // 'Snappy' tower rotation, immediately snaps aim to the target.
    }

    private void DoAttack(Transform target) // Attacks at a given target's position.
    {
        GameObject projectileObj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        BaseProjectile projectileScript = projectileObj.GetComponent<BaseProjectile>();
        projectileScript.SetAttributes(target, attackDamage, aerialTargetting, projectileLife, projectileSpeed, projectilePierce);
    } // TO-DO : Modify later to account for multiple projectiles, spread, etc.

    private bool CheckTargetIsInRange(Transform target)  // Ensures the tower doesn't attack a target that has left its range.
    {
        return Vector2.Distance(target.position, transform.position) <= attackRange;
    }
}