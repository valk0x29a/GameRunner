using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveGrenade : MonoBehaviour
{
    public float damage;
    public float range;
    public AnimationCurve forceToDistanceCurve;
    public float shieldPenetrationRate;
    public GameObject vfx;

    bool exploded;
    void Awake()
    {
        exploded = false;
    }

    private void OnCollisionEnter()
    {
        if (exploded) { return; }
        exploded = true;

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            
            float finalDamage = damage * forceToDistanceCurve.Evaluate(Vector3.Distance(transform.position, enemy.transform.position)/range);
            if(enemy.GetComponent<EnemyHealth>().isStrongZombie)
            {
                if (Physics.Raycast(new Ray(transform.position, (enemy.transform.position - transform.position).normalized), out RaycastHit hit))
                {
                    if (hit.collider.CompareTag("EnemyShield"))
                    {
                        finalDamage *= shieldPenetrationRate;
                    }
                }
            }
            enemy.GetComponent<EnemyHealth>().Damage((int)finalDamage, false);
        }
        GameObject temp = Instantiate(vfx, transform.position, Quaternion.identity);
        Destroy(temp, 0.5f);
        
        Destroy(this.gameObject);
    }
}
