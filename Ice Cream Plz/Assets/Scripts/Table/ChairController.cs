using DG.Tweening;
using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairController : MonoBehaviour
{
    public Stack<Transform> chairCones = new Stack<Transform>();
    public  Transform trash;
    CustomerController customer;
    CollectController collectController;
    public Transform conePlacer;
    private int maxConesOnCounter = 3;

    public bool tableHasTrash;

    public GameObject moneyPrefab;
    public Transform moneyInstancePos;

    public int moneyTransferCount;

    public GameObject[] moneys;
    public List<GameObject> destroyableMoney = new List<GameObject>();

  
    private void Start()
    {
       // customer=FindAnyObjectByType<CustomerController>();
        collectController= FindAnyObjectByType<CollectController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Customer"))
        {
            if (customer != null) // Check if customer is not null
            {
                // Check if the player is holding cones
                bool isHoldingCones = customer._customerCones.Count > 0;

                if (isHoldingCones)
                {
                    // Calculate how many cones can be placed on the table
                    int availableSlots = Mathf.Min(maxConesOnCounter - conePlacer.childCount, customer._customerCones.Count);

                    for (int i = 0; i < availableSlots; i++)
                    {
                        Transform cone = customer._customerCones.Pop();
                        Vector3 targetPosition = conePlacer.position + Vector3.up * conePlacer.childCount * 0.50f;

                        // Make the cone jump towards the counter's conePlacer
                        cone.DOJump(targetPosition, 1.0f, 1, 0.5f);
                        chairCones.Push(cone);
                        cone.SetParent(conePlacer);
                    }
                }
            }
        }

        if (other.CompareTag("Player"))
        {
            if (trash != null) // Check if trash is not null
            {
                if (!collectController._isCarryingCone)
                {
                    Transform trashTransform = trash;
                    collectController.collectedStuff.Push(trashTransform);

                    // Additional actions if needed
                    Vector3 targetPosition = Vector3.zero + Vector3.up * collectController.collectedStuff.Count * 0.25f;
                    trashTransform.transform.DOLocalJump(targetPosition, 1.0f, 0, 0.5f);

                    trashTransform.transform.localRotation = Quaternion.identity;
                    trashTransform.SetParent(collectController.HolderParent);
                    collectController._isCarryingTrash = true;
                }
                else
                {
                    print("Player already has his hands full");
                }
            }
            else
            {
                Debug.Log("Trash not here");
            }
        }
        else
        {
           // Debug.Log("Touched");
            return;
        }




    }

   

    public void SetCustomer(CustomerController customerController)
    {
        customer = customerController;
    }

    public bool IsAvailable()
    {        
        return customer == null && trash==null;
      
    }

    public void InstanceAndMoveMoney1Customer()
    {
        int customerMoney = 5;
        if (moneyTransferCount == moneys.Length) return;
        for(int i=1;i<customerMoney;i++) {

            GameObject money = LeanPool.Spawn(moneyPrefab, moneyInstancePos.position, moneyPrefab.transform.rotation);
            destroyableMoney.Add(money);
            money.transform.DOMove(moneys[moneyTransferCount].transform.position, 0.4f).SetEase(Ease.InExpo).OnComplete(() =>
            {
                moneys[moneyTransferCount].SetActive(true);
                moneyTransferCount++;
                LeanPool.Despawn(money);
            });
        }
       
        
    }

    public void CollectMoney()
    {
        if (moneyTransferCount > 0)
        {
            int a = moneyTransferCount;
            Controller.instance.moneyController.AddMoney(a * Controller.instance.moneyController.perBundelMoney);
            Controller.instance.uiController.UpdateMoneyTexts();
            moneyTransferCount = 0;
           // Controller.instance.gameController.PlayHaptic();
            for (int i = 0; i < a; i++)
            {
                GameObject m = LeanPool.Spawn(Controller.instance.moneyController.moneyPrefab, moneys[i].transform.position, moneys[i].transform.rotation);
                m.transform.DOMove(CollectController.instance.moneyMovePos.position, 0.7f).OnComplete(() => { LeanPool.Despawn(m); });
                m.transform.DOScale(Vector3.zero, 0.5f);

                moneys[i].SetActive(false);
            }
        }
    }

}
