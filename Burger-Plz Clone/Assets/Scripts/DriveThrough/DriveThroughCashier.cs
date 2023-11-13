using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveThroughCashier : MonoBehaviour
{

    public static DriveThroughCashier instance;
    public Stack<Transform> driveCashierCones = new Stack<Transform>();

    CollectController cc;
    public Transform conePlacer;
    private int maxConesOnCounter = 6;


    public GameObject cashierLoc;

    public bool unlocked = false;

    private void Start()
    {
        instance = this;
        cc = FindAnyObjectByType<CollectController>();
        unlocked = true;


    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Check if the player is holding cones
            bool isHoldingCones = cc.collectedStuff.Count > 0;

            if (isHoldingCones && cc._isCarryingConeBox == true && cc._isCarryingCone==false && cc._isCarryingTrash==false)
            {
                // Calculate how many cones can be placed on the counter
                int availableSlots = Mathf.Min(maxConesOnCounter - conePlacer.childCount, cc.collectedStuff.Count);

                for (int i = 0; i < availableSlots; i++)
                {
                    Transform cone = cc.collectedStuff.Pop();
                    Vector3 targetPosition = conePlacer.position + Vector3.up * conePlacer.childCount * 0.50f;
                    driveCashierCones.Push(cone);
                    // Make the cone jump towards the counter's conePlacer
                    cone.DOJump(targetPosition, 1.0f, 1, 0.5f);

                    cone.SetParent(conePlacer);

                    cc._isCarryingConeBox = false;
                }
            }


        }


    }

    public Transform GetCashierTrans()
    {
      
        return cashierLoc.gameObject.transform;
    }

}
