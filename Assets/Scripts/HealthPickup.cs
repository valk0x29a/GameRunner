using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int health;
    public float lifetime_DoesntWorkNow;
    void Start()
    {
      //  Destroy(this.gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !PlayerManager.instance.HasPlayerMaximumHP())
        {
            PlayerManager.instance.AddHealth(health);
            Destroy(this.gameObject);
        }
    }
}
