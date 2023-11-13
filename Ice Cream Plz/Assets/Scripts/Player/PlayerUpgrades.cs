using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUpgrades : MonoBehaviour
{
    [SerializeField] GameObject upgradePanel;
    [SerializeField] Button crossBtn;

    [Space]
    [Header("Capacity")]  
    [SerializeField] Button capacityUpgradeButton;
    public Image[] capacityProgressBars;
    public TextMeshProUGUI capacityPriceText;
    public int[] capacityPrices;
    public int[] capacities;


    [Space]
    [Header("Speed")]
    [SerializeField] Button speedUpgradeButton;
    public Image[] speedProgressBars;
    public TextMeshProUGUI speedPriceText;
    public int[] speedPrices;
    public int[] speeds;

    [Space]
    [Header("PriceHike")]
    [SerializeField] Button increasePriceButton;
    public Image[] priceProgressBars;
    public TextMeshProUGUI pricePriceText;
    public int[] pricePrices;
    public int[] prices;

    

    public UpgradeSystem upgradeSystem;

    private void Start()
    {
        // Add an event listener to the cross button
        crossBtn.onClick.AddListener(ToggleUpgradePanel);

        // Subscribe to the upgrade events
        capacityUpgradeButton.onClick.AddListener(UpgradeCapacity);
        UpdateCapacityPriceTexts();

        speedUpgradeButton.onClick.AddListener(UpgradeSpeed);
        UpdateSpeedPriceTexts();

        increasePriceButton.onClick.AddListener(UpgradeEarning);
        UpdateEarningPriceTexts();

        upgradeSystem =FindAnyObjectByType<UpgradeSystem>();

       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowUpgradePanel(); // Show the panel when the player enters the trigger
        }
    }

    private void ShowUpgradePanel()
    {
        if (!upgradePanel.activeSelf)
        {
            upgradePanel.SetActive(true); // Show the panel
        }
    }

    private void ToggleUpgradePanel()
    {
        upgradePanel.SetActive(!upgradePanel.activeSelf); // Toggle the panel when the cross button is pressed
    }





  

    private void UpgradeCapacity()
    {

        int currentCapacity = PlayerPrefs.GetInt("PlayerCapacity");

        for (int i = 0; i < capacities.Length - 1; i++)
        {
            int nextCapacity = capacities[i + 1];

            if (currentCapacity < capacities[i] || !PlayerPrefs.HasKey("PlayerCapacity"))
            {
                if (Controller.instance.moneyController.GetCurrentAmount() < capacityPrices[i]) return;
                PlayerPrefs.SetInt("PlayerCapacity", capacities[i]);
                Controller.instance.moneyController.CutMoney(capacityPrices[i]);
                upgradeSystem.UpgradeCapacity(capacities[i]);
                UpdateCapacityPriceTexts();
                return;
            }
            else if (currentCapacity >= capacities[i] && currentCapacity < nextCapacity)
            {
                if (Controller.instance.moneyController.GetCurrentAmount() < capacityPrices[i + 1]) return;
                PlayerPrefs.SetInt("PlayerCapacity", nextCapacity);
                Controller.instance.moneyController.CutMoney(capacityPrices[i + 1]);
                upgradeSystem.UpgradeCapacity(capacities[i]);
                UpdateCapacityPriceTexts();
                return;
            }
        }


    }

    public void UpdateCapacityPriceTexts()
    {
        
      
        int currentCapacity = PlayerPrefs.GetInt("PlayerCapacity");
        int maxCapacityIndex = capacities.Length - 1;

       
        for (int i = 0; i <= maxCapacityIndex; i++)
        {
            if (currentCapacity < capacities[i] || !PlayerPrefs.HasKey("PlayerCapacity"))
            {
                capacityPriceText.text = capacityPrices[i].ToString();
              
                return;
            }
            else if (currentCapacity == capacities[i] && i < maxCapacityIndex)
            {
                capacityPriceText.text = capacityPrices[i + 1].ToString();
                for (int j = 0; j <= i; j++)
                {
                    capacityProgressBars[j].color = Color.red;
                   
                }
              // return;
            }
        }

        // If the loop completes without returning, it means the capacity is equal to the last capacity.
        capacityPriceText.text = "Max";
        for (int i = 0; i <= maxCapacityIndex; i++)
        {
            capacityProgressBars[i].color = Color.red;
        }
        capacityUpgradeButton.interactable = false;

    }

    private void UpgradeSpeed()
    {
        int currentSpeed = PlayerPrefs.GetInt("PlayerSpeed");

        for (int i = 0; i < speeds.Length - 1; i++)
        {
            int nextSpeed = speeds[i + 1];

            if (currentSpeed < speeds[i] || !PlayerPrefs.HasKey("PlayerSpeed"))
            {
                if (Controller.instance.moneyController.GetCurrentAmount() < speedPrices[i]) return;
                PlayerPrefs.SetInt("PlayerSpeed", speeds[i]);
                Controller.instance.moneyController.CutMoney(speedPrices[i]);
                upgradeSystem.UpgradeSpeed(speeds[i]);
                UpdateSpeedPriceTexts();
                return;
            }
            else if (currentSpeed >= speeds[i] && currentSpeed < nextSpeed)
            {
                if (Controller.instance.moneyController.GetCurrentAmount() < speedPrices[i + 1]) return;
                PlayerPrefs.SetInt("PlayerSpeed", nextSpeed);
                Controller.instance.moneyController.CutMoney(capacityPrices[i + 1]);
                upgradeSystem.UpgradeSpeed(speeds[i]);
                UpdateSpeedPriceTexts();
                return;
            }
        }
    }

    public void UpdateSpeedPriceTexts()
    {
        // Controller.instance.uiController.UpdatePlayerMaxText();

        int currentSpeed = PlayerPrefs.GetInt("PlayerSpeed");
        int maxSpeedIndex = speeds.Length - 1;


        for (int i = 0; i <= maxSpeedIndex; i++)
        {
            if (currentSpeed < speeds[i] || !PlayerPrefs.HasKey("PlayerSpeed"))
            {
                speedPriceText.text = speedPrices[i].ToString();
               
                return;
            }
            else if (currentSpeed == speeds[i] && i < maxSpeedIndex)
            {
                speedPriceText.text = speedPrices[i + 1].ToString();
                for (int j = 0; j <= i; j++)
                {
                    speedProgressBars[j].color = Color.red;
                   
                }
                // return;
            }
        }

        // If the loop completes without returning, it means the capacity is equal to the last capacity.
        speedPriceText.text = "Max";
        for (int i = 0; i <= maxSpeedIndex; i++)
        {
            speedProgressBars[i].color = Color.red;
        }
        speedUpgradeButton.interactable = false;

    }


    private void UpgradeEarning()
    {
        int currentPrice = PlayerPrefs.GetInt("PlayerPrice");

        for (int i = 0; i < prices.Length - 1; i++)
        {
            int nextPrice = prices[i + 1];

            if (currentPrice < prices[i] || !PlayerPrefs.HasKey("PlayerPrice"))
            {
                if (Controller.instance.moneyController.GetCurrentAmount() < pricePrices[i]) return;
                PlayerPrefs.SetInt("PlayerPrice", prices[i]);
                Controller.instance.moneyController.CutMoney(pricePrices[i]);
                upgradeSystem.UpgradeEarning(prices[i]);
                UpdateEarningPriceTexts();
                return;
            }
            else if (currentPrice >= prices[i] && currentPrice < nextPrice)
            {
                if (Controller.instance.moneyController.GetCurrentAmount() < pricePrices[i + 1]) return;
                PlayerPrefs.SetInt("PlayerPrice", nextPrice);
                Controller.instance.moneyController.CutMoney(pricePrices[i + 1]);
                upgradeSystem.UpgradeEarning(prices[i]);
                UpdateEarningPriceTexts();
                return;
            }
        }
    }

    public void UpdateEarningPriceTexts()
    {
        // Controller.instance.uiController.UpdatePlayerMaxText();

        int currentPrice = PlayerPrefs.GetInt("PlayerPrice");
        int maxPriceIndex = prices.Length - 1;


        for (int i = 0; i <= maxPriceIndex; i++)
        {
            if (currentPrice < prices[i] || !PlayerPrefs.HasKey("PlayerPrice"))
            {
                pricePriceText.text = pricePrices[i].ToString();

                return;
            }
            else if (currentPrice == prices[i] && i < maxPriceIndex)
            {
                pricePriceText.text = pricePrices[i + 1].ToString();
                for (int j = 0; j <= i; j++)
                {
                    priceProgressBars[j].color = Color.red;

                }
                // return;
            }
        }

        // If the loop completes without returning, it means the capacity is equal to the last capacity.
        pricePriceText.text = "Max";
        for (int i = 0; i <= maxPriceIndex; i++)
        {
            priceProgressBars[i].color = Color.red;
        }
        increasePriceButton.interactable = false;

    }

}
