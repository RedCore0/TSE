using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TowerMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI titleUI;
    [SerializeField] TextMeshProUGUI damageUI;

    [SerializeField] Animator anim;
    private bool isTowerMenuOpen = false;

    public void ToggleTowerMenu()
    {
        isTowerMenuOpen = !isTowerMenuOpen;
        anim.SetBool("TowerMenuOpen", isTowerMenuOpen);
    }
    

}
