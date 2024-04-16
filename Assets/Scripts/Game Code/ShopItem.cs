using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopItem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI towerLabelUI;
    [SerializeField] GameObject towerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        towerLabelUI = towerPrefab.GetTowerName + " - " + (towerPrefab.GetTowerCost.toString());
    }

}
