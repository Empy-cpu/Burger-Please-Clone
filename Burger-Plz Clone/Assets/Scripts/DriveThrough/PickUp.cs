using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    CollectController cc;
    [SerializeField] private Transform coneBoxHolder;
    private void Start()
    {
        cc = FindAnyObjectByType<CollectController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {          
            if (this != null)
            {
                if (!cc._isCarryingTrash && !cc._isCarryingCone)
                {
                    if (AssembleCounter.instance.PickUpBoxes.Count > 0) // Check if the stack is not empty
                    {
                        Transform box = AssembleCounter.instance.PickUpBoxes.Pop();
                        box.transform.SetParent(coneBoxHolder);


                        Vector3 targetPosition = Vector3.zero + Vector3.up * cc.collectedStuff.Count * 1.25f;
                        box.transform.DOLocalJump(targetPosition, 1.0f, 0, 0.5f);


                        box.transform.localRotation = Quaternion.identity;
                        cc.collectedStuff.Push(box.transform);



                        cc._isCarryingConeBox = true;



                    }
                }
                else
                {
                    Debug.Log("PickUpBoxes stack is empty.");
                }
            }

          
        }
    }
}
