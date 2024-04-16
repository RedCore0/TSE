using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopItem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI towerLabelUI;
    [SerializeField] private GameObject towerPrefab;

    // Start is called before the first frame update
    public void Start() // Ideally this will take the name and price from the tower prefab and display them in the button's text ui, however it doesn't
    {
        //private string thisTowerName = towerPrefab.GetTowerName();
        //private string thisTowerCost = towerPrefab.GetTowerCost().toString();
        //private string thisTowerLabel = thisTowerName + " - " + thisTowerCost;

        //towerLabelUI.text = thisTowerLabel;
    }
}
