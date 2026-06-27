using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float attackDistance;
    public int fieldOfView;
    public int enemyDamage;


    public float attackCooldown;
    float attackCooldownLeft;

    public float attackDelay;

    public float attackStaminaDrain;

    public float outOfViewRotationSpeed;

    bool isCooldownActive;

    float timer;

    GameObject player;
    EnemyMovement movement;
    void Start()
    {
        player = GameObject.Find("Player");
        movement = GetComponent<EnemyMovement>();
        attackCooldownLeft = attackCooldown;
        timer = attackDelay;
    }

   
    void Update()
    {
        if (isCooldownActive)
        {
            attackCooldownLeft -= Time.deltaTime;
            if(attackCooldownLeft <= 0)
            {
                isCooldownActive = false;
                attackCooldownLeft = attackCooldown;
            }
        }
        if (!PlayerManager.instance.isDead)
        {
            float distanceFromPlayer = Vector3.Distance(player.transform.position, transform.position);
            if (distanceFromPlayer < attackDistance)
            {
                Vector3 forward = transform.TransformDirection(Vector3.forward);
                Vector3 toPlayer = player.transform.position - transform.position;

                float fov = 1 - fieldOfView / 180;
                if (Vector3.Dot(forward, toPlayer) >= fov)
                {
                    if (timer > 0) { timer -= Time.deltaTime; }
                    if (!isCooldownActive && !movement.IsRechargingStamina() && !movement.IsDisabled() && timer <= 0)
                    {
                        PlayerManager.instance.Damage(enemyDamage);
                        isCooldownActive = true;
                        movement.DrainStamina(attackStaminaDrain);
                    }
                }
                else if (timer <= 0)
                { 
                     timer = attackDelay;
                }
            }
            else if (timer <= 0)
            {
                timer = attackDelay;
            }
        }
    }
}
