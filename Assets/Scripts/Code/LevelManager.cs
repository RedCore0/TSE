using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{

    public static LevelManager main;

    public Transform startPoint;
    public Transform[] path; // An array of points that the enemy will follow

    public int currency; // Currency value that will be used to purchase towers / upgrades

    public int structureHealth;     // Health of the structure, could vary between level/difficulty

    
    public void IncreaseCurrency(int ammount)
    {
        currency += ammount;
    }

    public bool SpendCurrency(int ammount)
    {
        if (ammount  <= currency)
        {
            //Buy
            currency -= ammount;
            return true;
        }
        else
        {
            Debug.Log("Insufficient Currency");
            return false;
        }
    }

    public void Awake() 
    {
        main = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currency = 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StructureDamage(int damage)
    {
        structureHealth -= damage;  // reduce structure health

        if (structureHealth <= 0 )  // temporary destroy 'animation'
        {
            Transform finalNode = path[path.Length - 1];
            finalNode.GetChild(0).gameObject.SetActive(false);  // structure is destroyed
            finalNode.position += Vector3.right * 5;            // and enemies can now proceed through
        }
    }
}
