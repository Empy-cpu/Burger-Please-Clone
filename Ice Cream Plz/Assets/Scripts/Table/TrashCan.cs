using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MonoBehaviour
{
    CollectController cc;
    Vector3 dropPos;
    [SerializeField] Transform dropPlace;
    private bool isDroppingTrash = false;

    private void Start()
    {
        cc = FindAnyObjectByType<CollectController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        dropPos= dropPlace.transform.position;
        if (other.CompareTag("Player"))
        {
            if (cc._isCarryingTrash)
            {
                cc._isCarryingTrash = false;
                StartCoroutine(DropSlow(.10f));
            }
            else
            {
                Debug.Log("no trash");
            }
           
        }
    }

    IEnumerator DropSlow(float delay)
    {
        isDroppingTrash = true; // Set the flag to true when the coroutine starts
        yield return new WaitForSeconds(delay);

        while (isDroppingTrash && CollectController.instance.collectedStuff.Count > 0) // Use the flag and check stack count as the loop condition
        {
            Transform newItem = CollectController.instance.collectedStuff.Pop();

            newItem.DOJump(dropPos, 2, 1, .2f).OnComplete(() => newItem.DOPunchScale(new Vector3(.2f, .2f, .2f), .1f));
            Destroy(newItem.gameObject, 0.3f);
        }

        // Reset the flag when the coroutine is done
        isDroppingTrash = false;
        yield return null;
    }

}

