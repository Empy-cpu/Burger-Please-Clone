using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterEmployee : MonoBehaviour
{
    public static CounterEmployee instance;
    public bool isAtCounter;

    public SpriteRenderer Bordering;
    private void Start()
    {
        instance = this;
        isAtCounter = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Employee"))
        {
            isAtCounter = true;
            Bordering.DOBlendableColor(Color.green, .5f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isAtCounter = false;
            Bordering.DOBlendableColor(Color.white, .5f);
        }
    }
}
