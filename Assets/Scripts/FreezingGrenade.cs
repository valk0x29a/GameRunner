using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezingGrenade : MonoBehaviour
{
    public float freezingRadius;

    private void OnCollisionEnter()
    {
        foreach (GameObject Enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            float distance = Vector3.Distance(transform.position, Enemy.transform.position);
            if (distance < freezingRadius) { Enemy.GetComponent<EnemyMovement>().Freeze(); }
        }
        Destroy(this.gameObject);
    }
}

