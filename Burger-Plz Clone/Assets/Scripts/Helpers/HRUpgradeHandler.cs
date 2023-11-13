using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HRUpgradeHandler : MonoBehaviour
{
    [SerializeField] private UpgradeSystem upgradeSystem;

    public Employee2Controller employee2Controller;
    public EmployeeController employeeController;
  
  

    private void Start()
    {
        employee2Controller=FindAnyObjectByType<Employee2Controller>();
        employeeController=FindAnyObjectByType<EmployeeController>();

        // Set the initial state of employee GameObjects here
        InitializeEmployeeGameObjects();
    }

    private void InitializeEmployeeGameObjects()
    {
        int numberOfEmployees = PlayerPrefs.GetInt("Helpers");
        for (int i = 0; i < numberOfEmployees; i++)
        {
            // Activate the appropriate employee GameObjects based on saved PlayerPrefs
            employee[i % employee.Length].SetActive(true);
        }
    }
    private void OnEnable()
    {
        // Subscribe to the capacity upgrade event
        upgradeSystem.OnUpgradeEmployeeCapacity += HandleCapacityUpgrade;
        upgradeSystem.OnUpgradeEmployeeSpeed += HandleSpeedUpgrade;
        upgradeSystem.OnHireEmployee += HandleHiringUpgrade;
    }

    private void OnDisable()
    {
        // Unsubscribe from the capacity upgrade event
        upgradeSystem.OnUpgradeEmployeeCapacity -= HandleCapacityUpgrade;
        upgradeSystem.OnUpgradeEmployeeSpeed -= HandleSpeedUpgrade;
        upgradeSystem.OnHireEmployee -= HandleHiringUpgrade;
    }

    private void HandleCapacityUpgrade(int amount)
    {
        print("blah1");
        employeeController._maxConeCount += amount;
        employee2Controller._maxConeCount= amount;
        Debug.Log("employees capacity upgraded");
    }

    private void HandleSpeedUpgrade(int amount)
    {
        employeeController.moveSpeed += amount;
        employee2Controller._maxConeCount = amount;
        Debug.Log("player speed upgraded");
    }

    public GameObject[] employee;
    private int numberOfEmployees = 0;
    public Transform spawnPosition;

    private void HandleHiringUpgrade()
    {
        // Check if there are employee prefabs assigned
        if (employee.Length == 0)
        {
            Debug.LogError("No employee prefabs assigned.");
            return;
        }

        // Choose which prefab to activate (e.g., based on numberOfEmployees)
        int prefabIndex = numberOfEmployees % employee.Length;
      
            // Activate an existing employee GameObject
        GameObject selectedEmployee = employee[prefabIndex];
        selectedEmployee.SetActive(true);
        
     

        // Increase the number of employees.
        numberOfEmployees++;

        Debug.Log("Hired a new employee. Total employees: " + numberOfEmployees);
    }

}
