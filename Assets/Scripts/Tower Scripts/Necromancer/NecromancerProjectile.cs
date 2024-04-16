using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NecromancerProjectile : BaseProjectile
{
    private Necromancer myNecromancer;

    public override void SetUp(BaseTower owner, Transform target, int damage, bool aerial, float life, float speed, int pierce)
    {
        base.SetUp(owner, target, damage, aerial, life, speed, pierce);
        myNecromancer = (Necromancer)myOwner; // Get the Necromancer's script.
    }

    protected override void HitEnemy(BaseEnemy enemy) // When a necromancer projectile 'hits' an enemy.
    {
        myOwner.damageDealt += enemy.TakeDamage(myDamage); // The projectile will damage the enemy,
        myPierce -= 1; // decrease remaining pierce by one,

        if (enemy.enemyHealth <= 0) // check if it killed an enemy,
        {
            AddToStock(enemy); // add the killed enemy to stock,
        }

        if (myPierce == 0) // and if remaining pierce hits zero,
        {
            Destroy(gameObject); // the projectile should be destroyed.
        }
        // this is '== 0' instead of '<= 0' to allow -1 to represent infinite pierce.
    }

    private void AddToStock(BaseEnemy enemy) // Adds the given enemy to the Necromancer's summon stock.
    {
        switch (enemy.enemyName) // Works based on the enemy's in game name.
        {
            case "Corrupted Knight":
                myNecromancer.StockAdd("CKNIGHT");
                break;

            case "Dragon":
                myNecromancer.StockAdd("DRAGON");
                break;

            case "Goblin":
                myNecromancer.StockAdd("GOBLIN");
                break;

            case "Slime":
                myNecromancer.StockAdd("SLIME");
                break;

            case "Poltergeist":
                myNecromancer.StockAdd("PGEIST");
                break;

            case "Skeleton":
                myNecromancer.StockAdd("SKELETON");
                break;

            case "Witch":
                myNecromancer.StockAdd("WITCH");
                break;
        }
    }

    private void IgnoreDef()
    {
        if (myPierce < 0) // If pierce is less than zero,
        {
            // we know this projectile is the scythe melee attack:
        }
    }
}