using DG.Tweening;
using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeTable : MonoBehaviour
{
    [SerializeField] Image _fillImage;
    [SerializeField] int _cost;
    [SerializeField] float _spendSpeed;
    [SerializeField] float _waitTime;
    [SerializeField] GameObject _setActiveObject;


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





    private void DropSlow()
    {
        GameObject money = LeanPool.Spawn(cash, CollectController.instance.moneyMovePos.position, Controller.instance.moneyController.moneyPrefab.transform.rotation);
        money.transform.DOMove(this.transform.position, 0.07f).SetEase(Ease.InOutQuad).OnComplete(() =>
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
                DropSlow();
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
