using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poltergeist : BaseEnemy
{
    // The poltergeist (as of typing) will attack in 2 hits, once with each claw and with a small delay between each
    [Header("Poltergeist specific stats")]
    public int hitsPerAttack; 
    public float timeBetweenHits; 
    
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
        for (int i = 0; i < hitsPerAttack; i++)
        {
            LevelManager.main.DamageStructure(attackDamage);    // The structure will take a hit of damage
            yield return new WaitForSeconds(timeBetweenHits);  // And then the coroutine will wait for a short time before causing damage again
        }
    }
}
