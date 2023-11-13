using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCreamMachine : MonoBehaviour
{
    public static IceCreamMachine instance;
    [SerializeField] private Transform[] conePlace=new Transform[9];
    public Stack<Transform> conesStack = new Stack<Transform>();
    [SerializeField] private GameObject iceCreamCone;
   
    [SerializeField] private GameObject coneHolder;
    // Start is called before the first frame update
    CollectController cc;
    public Transform HolderParent;

    public bool unlocked = false;
    void Start()
    {
        instance = this;
        unlocked= true;
        cc=FindAnyObjectByType<CollectController>();
       
        for(int i=0;i<conePlace.Length;i++)
        {
            conePlace[i] = transform.GetChild(i);
            
        }

        Invoke("callAftersomeTime", 1f);
                   
    }

    void callAftersomeTime()
    {
        CallMakeIceCream(0.1f, 0);
    }
  
    public void CallMakeIceCream(float deliveryTime,int index)
    {
        StartCoroutine(MakeIceCream(deliveryTime, index));
    }

   
    public IEnumerator MakeIceCream(float time, int index)
    {    
        
        var CC_index = index;

        while (CC_index < 9)
        {
            
                GameObject newCone = Instantiate(iceCreamCone, new Vector3(transform.position.x, -3f, transform.position.z), Quaternion.identity, coneHolder.transform);
                newCone.transform.DOJump(new Vector3(conePlace[CC_index].position.x, conePlace[CC_index].position.y, conePlace[CC_index].position.z), 1f, 1, 0.2f).SetEase(Ease.OutQuad);

                // Access the Item component of the iceCreamCone instance
                Item coneItem = newCone.GetComponent<Item>();

                CC_index++;
                conesStack.Push(coneItem.transform);

                yield return new WaitForSecondsRealtime(time);

                 
                             
        }                           
    }

    private void OnTriggerEnter(Collider other)
    {
        int poppedCones = 9;
        int targetIndex = 0;

        if (other.CompareTag("Player"))
        {
           
            if (this != null)
            {
                if(!cc._isCarryingTrash)
                {
                    // Calculate how many cones can player hold
                    int availableSlots = Mathf.Min(cc.maxCarryCones - HolderParent.childCount, this.conesStack.Count);

                    for (int i = 0; i < availableSlots; i++)
                    {
                        Transform cone1 = this.conesStack.Pop();
                        cone1.transform.SetParent(HolderParent);

                        Vector3 targetPosition = Vector3.zero + Vector3.up * cc.collectedStuff.Count * 1.25f;
                        cone1.transform.DOLocalJump(targetPosition, 1.0f, 0, 0.5f);

                        cone1.transform.localRotation = Quaternion.identity;
                        cc.collectedStuff.Push(cone1.transform);

                        poppedCones--;
                        cc._isCarryingCone = true;

                    }

                    targetIndex = poppedCones;
                    this.CallMakeIceCream(0.5f, targetIndex);
                }
               

            }


        }


    }

}

