using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Necromancer : BaseTower
{
    private Transform summonTargetLocation; // The location of the necromancer's current target for summon attacking.
    private GameObject summonTargettedEnemy; // The enemy being targetted by the necromancer's summon attack.
    private List<GameObject> summonTargetsInRange; // A list of targets that are in the necromancer's summon attack range.
    private float summonNextAttack; // The next time the necromancer can summon attack.

    private GameObject summonProjectilePrefab; // The projectile of the current summon attack.
    private float summonAttackRange;
    private float summonAttackDelay;

    private List<string> summonStock; // The necromancer's summon stock - the summon attacks available to it.
    private string upcomingSummon; // The summon the necromancer will use in its next summon attack.
    // "CKNIGHT"
    // "DRAGON"
    // "GOBLIN"
    // "SLIME"
    // "PGEIST"
    // "SKELETON"
    // "WITCH"

    [Header("Necromancer References")]
    [SerializeField] private GameObject[] summonProjectiles; // The prefabs for projectiles which can be used in summon attacks.

    public void StockAdd(string summon)
    {
        summonStock.Add(summon);
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected(); // For the melee attack's range.
        Handles.color = Color.blue; // For the current summon attack's range.
        Handles.DrawWireDisc(transform.position, transform.forward, summonAttackRange);
    }

    protected override void Start()
    {
        base.Start();
        summonNextAttack = Time.time;
        summonStock = new List<string>();
    }

    protected override void Update()
    {
        // Summon attack handling:
        if (summonStock.Count > 0)
        {
            upcomingSummon = summonStock[0];
            summonStock.RemoveAt(0);
            SetUpSummon();
            summonTargetsInRange = FindSummonTargets();
            summonTargettedEnemy = SelectSummonTarget();

            if (summonTargettedEnemy != null && Time.time > summonNextAttack)
            {
                summonTargetLocation = summonTargettedEnemy.transform;
                AimAtTarget(summonTargetLocation);
                DoSummonAttack(summonTargetLocation);
                summonNextAttack = Time.time + summonAttackDelay;
            }
        }

        // Melee attack handling:
        targetsInRange = base.FindTargets();
        targettedEnemy = SelectWeakestTarget();

        if (targettedEnemy != null && Time.time >= nextAttack)
        {
            targetLocation = targettedEnemy.transform;
            AimAtTarget(targetLocation);
            DoAttack(targetLocation);
            nextAttack = Time.time + attackDelay;
        }
    }

    private void SetUpSummon() // Sets up a summon attack.
    {
        switch (upcomingSummon) // There may be a more efficient way, but for sake of ease
        { // I manually set the projectiles up here on a case by case basis.
            case "CKNIGHT":
                summonProjectilePrefab = summonProjectiles[0];
                summonAttackRange = 20;
                summonAttackDelay = 1;
                break;

            case "DRAGON":
                summonProjectilePrefab = summonProjectiles[0];
                summonAttackRange = 20;
                summonAttackDelay = 1;
                break;

            case "GOBLIN":
                summonProjectilePrefab = summonProjectiles[0];
                summonAttackRange = 20;
                summonAttackDelay = 1;
                break;

            case "SLIME":
                summonProjectilePrefab = summonProjectiles[0];
                summonAttackRange = 20;
                summonAttackDelay = 1;
                break;

            case "PGEIST":
                summonProjectilePrefab = summonProjectiles[0];
                summonAttackRange = 20;
                summonAttackDelay = 1;
                break;

            case "SKELETON":
                summonProjectilePrefab = summonProjectiles[0];
                summonAttackRange = 20;
                summonAttackDelay = 1;
                break;

            case "WITCH":
                summonProjectilePrefab = summonProjectiles[0];
                summonAttackRange = 20;
                summonAttackDelay = 1;
                break;
        }
    }

    private List<GameObject> FindSummonTargets() // Finds targets in range of the summon attack.
    {
        List<GameObject> summonTargets = new List<GameObject>();
        float temp = attackRange; // Temporarily store our attack range,
        attackRange = summonAttackRange; // set it to our summon attack's range,
        summonTargets = base.FindTargets(); // find targets using that range,
        attackRange = temp; // set the normal attack range back to its true value,
        return summonTargets; // and return the list of summon targets.
    }

    private GameObject SelectSummonTarget() // Selects the summon attack's target.
    { // Same logic as the above method, but temporarily swaps targets in range instead.
        GameObject summonTarget = null;
        List<GameObject> temp = targetsInRange;
        targetsInRange = summonTargetsInRange;
        summonTarget = base.SelectTarget();
        targetsInRange = temp;
        return summonTarget;
    }

    private GameObject SelectWeakestTarget() // Selects the weakest target in range.
    {
        GameObject target = null; // Initialise target to null.
        if (targetsInRange.Count <= 0)
        {
            return target; // If there are no targets in range, return null.
        }
        target = targetsInRange[0];
        foreach (GameObject checkTarget in targetsInRange) // Otherwise, for each target in range,
        {
            BaseEnemy currentTarget = target.GetComponent<BaseEnemy>(); // compare our current target,
            BaseEnemy possibleTarget = checkTarget.GetComponent<BaseEnemy>(); // and the next possible target.
            if (possibleTarget.GetHealth() < currentTarget.GetHealth())
            { // Set our current target to whichever has less health.
                target = checkTarget;
            }
        }
        return target; // Return the final target we're left with.
    }

    private void DoSummonAttack(Transform target)
    {
        GameObject temp = projectilePrefab;
        projectilePrefab = summonProjectilePrefab;
        base.DoAttack(target);
        projectilePrefab = temp;
    }
}