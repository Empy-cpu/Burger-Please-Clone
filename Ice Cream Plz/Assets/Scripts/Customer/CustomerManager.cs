using System.Collections;
using System.Collections.Generic;
using Lean.Common;
using Sirenix.OdinInspector;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager instance;
    [SerializeField] List<CustomerController> _customerPrefabs = new List<CustomerController>();
    [SerializeField] List<CustomerController> _customersQueue = new List<CustomerController>();
    [SerializeField] List<Transform> _waitPoints = new List<Transform>();
    [SerializeField] Transform _spawnPoint;
     float _spawnTime = 1f;

    int _maxCustomers, _currentQueue = 0;
    float _ctr = 0f;
    int lastCustomerIndex = -1;
    bool canSpawn = true;

   
    private void Awake()
    {
        instance = this;
        _maxCustomers =_waitPoints.Count;
      
    }
  

    private void Update()
    {
        if (!GameManager.instance._isGameStarted) return;    
        if (!canSpawn)
        {
            _ctr = 0;
            return;
        }

        _ctr += Time.deltaTime;
        if (_ctr >= _spawnTime)
        {
            _ctr = 0f;
       
           SpawnCustomer();
        
        }
    }

   // [Button(ButtonSizes.Small)]
    private void SpawnCustomer()
    {
        if (_customersQueue.Count >= _maxCustomers) return;

        var randIndex = Random.Range(0, _customerPrefabs.Count);
        while (randIndex == lastCustomerIndex && lastCustomerIndex != -1)
        {
            randIndex = Random.Range(0, _customerPrefabs.Count);
        }
        lastCustomerIndex = randIndex;
        CustomerController customer = Instantiate(_customerPrefabs[randIndex], _spawnPoint.position, Quaternion.identity);
       
        customer.transform.SetParent(transform);
        _customersQueue.Add(customer);
        customer.SetTarget(_waitPoints[_currentQueue]);
        _currentQueue++;
      
        

    }

    public void RemoveCustomer()
    {
        if (_customersQueue.Count > 0)
        {
          
            _customersQueue.RemoveAt(0);
            _currentQueue--;
            for (int i = 0; i < _customersQueue.Count; i++)
            {
                _customersQueue[i].SetTarget(_waitPoints[i]);
            }
            canSpawn = true;
        }

    }


    public void GetCone()
    {
        if (_customersQueue.Count > 0)
        {
            CustomerController customer = _customersQueue[0];
            customer.SetTarget(Cashier.instance.GetCashierTrans());
           
        }
    }
   
    public Transform GetSpawnPoint()
    {
        return _spawnPoint;
    }

}
