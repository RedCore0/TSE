using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Catapult : BaseTower
{
    protected override void OnDrawGizmosSelected() // Shows the tower's range while selected in the editor.
    {
        Handles.color = Color.red;
        Vector3 rangeEnd = transform.position + transform.up * attackRange;
        Handles.DrawLine(transform.position, rangeEnd);
    }

    protected override void Start()
    {
        base.Start();
        targettingMode = "UP"; // Reset targetting mode to be "UP" by default.
    }

    protected override void Update()
    {
        switch (targettingMode) // Rotate the catapult based on targetting mode.
        {
            case "UP":
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;

            case "LEFT":
                transform.rotation = Quaternion.Euler(0, 0, 90);
                break;

            case "DOWN":
                transform.rotation = Quaternion.Euler(0, 0, 180);
                break;

            case "RIGHT":
                transform.rotation = Quaternion.Euler(0, 0, 270);
                break;
        }

        targetsInRange = FindTargets();
        if ( targetsInRange.Count > 0 ) // If there are any targets in range,
        {
            targettedEnemy = targetsInRange[0]; // set targetted enemy so we can attack.
        }
        else // Otherwise,
        {
            targettedEnemy = null; // set targetted enemy to null.
        }

        if (targettedEnemy != null && Time.time >= nextAttack) // If an enemy to target has been found, and the next attack is due,
        {
            targetLocation = targettedEnemy.transform; // then get the selected target's position,
            DoAttack(targetLocation); // attack in that direction,
            nextAttack = Time.time + attackDelay; // and reset the attack cooldown.
        }
    }

    protected override List<GameObject> FindTargets()
    {
        List<GameObject> targets = new List<GameObject>(); // A list of targets found in the tower's range.
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.up, attackRange, enemyMask); // Raycast to see enemies in range.

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
}
