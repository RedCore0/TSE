using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Necromancer : BaseTower
{
    protected Transform summonTargetLocation; // The location of the necromancer's current target for summon attacking.
    protected GameObject summonTargettedEnemy; // The enemy being targetted by the necromancer's summon attack.
    protected List<GameObject> summonTargetsInRange; // A list of targets that are in the necromancer's summon attack range.
    protected float summonNextAttack; // The next time the necromancer can summon attack.

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
    [SerializeField] private GameObject[] summonEnemies; // The prefabs for enemies which correspond to specific summon projectiles.
    [SerializeField] private GameObject[] summonProjectiles; // The prefabs for projectiles which can be used in summon attacks.

    public void StockAdd(string summon)
    {
        summonStock.Add(summon);
    }
}