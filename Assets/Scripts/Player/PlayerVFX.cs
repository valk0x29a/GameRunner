using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVFX : MonoBehaviour
{
    public ParticleSystem healthRegen;

    public void PlayVFX(int id)
    {
        if(id == 0) { healthRegen.Play(); }
    }
}
