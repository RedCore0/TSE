using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goblinProj : NecromancerProjectile
{
    public override void SetUp(BaseTower owner, Transform target, int damage, bool aerial, float life, float speed, int pierce)
    {
        base.SetUp(owner, target, damage, aerial, life, speed, pierce);
        // Manual overrides of the projectile's values for this summon:
        myDamage = 5;
        myLife = 2;
        mySpeed = 5;
        myPierce = 5;
        deathTime = Time.time + myLife;
    }
}