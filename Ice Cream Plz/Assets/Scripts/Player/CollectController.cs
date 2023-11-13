using DG.Tweening;
using DitzelGames.FastIK;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectController : MonoBehaviour {

    public static CollectController instance;

    public Transform moneyMovePos;
    public Stack<Transform> collectedStuff = new Stack<Transform>();
    public Transform HolderParent;

    
    public bool _isCarryingTrash, _isCarryingCone,_isCarryingConeBox;

    public int maxCarryCones = 4;

    [SerializeField] private FastIKFabric ikBone1;
    [SerializeField] private FastIKFabric ikBone2;


    private void Awake()
    {
        instance = this;

    }

    //public void Start()
    //{
    //    ikBone1 = GameObject.Find("LeftHandIndex1").GetComponent<FastIKFabric>();
    //    ikBone2 = GameObject.Find("RightHandIndex1").GetComponent<FastIKFabric>();
    //}
    private void OnTriggerEnter(Collider obj)
    {

        if (obj.CompareTag("Money"))
        {
            ChairController chairController = obj.GetComponentInParent<ChairController>();
            Cashier cashierController = obj.GetComponentInParent<Cashier>();

            if (chairController != null)
            {
                if (chairController.destroyableMoney != null)
                {
                    chairController.CollectMoney();

                    foreach (var moneyObject in chairController.destroyableMoney)
                    {
                        if (moneyObject != null && moneyObject.activeInHierarchy)
                        {
                            moneyObject.SetActive(false);
                        }
                    }

                    chairController.destroyableMoney.Clear();
                }
            }

            if (cashierController != null)
            {
                if (cashierController.destroyableMoney != null)
                {
                    cashierController.CollectMoney();

                    foreach (var moneyObject in cashierController.destroyableMoney)
                    {
                        if (moneyObject != null && moneyObject.activeInHierarchy)
                        {
                            moneyObject.SetActive(false);
                        }
                    }

                    cashierController.destroyableMoney.Clear();
                }
            }
        }
    }

    public void Update()
    {
        if(_isCarryingCone || _isCarryingTrash || _isCarryingConeBox)
        {
            ikBone1.enabled = true;
            ikBone2.enabled = true;
        }
        else
        {
            ikBone1.enabled = false;
            ikBone2.enabled = false;
        }
    }


}





