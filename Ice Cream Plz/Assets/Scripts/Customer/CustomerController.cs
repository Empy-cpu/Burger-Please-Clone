using DG.Tweening;

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;



public class CustomerController : MonoBehaviour
{
    [SerializeField] public Stack<Transform> _customerCones = new Stack<Transform>();
    public Transform HolderParent;

    PlayerAnimationHandler _playerAnimationHandler;
    ChairController _currentChair;
    

    NavMeshAgent _agent;  
    public Transform _target;
    
  
    bool _waitingForTable = false, _startedEating = false;

    [SerializeField] int _maxConeCount = 3;
    int _wantedConeCount;

    [SerializeField] GameObject _coneCanvas, _unavailableAnyTableCanvas;
    [SerializeField] TMP_Text _wantedConeCountText;

    float _eatingTime = 1.5f;
    float _eatingTimer = 0f;

    [SerializeField] public GameObject trashPrefab;

   
    public void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _playerAnimationHandler = GetComponentInParent<PlayerAnimationHandler>();
        _wantedConeCount = UnityEngine.Random.Range(1, _maxConeCount + 1);
        SetWantedConeCount();
    }

    private void Update()
    {
        if(_target==null) return;
        
            Move();
            Eat();
        
    }

  
    private void Move()
    {
        if (Vector3.Distance(transform.position, _target.position) <= 1f)
        {
            
            if (_target.CompareTag("WaitPoint"))
            {
              
                WaitInQueue();
                _playerAnimationHandler.MakeCharacterIdle();
               

            }
            else if (_target.CompareTag("SpawnPoint"))
            {
                Destroy(gameObject);
            }
            else if (_target.CompareTag("Cashier"))
            {
                _playerAnimationHandler.MakeCharacterIdle();
                ShowConeCanvas();
                Invoke("TakeCone",0.9f);
            }
            else if (_target.gameObject.CompareTag("Chair"))           
            {
                
                SitChair();

            }

        }

        else
        {
            _agent.isStopped = false;
           
            _agent.SetDestination(_target.position);
            _playerAnimationHandler.MakeCharacterWalk();

        }
        if (_waitingForTable)
        {
           
            var chair = ChairManager.instance.GetRandomChair();

            if (chair != null)
            {
                HideUnavailableAnyTableCanvas();

                SetTarget(chair);

                chair.GetComponent<ChairController>().SetCustomer(this);
                _currentChair = chair.GetComponent<ChairController>();
                _waitingForTable = false;
                CustomerManager.instance.RemoveCustomer();


               
            }
            else
            {

                Debug.Log("no chair available for" + this.name);
                ShowUnavailableAnyTableCanvas();

            }
        }

     

    }

    private void WaitInQueue()
    {
       
        _agent.isStopped = true;
        transform.position = _target.position;
        var lookPos = _target.position;
        lookPos.z = transform.position.z + 1f;
        lookPos.y = transform.position.y;
        transform.LookAt(lookPos);

        CustomerManager.instance.GetCone();

    }
    public void TakeCone()
    {
        if (_waitingForTable) return;
        _agent.isStopped = true;
        int _currentCone = 0;

            if (this != null && _wantedConeCount <= Cashier.instance.cashierCones.Count && CashierEmployee.instance.isAtCashier)
            {
               // Calculate how many cones can player hold
               int availableSlots = Mathf.Min(_wantedConeCount - HolderParent.childCount, Cashier.instance.cashierCones.Count);
               
                for (int i = 0; i < availableSlots; i++)
                {
                    Transform cone1 = Cashier.instance.cashierCones.Pop();
                    cone1.transform.SetParent(HolderParent);
                    Vector3 targetPosition = Vector3.zero + Vector3.up * _customerCones.Count * 0.25f;
                    cone1.transform.DOLocalJump(targetPosition, 1.0f, 0, 0.5f);
                    cone1.transform.localRotation = Quaternion.identity;
                    _customerCones.Push(cone1.transform);
                    _currentCone++;

                }
                           
            }
        if (_currentCone == _wantedConeCount)
        {
           
            Cashier.instance.InstanceAndMoveMoney1Customer();
            _waitingForTable = true;
            HideConeCanvas();

        }

    }


    public void SitChair()
    {
        _agent.isStopped = true;
        _playerAnimationHandler.MakeCharacterSit();

        transform.position = _target.position;
        var lookPos = _target.position + _target.forward;
        transform.LookAt(lookPos);   
        _currentChair.SetCustomer(this);
        
        _startedEating = true;
    }
    private void Eat()
    {
        if (_currentChair == null) return;

        if (!_startedEating) return;

        _eatingTimer += Time.deltaTime;
           
        
        if (_eatingTimer >= _eatingTime)
        {
            while (_currentChair.chairCones.Count > 0)
            {
                Transform poppedCone = _currentChair.chairCones.Pop();
                Destroy(poppedCone.gameObject);
            }

            _eatingTimer = 0f;
            _currentChair.InstanceAndMoveMoney1Customer();
            LeaveTrash();

            if (_currentChair.chairCones.Count == 0)
            {
                _currentChair = null;
                _startedEating = false;
                _agent.isStopped = false;

                SetTarget(CustomerManager.instance.GetSpawnPoint());

              
            }

           
        }


    }

    public void  LeaveTrash()
    {

        var trash1=Instantiate(trashPrefab, _currentChair.conePlacer.transform.position, Quaternion.identity);
        _currentChair.tableHasTrash= true;
        _currentChair.trash=trash1.transform;
        
       
      
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    public void SetWantedConeCount()
    {
        _wantedConeCountText.text = _wantedConeCount.ToString();
    }

    public void ShowUnavailableAnyTableCanvas()
    {
        _unavailableAnyTableCanvas.SetActive(true);
    }

    public void HideUnavailableAnyTableCanvas()
    {
        _unavailableAnyTableCanvas.SetActive(false);
    }
    public void ShowConeCanvas()
    {
        _coneCanvas.SetActive(true);
    }

    public void HideConeCanvas()
    {
       // Debug.Log("Hiding Cone Canvas");
        _coneCanvas.GetComponent<Canvas>().enabled=false;
      
    }

}
