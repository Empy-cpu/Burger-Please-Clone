using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgradeHandler : MonoBehaviour
{
    [SerializeField] private UpgradeSystem upgradeSystem;

    CollectController cc;
    PlayerInputCont player;
    MoneyManager moneyManager;

    private void Start()
    {
        cc=FindAnyObjectByType<CollectController>();
        player=FindAnyObjectByType<PlayerInputCont>();
        moneyManager=FindAnyObjectByType<MoneyManager>();

        InitializeUpgrades();
    }

    public void InitializeUpgrades()
    {
        // Initialize player capacity, speed, and earning based on PlayerPrefs
        HandleCapacityUpgrade(PlayerPrefs.GetInt("PlayerCapacity"));
        HandleSpeedUpgrade(PlayerPrefs.GetInt("PlayerSpeed"));
        HandleEarningUpgrade(PlayerPrefs.GetInt("PlayerPrice"));
    }
    private void OnEnable()
    {
        // Subscribe to the capacity upgrade event
        upgradeSystem.OnUpgradeCapacity += HandleCapacityUpgrade;
        upgradeSystem.OnUpgradeSpeed += HandleSpeedUpgrade;
        upgradeSystem.OnUpgradeEarning += HandleEarningUpgrade;
    }

    private void OnDisable()
    {
        // Unsubscribe from the capacity upgrade event
        upgradeSystem.OnUpgradeCapacity -= HandleCapacityUpgrade;
        upgradeSystem.OnUpgradeSpeed -= HandleSpeedUpgrade;
        upgradeSystem.OnUpgradeEarning -= HandleEarningUpgrade;
    }

    private void HandleCapacityUpgrade(int amount)
    {
        cc.maxCarryCones += amount;
        //Debug.Log("player capacity upgraded");
    }

    private void HandleSpeedUpgrade(int amount)
    {
        player._moveSpeed += amount;
        //Debug.Log("player speed upgraded");
    }

    private void HandleEarningUpgrade(int amount)
    {
        moneyManager.perBundelMoney+= amount;
        //Debug.Log("player earning upgraded");
    }
}
