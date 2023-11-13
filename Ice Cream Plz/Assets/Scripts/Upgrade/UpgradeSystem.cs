using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSystem : MonoBehaviour
{  
    public event Action<int> OnUpgradeCapacity;
    public event Action<int> OnUpgradeSpeed;
    public event Action<int> OnUpgradeEarning;

    public event Action<int> OnUpgradeEmployeeCapacity;
    public event Action<int> OnUpgradeEmployeeSpeed;
    public event Action OnHireEmployee;
    

    // Method to trigger capacity upgrade event
    public void UpgradeCapacity(int amount)
    {
        OnUpgradeCapacity?.Invoke(amount);
    }

    // Method to trigger speed upgrade event
    public void UpgradeSpeed(int amount)
    {
        OnUpgradeSpeed?.Invoke(amount);
    }

    public void UpgradeEmployeeCapacity(int amount)
    {
        OnUpgradeEmployeeCapacity?.Invoke(amount);
    }

    // Method to trigger speed upgrade event
    public void UpgradeEmployeeSpeed(int amount)
    {
        OnUpgradeEmployeeSpeed?.Invoke(amount);
    }


    public void UpgradeEarning(int amount)
    {
        OnUpgradeEarning?.Invoke(amount);
    }

    // Method to trigger hire employee event
    public void HireEmployee()
    {
        OnHireEmployee?.Invoke();
    }
}
