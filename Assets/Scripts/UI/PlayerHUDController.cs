using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDController : MonoBehaviour
{
    public Text playerHPText;
    
    public Text gunNameText;
    public Text gunAmmoText;
    
    public Text playerPointsText;
    
    public Text playerNumberOfDashesText;
    
    public Text waveStatusText;
    public Text skipInterwaveText;
    
    public Text exploderCooldownText;
    
    public Text decoyText;
    
    public Text equippedUtilityText;

    public Text[] grenadesText;
    InventoryWeaponData equippedWeapon;

    GameObject Player;
    PlayerMovement playerMovement;

    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        playerMovement = Player.GetComponent<PlayerMovement>();
        InitializeText();
    }

   
    void Update()
    {
        UpdatePlayerHPText();
        UpdatePlayerPointsText();
        UpdatePlayerWeaponText();
        UpdatePlayerNumberOfDashesText();
        UpdateWaveStatusText();
        UpdateExploderCooldownText();
        UpdateDecoyText();
        UpdateEquippedUtilityText();
        UpdateGrenadesText();
    }

    void InitializeText()
    {
        if(ProgressionManager.instance.GetUpgradeLevel(0) == 0)
        {
            playerNumberOfDashesText.gameObject.SetActive(false);
        }
        if(ProgressionManager.instance.GetUpgradeLevel(3) == 0)
        {
            exploderCooldownText.gameObject.SetActive(false);
        }
        if (InventoryManager.instance.Decoy.itemName != "Decoy")
        {
            decoyText.gameObject.SetActive(false);
        }
        if (InventoryManager.instance.equippedUtility.itemName == "")
        {
            equippedUtilityText.gameObject.SetActive(false);
        }
        foreach (Text text in grenadesText)
        {
            text.gameObject.SetActive(false);
        }
        for (int i = 0; i < InventoryManager.instance.quantityOfGrenadeTypes; i++)
        {
            grenadesText[i].gameObject.SetActive(true);
        }
    }
    void UpdatePlayerHPText()
    {
        playerHPText.text = PlayerManager.instance.GetHP().ToString();
    }

    void UpdatePlayerPointsText()
    {
        playerPointsText.text = ProgressionManager.instance.GetPoints()+ " P";
    }

    void UpdatePlayerNumberOfDashesText()
    {
        playerNumberOfDashesText.text = "Number Of Dashes: " + playerMovement.GetNumberOfDashes();
    }

    void UpdatePlayerWeaponText()
    {
        equippedWeapon = InventoryManager.instance.equippedWeapon;
        if (!equippedWeapon.isReloading)
        {
            gunAmmoText.text = equippedWeapon.ammoLeftInMagazine + "/" + equippedWeapon.maxAmountOfBulletsInMagazine;
        }
        else 
        {
            gunAmmoText.text = "Reloading...";
        }
        gunNameText.text = InventoryManager.instance.equippedWeapon.itemName;
    }

    void UpdateWaveStatusText()
    {
        if (WavesManager.instance.IsInterwaveTime())
        {
            waveStatusText.text = "Prepare for next wave in: " + WavesManager.instance.GetTimeLeftToNextWave().ToString("F1") + " seconds";
            skipInterwaveText.gameObject.SetActive(true);
        }
        else
        {
            skipInterwaveText.gameObject.SetActive(false);
            int enemiesLeft = EnemiesManager.instance.GetNumberOfEnemiesLeft();
            string enemiesStatus;
            if (enemiesLeft > 1)
            {
                enemiesStatus = EnemiesManager.instance.GetNumberOfEnemiesLeft() + " enemies left";
            }
            else
            {
                enemiesStatus = "1 enemy left";
            }
            waveStatusText.text = "Wave " + WavesManager.instance.GetActualWave() + "\n " + enemiesStatus;
        }
    }

    public void UpdateExploderCooldownText()
    {
        string exploderCooldown = "Error ";
        if (Player.GetComponent<Exploder>() != null)
        {
            exploderCooldown = Player.GetComponent<Exploder>().GetExploderCooldownLeft().ToString("F1");
        }
        if (exploderCooldown == "0,0")
        {
            exploderCooldown = "Ready";
        }
        else { exploderCooldown += "s to recharge"; }
        exploderCooldownText.text = "Exploder: " + exploderCooldown;
    }

    public void UpdateDecoyText()
    {
        decoyText.text = "Number of Decoys: " + InventoryManager.instance.Decoy.quantity;
    }

    public void UpdateEquippedUtilityText()
    {
        if (InventoryManager.instance.equippedUtility != null)
        {
            equippedUtilityText.text = InventoryManager.instance.equippedUtility.itemName;
        }
    }

    public void UpdateGrenadesText()
    {
        for(int i = 0; i < InventoryManager.instance.quantityOfGrenadeTypes; i++)
        {
            grenadesText[i].text = "Number of " + InventoryManager.instance.Grenades[i].itemName + "s: " + InventoryManager.instance.Grenades[i].quantity;
        }
    }
    public void UpdateTextVisibility()
    {
        if (ProgressionManager.instance.GetUpgradeLevel(0) == 0)
        {
            playerNumberOfDashesText.gameObject.SetActive(false);
        }
        else
        {
            playerNumberOfDashesText.gameObject.SetActive(true);
        }
        if(ProgressionManager.instance.GetUpgradeLevel(3) == 0)
        {
            exploderCooldownText.gameObject.SetActive(false);
        }
        else
        {
            exploderCooldownText.gameObject.SetActive(true);
        }
        if(InventoryManager.instance.Decoy.itemName == "Decoy")
        {
            decoyText.gameObject.SetActive(true);
        }
        else
        {
            decoyText.gameObject.SetActive(false);
        }
        if(InventoryManager.instance.equippedUtility != null && InventoryManager.instance.equippedUtility.itemName != "")
        {
            equippedUtilityText.gameObject.SetActive(true);
        }
        else
        {
            equippedUtilityText.gameObject.SetActive(false);
        }
        foreach(Text text in grenadesText)
        {
            text.gameObject.SetActive(false);
        }
        for(int i = 0; i < InventoryManager.instance.quantityOfGrenadeTypes; i++)
        {
            grenadesText[i].gameObject.SetActive(true);
        }
    }

    public void UpdateUtilityText()
    {
        UpdateDecoyText();
        UpdateGrenadesText();
        UpdateTextVisibility();
    }
}
