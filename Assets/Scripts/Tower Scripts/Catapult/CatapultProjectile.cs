using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultProjectile : BaseProjectile
{
    public override void SetUp(BaseTower owner, Transform target, int damage, bool aerial, float life, float speed, int pierce)
    {
        base.SetUp(owner, target, damage, aerial, life, speed, pierce);
        direction = owner.transform.up; // Resets the projectile's direction to the direction of the catapult.
        Debug.Log("word");
    }
}
