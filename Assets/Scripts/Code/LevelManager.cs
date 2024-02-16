using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public static LevelManager main;

    public Transform startPoint;
    public Transform[] path; // An array of points that the enemy will follow

    public int currency; // Currency value that will be used to purchase towers / upgrades

    
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
}
