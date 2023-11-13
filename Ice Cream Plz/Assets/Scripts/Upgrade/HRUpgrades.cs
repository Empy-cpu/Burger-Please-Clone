using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class HRUpgrades : MonoBehaviour
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
    [Header("Employee Helpers")]
    [SerializeField] Button hireEmployeeButton;
    public Image[] employeeProgressBars;
    public TextMeshProUGUI helperPriceText;
    public int[] employeePrices;
   
    public UpgradeSystem upgradeSystem;

    private bool hasHiredEmployee = false;

    private void Start()
    {
        // Add an event listener to the cross button
        crossBtn.onClick.AddListener(ToggleUpgradePanel);

        // Subscribe to the upgrade events
        capacityUpgradeButton.onClick.AddListener(UpgradeCapacity);
        UpdateCapacityPriceTexts();

        speedUpgradeButton.onClick.AddListener(UpgradeSpeed);
        UpdateSpeedPriceTexts();

        hireEmployeeButton.onClick.AddListener(HireEmployees);
        UpdateEmployeePriceTexts();

        upgradeSystem = FindAnyObjectByType<UpgradeSystem>();


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
        if (!hasHiredEmployee)
        {
            Debug.Log("You need to hire at least 1 employee before upgrading capacity.");
            return;
        }

        int currentCapacity = PlayerPrefs.GetInt("EmployeeCapacity");


        for (int i = 0; i < capacities.Length - 1; i++)
        {

            int nextCapacity = capacities[i + 1];

            if (currentCapacity < capacities[i] || !PlayerPrefs.HasKey("EmployeeCapacity"))
            {

                if (Controller.instance.moneyController.GetCurrentAmount() < capacityPrices[i]) return;
                PlayerPrefs.SetInt("EmployeeCapacity", capacities[i]);
                Controller.instance.moneyController.CutMoney(capacityPrices[i]);
                upgradeSystem.UpgradeEmployeeCapacity(capacities[i]);               
                UpdateCapacityPriceTexts();

                return;
            }
            else if (currentCapacity >= capacities[i] && currentCapacity < nextCapacity)
            {

                if (Controller.instance.moneyController.GetCurrentAmount() < capacityPrices[i + 1])
                    return;
                PlayerPrefs.SetInt("EmployeeCapacity", nextCapacity);
                Controller.instance.moneyController.CutMoney(capacityPrices[i + 1]);
                upgradeSystem.UpgradeEmployeeCapacity(capacities[i]);
                UpdateCapacityPriceTexts();
                return;
            }


        }



    }

    public void UpdateCapacityPriceTexts()
    {
        

        int currentCapacity = PlayerPrefs.GetInt("EmployeeCapacity");
        int maxCapacityIndex = capacities.Length - 1;


        for (int i = 0; i <= maxCapacityIndex; i++)
        {
            if (currentCapacity < capacities[i] || !PlayerPrefs.HasKey("EmployeeCapacity"))
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
        if (!hasHiredEmployee)
        {
            Debug.Log("You need to hire at least 1 employee before upgrading capacity.");
            return;
        }

        int currentSpeed = PlayerPrefs.GetInt("EmployeeSpeed");

        for (int i = 0; i < speeds.Length - 1; i++)
        {

            int nextSpeed = speeds[i + 1];

            if (currentSpeed < speeds[i] || !PlayerPrefs.HasKey("EmployeeSpeedSpeed"))
            {

                if (Controller.instance.moneyController.GetCurrentAmount() < speedPrices[i]) return;
                PlayerPrefs.SetInt("EmployeeSpeed", speeds[i]);

                Controller.instance.moneyController.CutMoney(speedPrices[i]);

                upgradeSystem.UpgradeEmployeeSpeed(speeds[i]);
                UpdateSpeedPriceTexts();

                return;
            }
            else if (currentSpeed >= speeds[i] && currentSpeed < nextSpeed)
            {

                if (Controller.instance.moneyController.GetCurrentAmount() < speedPrices[i + 1])
                    return;
                PlayerPrefs.SetInt("EmployeeSpeed", nextSpeed);
                Controller.instance.moneyController.CutMoney(capacityPrices[i + 1]);

                upgradeSystem.UpgradeEmployeeSpeed(speeds[i]);
                UpdateSpeedPriceTexts();
                return;
            }


        }
    }

    public void UpdateSpeedPriceTexts()
    {
        // Controller.instance.uiController.UpdatePlayerMaxText();

        int currentSpeed = PlayerPrefs.GetInt("EmployeeSpeed");
        int maxSpeedIndex = speeds.Length - 1;


        for (int i = 0; i <= maxSpeedIndex; i++)
        {
            if (currentSpeed < speeds[i] || !PlayerPrefs.HasKey("EmployeeSpeed"))
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


    private void HireEmployees()
    {

        int currentPrice = PlayerPrefs.GetInt("Helpers");


        for (int i = 0; i < employeePrices.Length - 1; i++)
        {

            int nextPrice = employeePrices[i + 1];

            if (currentPrice < employeePrices[i] || !PlayerPrefs.HasKey("Helpers"))
            {

                if (Controller.instance.moneyController.GetCurrentAmount() < employeePrices[i]) return;
                PlayerPrefs.SetInt("Helpers", employeePrices[i]);

                Controller.instance.moneyController.CutMoney(employeePrices[i]);

                upgradeSystem.HireEmployee();
                UpdateEmployeePriceTexts();

                return;
            }
            else if (currentPrice >= employeePrices[i] && currentPrice < nextPrice)
            {

                if (Controller.instance.moneyController.GetCurrentAmount() < employeePrices[i + 1])
                    return;
                PlayerPrefs.SetInt("Helpers", nextPrice);
                Controller.instance.moneyController.CutMoney(employeePrices[i + 1]);

                upgradeSystem.HireEmployee();
                UpdateEmployeePriceTexts();
                return;
            }


        }

        hasHiredEmployee = true;
    }

    public void UpdateEmployeePriceTexts()
    {
        // Controller.instance.uiController.UpdatePlayerMaxText();

        int currentPrice = PlayerPrefs.GetInt("Helpers");
        int maxPriceIndex = employeePrices.Length - 1;


        for (int i = 0; i <= maxPriceIndex; i++)
        {
            if (currentPrice < employeePrices[i] || !PlayerPrefs.HasKey("Helpers"))
            {
                helperPriceText.text = employeePrices[i].ToString();

                return;
            }
            else if (currentPrice == employeePrices[i] && i < maxPriceIndex)
            {
                helperPriceText.text = employeePrices[i + 1].ToString();
                for (int j = 0; j <= i; j++)
                {
                    employeeProgressBars[j].color = Color.red;

                }
                // return;
            }
        }

        // If the loop completes without returning, it means the capacity is equal to the last capacity.
        helperPriceText.text = "Max";
        for (int i = 0; i <= maxPriceIndex; i++)
        {
            employeeProgressBars[i].color = Color.red;
        }
        hireEmployeeButton.interactable = false;

    }

}
