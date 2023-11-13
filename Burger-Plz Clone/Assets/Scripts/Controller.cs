using EPOOutline.Demo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class Controller : MonoBehaviour
{
    public static Controller instance;
   
    public UIController uiController;
    public MoneyManager moneyController;
   
    private void Awake()
    {
        instance = this;
    }
}
