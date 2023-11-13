using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EmployeeController : MonoBehaviour
{
   
     public Stack<Transform> _employee1Cones = new Stack<Transform>();

    public Transform HolderParent;

    PlayerAnimationHandler _playerAnimationHandler;

    public Transform iceCreamMachine;
    public Transform moveTarget;


    public  NavMeshAgent _agent;
    public Transform _target;


    public int moveSpeed = 0;

    public int _maxConeCount = 4;

    private bool isTakingCone = false;
    private void Awake()
    {
      
        _agent = GetComponent<NavMeshAgent>();
        _playerAnimationHandler = GetComponentInParent<PlayerAnimationHandler>();
        // Set the initial speed
        moveSpeed = 4;
        _agent.speed = moveSpeed;
    }

    private void Start()
    {
        SetTarget(iceCreamMachine);
    }


    private void Update()
    {
        if (_target == null) return;
        Move();
        
    }


    private void Move()
    {
        
        if (Vector3.Distance(transform.position, _target.position) <= 3f)
        {
           
            if (_target.CompareTag("IceMachine"))
            {

                _playerAnimationHandler.MakeCharacterIdle();
                Invoke("TakeCone", 0.9f);
            }
            if (_target.CompareTag("Counter"))
            {
                
                _playerAnimationHandler.MakeCharacterIdle();
                SetTarget(iceCreamMachine);
            }
            if (_target.CompareTag("Cashier"))
            {

                _playerAnimationHandler.MakeCharacterIdle();
                SetTarget(iceCreamMachine);
            }


        }

        else
        {
            _agent.isStopped = false;

            _agent.SetDestination(_target.position);
            _playerAnimationHandler.MakeCharacterWalk();

        }
        
        



    }

   
    public void TakeCone()
    {

        if (isTakingCone)
            return; // If already taking a cone, do nothing

        isTakingCone = true;

        _agent.isStopped = true;

        int targetIndex = 0;
        int poppedCones = 9;
        if (this != null)
        {
            int _currentCone = 0;

            // Calculate how many cones can player hold
            int availableSlots = Mathf.Min(_maxConeCount - HolderParent.childCount, IceCreamMachine.instance.conesStack.Count);

            for (int i = 0; i < availableSlots; i++)
            {
                Transform cone1 = IceCreamMachine.instance.conesStack.Pop();
                cone1.transform.SetParent(HolderParent);
                Vector3 targetPosition = Vector3.zero + Vector3.up * _employee1Cones.Count * 0.25f;
                cone1.transform.DOLocalJump(targetPosition, 1.0f, 0, 0.5f);
                cone1.transform.localRotation = Quaternion.identity;
                _employee1Cones.Push(cone1.transform);
                _currentCone++;
                poppedCones--;
            }

            if (_currentCone == availableSlots)
            {

                SetTarget(moveTarget);
            }

            targetIndex = poppedCones;
            IceCreamMachine.instance.CallMakeIceCream(0.5f, targetIndex);
        }

        isTakingCone = false;
    }

   
  

    public void SetTarget(Transform target)
    {
        _target = target;
    }

 

 

}
