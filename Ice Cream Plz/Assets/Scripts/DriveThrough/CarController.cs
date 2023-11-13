using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class CarController : MonoBehaviour
{

    [SerializeField] public Stack<Transform> _carCones = new Stack<Transform>();
    public Transform HolderParent;

   
  
    NavMeshAgent _agent;
    public Transform _target;


  

    [SerializeField] int _maxConeCount = 3;
    int _wantedConeCount;

    [SerializeField] GameObject _coneCanvas;
    [SerializeField] TMP_Text _wantedConeCountText;


  

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        

    }

    private void Start()
    {
        _wantedConeCount = UnityEngine.Random.Range(1, _maxConeCount + 1);
        SetWantedConeCount();
    }

    private void Update()
    {
        if (_target == null) return;

        Move();
       

    }


    private void Move()
    {
        if (Vector3.Distance(transform.position, _target.position) <= 1f)
        {
           
           if (_target.CompareTag("WaitPoint"))
            {

                WaitInQueue();
                //_playerAnimationHandler.MakeCharacterIdle();

            }
            else if (_target.CompareTag("DespawnPoint"))
            {
                Destroy(gameObject);
            }
            else if (_target.CompareTag("DriveThrough"))
            {
                //_playerAnimationHandler.MakeCharacterIdle();
              
                ShowConeCanvas();
                Invoke("TakeCone", 0.9f);
            }


        }

        else
        {
            _agent.isStopped = false;

            _agent.SetDestination(_target.position);
           // _playerAnimationHandler.MakeCharacterWalk();

        }
       



    }

    private void WaitInQueue()
    {

        _agent.isStopped = true;
      
        //transform.position = _target.position;
        // Set the car's rotation to look straight ahead
       // transform.rotation = Quaternion.LookRotation(Vector3.forward);
       

        DriveThroughManager.instance.GetCone();

    }
    public void TakeCone()
    {
        
        _agent.isStopped = true;
        int _currentCone = 0;

        if (this != null && _wantedConeCount <= DriveThroughCashier.instance.driveCashierCones.Count && DriveThroCashierEmployee.instance.isAtCashier)
        {
          
            // Calculate how many cones can player hold
            int availableSlots = Mathf.Min(_wantedConeCount - HolderParent.childCount, DriveThroughCashier.instance.driveCashierCones.Count);

            for (int i = 0; i < availableSlots; i++)
            {
                Transform cone1 = DriveThroughCashier.instance.driveCashierCones.Pop();
                cone1.transform.SetParent(HolderParent);
                Vector3 targetPosition = Vector3.zero + Vector3.up * _carCones.Count * 0.25f;
                cone1.transform.DOLocalJump(targetPosition, 1.0f, 0, 0.5f);
                cone1.transform.localRotation = Quaternion.identity;
                _carCones.Push(cone1.transform);
                _currentCone++;
               
            }

        }
        if (_currentCone == _wantedConeCount)
        {

            //Cashier.instance.InstanceAndMoveMoney1Customer();
          
            HideConeCanvas();
             DriveThroughManager.instance.RemoveCar();
            SetTarget(DriveThroughManager.instance.GetDeSpawnPoint());
        }

    }


     
    public void SetTarget(Transform target)
    {
        _target = target;
    }

    public void SetWantedConeCount()
    {
        _wantedConeCountText.text = _wantedConeCount.ToString();
    }

   
    public void ShowConeCanvas()
    {
        _coneCanvas.SetActive(true);
    }

    public void HideConeCanvas()
    {
        Debug.Log("Hiding Cone Canvas");
        _coneCanvas.GetComponent<Canvas>().enabled = false;

    }
}
