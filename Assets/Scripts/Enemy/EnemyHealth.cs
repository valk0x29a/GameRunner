using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHP;
    int hp;

    public int pointsPerKill;
    public int bonusPointsForSpecialKill;

    public GameObject HealthPickup;

    bool wasLastHitSpecial;
    public float specialHitDamageMulitplier;
    
    public delegate void IfLastHitWasSpeical();
    public IfLastHitWasSpeical lastHitSpecial;
    
    bool isDead;
    public bool isStrongZombie;
    void Start()
    {
        hp = maxHP;
        isDead = false;
    }

    public void Damage(int damage, bool specialHit)
    {
        hp -= damage;
        int specialDamage = (int)(specialHitDamageMulitplier * damage);
        if(specialHit) hp -= specialDamage;
        if(hp <= 0 && !isDead)
        {
            isDead = true;
            Die();
        }
        wasLastHitSpecial = specialHit;
        lastHitSpecial?.Invoke();
    }

    void SpawnHealthPickup()
    {
        ProgressionManager.instance.AddPoints(bonusPointsForSpecialKill);

        if(PlayerManager.instance.autoHealthPickup && !PlayerManager.instance.HasPlayerMaximumHP())
        {
            PlayerManager.instance.AddHealth(9);
            return;
        }
        GameObject healthPickUp = Instantiate(HealthPickup);
        healthPickUp.transform.position = transform.position;    
    }

    void Die()
    {
        if (wasLastHitSpecial) { SpawnHealthPickup(); }
        GetComponent<EnemyMovement>().OnDying();
        Destroy(this.gameObject);
        ProgressionManager.instance.AddPoints(pointsPerKill);
        EnemiesManager.instance.UnregisterEnemy();
    }

    public int GetHP() { return hp; }
}
