using System.Collections;
using System.Collections.Generic;
using System.Drawing.Imaging;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public bool isDead;
    public int maxHPOnStart;

    int maxHP;
    int hp;
    GameObject player;

    public bool autoHealthPickup = false;

    PlayerVFX playerVFX;
    
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one GameManager in the scene");
        }
        else
        {
            instance = this;
            InitalizeManager();
        }
    }

    void InitalizeManager()
    {
        maxHP = maxHPOnStart;
        hp = maxHP;
        player = GameObject.Find("Player");
        playerVFX = player.GetComponent<PlayerVFX>();
    }

    public void Damage(int damage)
    {
        hp -= damage;
        if(hp <= 0)
        {
            Die();
        }
    }

    public void AddHealth(int health)
    {
        if(hp < maxHP) { playerVFX.PlayVFX(0); }
        
        hp += health;
        if(hp > maxHP)
        {
            hp = maxHP;
        }
    }

    public void SetMaxHealth(int maxHealth)
    {
        maxHP = maxHealth;
    }

    void Die()
    {
        Destroy(player);
        isDead = true;
    }

    public int GetHP() => hp; 
    public int GetMaxHP() => maxHP;
    public bool HasPlayerMaximumHP() => hp == maxHP; 
    public GameObject GetPlayer() => player; 

}
