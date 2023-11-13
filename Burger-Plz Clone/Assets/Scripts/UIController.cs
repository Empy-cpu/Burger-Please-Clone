using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public TextMeshProUGUI[] moneyTexts;

    public void Start()
    {
        UpdateMoneyTexts();
    }

    public void UpdateMoneyTexts()
    {
        float money = Controller.instance.moneyController.GetCurrentAmount();
        for (int i = 0; i < moneyTexts.Length; i++)
        {
            if (money >= 1000)
            {
                float value = money / 1000;
                value = Mathf.Round(value * 100.0f) * 0.01f;
                moneyTexts[i].text = value.ToString() + "K";
            }
            else
            {
                moneyTexts[i].text = money.ToString();
            }
        }
    }
}
