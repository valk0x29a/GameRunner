using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Exploder : MonoBehaviour
{
    float exploderCooldown;
    float exploderCooldownLeft;
    float explosionForce;
    float explosionRadius;
    float staminaDrain;

    bool isReady;
    void Start()
    {
        InitializeInputAsset();
    }

    void Update()
    {
        if(!isReady)
        {
            exploderCooldownLeft -= Time.deltaTime;
            if(exploderCooldownLeft <= 0)
            {
                isReady = true;
            }
            
        }
    }

    void Explode()
    {
        if (isReady)
        {
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                enemy.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRadius, 0f, ForceMode.Impulse);
                enemy.GetComponent<EnemyMovement>().DrainStamina(staminaDrain);

            }
            isReady = false;
            exploderCooldownLeft = exploderCooldown;
        }
    }
    void InitializeInputAsset()
    {
        InputSystem.actions.FindActionMap("Interactions").FindAction("Skill").performed += contex => Explode();
    }

    public void SetExploderData(ExploderData data)
    {
        exploderCooldown = data.exploderCooldown;
        explosionForce = data.explosionForce;
        explosionRadius = data.explosionRadius;
        staminaDrain = data.staminaDrain;
    }

    public float GetExploderCooldownLeft()
    {
        return exploderCooldownLeft;
    }

    void OnEnable()
    {
        isReady = true;
    }
}
