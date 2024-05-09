using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Vector3 offset;
    [SerializeField] private GameObject parent;
    [SerializeField] private Slider healthBar;
    float maxHealth;

    // Update is called once per frame
    private void Start()
    {
        maxHealth = parent.GetComponent<BaseEnemy>().GetHealth();
        healthBar.maxValue = maxHealth;
    }
    void Update()
    {
        healthBar.value = parent.GetComponent<BaseEnemy>().GetHealth();
        //transform.position=parent.transform.position;
        transform.rotation=cam.transform.rotation;
    }
}
