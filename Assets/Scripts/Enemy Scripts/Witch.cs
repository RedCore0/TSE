using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Witch : BaseEnemy
{
    // The witch will be considered a tankier unit and has a shield that will regenerate after a period of time
    // The following code has been set up so that this shield can be adjusted so that it can regenerate in bursts or all in one, with varying times between start and bursts
    // An optional idea is that damage to the witch would prevent shield regen, but this has not been implemented yet
    // Another idea is that the witches speed could increase when the shield breaks
    [Header("Witch specific stats")] 
    public int shieldHealth;
    public int shieldDefense;
    public float shieldRegenStartTime; // Time before shield starts regenerating
    public float shieldRegenTime; // Time between bursts of shield health restoration
    public int shieldRegenAmount; // Amount of shield health restored per burst

    private bool shieldBroken = false;
    private float shieldWaitTime;   // Remaining time before shield regen starts
    private int maxShieldHealth;    // Additional declaration to ensure shield health doesnt go over the inital value

    public override void Start()    // Appends additional lines to the start method 
    {
        base.Start();
        maxShieldHealth = shieldHealth;
        shieldWaitTime = Time.time;
    }

    public override void Update()   // Appends the additional line to the update method
    {
        if (shieldBroken && Time.time > shieldWaitTime) { RegenShield(); }  // After the time has passed, shield will regen
        base.Update();
    }
    
    public override int TakeDamage(int incomingDamage) 
    {
        if (!shieldBroken)  // Applies damage the same way that the base class does, but to the the shield initally instead
        {
            incomingDamage -= shieldDefense;
            if (incomingDamage <= 0) { incomingDamage = 1; }
            shieldHealth -= incomingDamage;
            if (shieldHealth <= 0) { BreakShield(); }
            return 0;
        }
        
        else { return base.TakeDamage(incomingDamage); }   // run standard function if theres no shield
    }
    
    private void BreakShield()  // Disables shield and begins wait time for regen
    {
        shieldBroken = true;
        shieldHealth = 0; // Prevents underflow
        gameObject.transform.Find("Shield").gameObject.SetActive(false);
        shieldWaitTime = Time.time + shieldRegenStartTime;
    }
    
    private void RegenShield()  // Enables shield and begins regen
    {
        shieldBroken = false;
        gameObject.transform.Find("Shield").gameObject.SetActive(true);
        StartCoroutine(ShieldHealthRegen());
    }

    
    private IEnumerator ShieldHealthRegen() // Coroutine for the witch's shield regen
    {
        int regenTally = 0; // A tally of how much shield has been regenerated is kept so that the witch doesnt accidentally become immortal
        while (shieldHealth <= maxShieldHealth && regenTally < maxShieldHealth)
        {
            shieldHealth += shieldRegenAmount;
            regenTally += shieldRegenAmount;
            yield return new WaitForSeconds(shieldRegenTime);
        }
        if (shieldHealth > maxShieldHealth) { shieldHealth = maxShieldHealth; } // Prevents overflow
    }
}
