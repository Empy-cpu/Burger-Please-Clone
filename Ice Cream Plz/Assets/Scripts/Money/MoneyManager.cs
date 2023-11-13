using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class MoneyManager : MonoBehaviour
{   
    public GameObject moneyPrefab;
    public int perBundelMoney;
    int startingCash=5000;

    private void Start()
    {
        if (PlayerPrefs.HasKey("BundleCapacity"))
        {
            perBundelMoney = PlayerPrefs.GetInt("BundleCapacity");
        }
        else
        {
            perBundelMoney = 5;
        }

        PlayerPrefs.SetInt("Virtual_Currency", startingCash);
        Controller.instance.uiController.UpdateMoneyTexts();
    }
    public void AddMoney(int amount)
    {
        int current = GetCurrentAmount();
        current = amount + current;
        PlayerPrefs.SetInt("Virtual_Currency", current);
        Controller.instance.uiController.UpdateMoneyTexts();
    }

    public int GetCurrentAmount()
    {
        return PlayerPrefs.GetInt("Virtual_Currency");
      
    }

    public void CutMoney(int amount)
    {
        int current = GetCurrentAmount();      
        current = current - amount;
        PlayerPrefs.SetInt("Virtual_Currency", current);
        Controller.instance.uiController.UpdateMoneyTexts();
    }
}
