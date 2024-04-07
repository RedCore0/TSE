using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : BaseEnemy
{
    [Header("Dragon specific stats")]
    public int shotsPerBurst; // The number of shots the dragon will fire in a single burst.
    public float timeBetweenShots; // The time (seconds) between each shot in a burst.
    
    public override void DoAttack() // Enenmy attacks structure
    {
        if (attackLimit > 0)
        {
            attackLimit -= 1;
            StartCoroutine(AttackBurst());  // A Dragon will attack in a burst, like a flamethrower
        }                                          

        else
        {
            DestroyEnemy(); // Destroys enemy when it reaches its limit
        }
    }
    
    private IEnumerator AttackBurst() // Coroutine for the dragon's burst attack
    {
        for (int i = 0; i < shotsPerBurst; i++)
        {
            LevelManager.main.DamageStructure(attackDamage);    // The structure will take a hit of damage
            yield return new WaitForSeconds(timeBetweenShots);  // And then the coroutine will wait for a short time before causing damage again
        }
    }
}
