using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Lean.Pool;
using static Unity.Burst.Intrinsics.X86.Avx;
using Unity.VisualScripting;

public class Cashier : MonoBehaviour
{
    public static Cashier instance;
    public Stack<Transform> cashierCones = new Stack<Transform>();

    public  CollectController cc;
    public Employee2Controller emp2;


    public Transform conePlacer;
    private int maxConesOnCounter = 6;
   

    public GameObject cashierLoc;

    public bool unlocked=false;

    public GameObject moneyPrefab;
    public Transform moneyInstancePos;

    public int moneyTransferCount;

    public GameObject[] moneys;
    public List<GameObject> destroyableMoney = new List<GameObject>();

    private void Awake()
    {
        emp2 = FindAnyObjectByType<Employee2Controller>();
    }
    private void Start()
    {
        instance = this;
        cc=FindAnyObjectByType<CollectController>();
       

        unlocked = true;
       
       
    }
    private void Update()
    {
        emp2 = FindAnyObjectByType<Employee2Controller>();
    }
    private void OnTriggerEnter(Collider other)
    {
         if (other.CompareTag("Player"))
         {
            // Check if the player is holding cones
            bool isHoldingCones = cc.collectedStuff.Count > 0;

            if (isHoldingCones && cc._isCarryingConeBox==false)
            {
                // Calculate how many cones can be placed on the counter
                int availableSlots = Mathf.Min(maxConesOnCounter - conePlacer.childCount, cc.collectedStuff.Count);

                for (int i = 0; i < availableSlots; i++)
                {
                    Transform cone = cc.collectedStuff.Pop();
                    Vector3 targetPosition = conePlacer.position + Vector3.up * conePlacer.childCount * 0.50f;
                    cashierCones.Push(cone);
                    // Make the cone jump towards the counter's conePlacer
                    cone.DOJump(targetPosition, 1.0f, 1, 0.5f);

                    cone.SetParent(conePlacer);

                    cc._isCarryingCone = false;
                }
            }


         }
        if (other.CompareTag("Employee"))
        {
           
            // Check if the player is holding cones
            bool isHoldingCones = emp2._employee2Cones.Count > 0;
           
            if (isHoldingCones)
            {
                // Calculate how many cones can be placed on the counter
                int availableSlots = Mathf.Min(maxConesOnCounter - conePlacer.childCount, emp2._employee2Cones.Count);

                for (int i = 0; i < availableSlots; i++)
                {
                    Transform cone = emp2._employee2Cones.Pop();
                    Vector3 targetPosition = conePlacer.position + Vector3.up * conePlacer.childCount * 0.50f;
                    cashierCones.Push(cone);
                    // Make the cone jump towards the counter's conePlacer
                    cone.DOJump(targetPosition, 1.0f, 1, 0.5f);

                    cone.SetParent(conePlacer);

                    cc._isCarryingCone = false;
                }
            }


        }


    }

    public Transform GetCashierTrans()
    {
       
        return cashierLoc.gameObject.transform;
    }

    public void InstanceAndMoveMoney1Customer()
    {
        int customerMoney = 5;
        if (moneyTransferCount == moneys.Length) return;
        for (int i = 1; i < customerMoney; i++)
        {

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

