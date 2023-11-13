using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CashierEmployee : MonoBehaviour
{
    public static CashierEmployee instance;
    public bool isAtCashier;

    public SpriteRenderer Bordering;
    private  void Start()
    {
        instance = this;
        isAtCashier= false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Employee"))
        {
            isAtCashier = true;
            Bordering.DOBlendableColor(Color.green, .5f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Employee"))
        {
            isAtCashier = false;
            Bordering.DOBlendableColor(Color.white, .5f);
        }
    }
}
