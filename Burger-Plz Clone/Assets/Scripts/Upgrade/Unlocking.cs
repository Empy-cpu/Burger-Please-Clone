using DG.Tweening;
using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Unlocking : MonoBehaviour
{
    [SerializeField] Image _fillImage;
    [SerializeField] int _cost;
    [SerializeField] float _spendSpeed;
    [SerializeField] float _waitTime;
    [SerializeField] GameObject _setActiveObject;


    public bool isAtArea;
    public GameObject cash;
   
    public TextMeshPro txtAtUnlock;
  

    public bool unlocked = false;
   
    public string unlockPlayerPrefsKey;
    float _spentAmount;
    float ctr;


    private void Start()
    {
        unlocked = PlayerPrefs.GetInt(unlockPlayerPrefsKey, 0) == 1;

        if (unlocked)
        {
            _setActiveObject.SetActive(true);                     
            Destroy(gameObject);
        }
        else
        {
            txtAtUnlock.text = string.Format("{0}/{1}", _spentAmount, _cost);
        }
    }





    IEnumerator  DropSlow(float delay)
    {
        yield return new WaitForSeconds(delay);

        GameObject money = LeanPool.Spawn(cash, CollectController.instance.moneyMovePos.position, Controller.instance.moneyController.moneyPrefab.transform.rotation);

        // Define jump parameters
        float jumpHeight = 0.5f;
        float jumpDuration = 0.3f;
        int jumpCount = 3; // Number of jumps

        // Use DOJump to make the money object jump
        money.transform.DOJump(transform.position, jumpHeight, jumpCount, jumpDuration)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                Destroy(money);
                txtAtUnlock.text = string.Format("{0}/{1}", _spentAmount, _cost);
                Controller.instance.moneyController.CutMoney(_cost);
                Controller.instance.uiController.UpdateMoneyTexts();
            });
    }

  
  

   
   
    private void Awake()
    {
        _fillImage.fillAmount = 0;
        _spentAmount = 0;
        ctr = _waitTime;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           isAtArea= true;

            if (_spentAmount >= _cost)
            {
                              
                _setActiveObject.SetActive(true);
                ApplyPopUpEffect();
                unlocked = true;
                PlayerPrefs.SetInt(unlockPlayerPrefsKey, 1); // Save unlocked state
                PlayerPrefs.Save();
              
                Destroy(gameObject);
            }

            var hasCash = Controller.instance.moneyController.GetCurrentAmount();

            if (hasCash == 0)
            {
                ctr = _waitTime;
                return;
            }

            if (ctr <= 0)
            {
               
                _spentAmount += _spendSpeed;
                _fillImage.fillAmount = (float)(_spentAmount / _cost);                        
                ctr = _waitTime;
                StartCoroutine(DropSlow(0.4f));
            }
            else
            {
                ctr -= Time.deltaTime;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ctr = _waitTime;
            isAtArea= false;
        }
    }

    private void ApplyPopUpEffect()
    {
        // Store the original scale of the object.
        Vector3 originalScale = _setActiveObject.transform.localScale;

        // Set the initial scale to zero.
        _setActiveObject.transform.localScale = Vector3.zero;

        // Use DOTween to animate the scale to the original scale.
        _setActiveObject.transform.DOScale(originalScale, 0.5f)
            .SetEase(Ease.OutBounce);
    }
}
