using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    public Slider enemyHealth;
    public Image slider;
    Color normalColor;
    public Color exhaustedColor;
    EnemyHealth enemy;
    EnemyMovement movement;

    void Start()
    {
        enemy = transform.parent.GetComponent<EnemyHealth>();
        movement = transform.parent.GetComponent<EnemyMovement>();
        normalColor = slider.color;
    }


    void Update()
    {
        enemyHealth.value = (float)enemy.GetHP() / enemy.maxHP;
        if(movement.IsRechargingStamina() && slider.color != exhaustedColor) { slider.color = exhaustedColor; }
        if(!movement.IsRechargingStamina() && slider.color != normalColor) { slider.color = normalColor; }
    }
}
