using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromancerProjectile : BaseProjectile
{
    // features:
    // when this projectile kills an enemy, send the enemy killed to the host necromancer
    // the enemy will be used to add to the summon stock

    // ideally, this projectile, although used for the necro slash attack,
    // can also be inherited by the summon projectiles to reduce their implementation
}
