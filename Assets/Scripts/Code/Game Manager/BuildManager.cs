using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{

    public static BuildManager main;
    private int selectedTower = 0; // The index of the tower currently selected by the player.

    [Header("References")]
    [SerializeField] private GameObject[] availableTowers; // Array of all tower types available to the player.
    
    private void Awake()
    {
        main = this;
    }

    public BaseTower GetSelectedTower()
    {
        return availableTowers[selectedTower].GetComponent<BaseTower>();
    }

    public void SetSelectedTower(int _selectedTower)
    {
        selectedTower = _selectedTower;
    }
}