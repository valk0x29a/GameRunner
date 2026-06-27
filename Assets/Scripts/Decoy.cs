using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decoy : MonoBehaviour
{
    public int maxNumberOfEnemies;
    int numberOfEnemies;

    int[] ids;

    private void Awake()
    {
        ids = new int[maxNumberOfEnemies];
    }
    void Start()
    {
        this.gameObject.name = "Decoy " + GetEntityId();
        numberOfEnemies = 0;
        Destroy(this.gameObject, 20f);
    }

    public void RegisterEnemy(int id)
    {
        Debug.Log("Enemy registered" + id);
        ids[numberOfEnemies] = id;
        numberOfEnemies++;
    }

    public void UnregisterEnemy(int id)
    {
        Debug.Log("Enemy unregistered" + id);
        int index = indexOfID(id);
        Debug.Log(index);
        if(index == -1) { Debug.Log(gameObject.name + " has an error with id " + id); return; }
        if(index+1 == numberOfEnemies) { ids[index] = -1; }
        else
        {
            for(int i = index; i <= numberOfEnemies-2; i++)
            {
                ids[i] = ids[i + 1];
            }
            ids[numberOfEnemies-1] = -1;
        }

        numberOfEnemies--;
    }

    public bool IsAvailable(int id)
    {
        return numberOfEnemies < maxNumberOfEnemies || indexOfID(id) != -1;
    }

    public int indexOfID(int idToFind)
    {
        int i = 0;
        foreach(int id in ids)
        {
            if (id == idToFind) return i;
            i++;
        }
        return -1;
    }
}
