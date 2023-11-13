using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssembleCounter : MonoBehaviour
{
    public static AssembleCounter instance;
    public Stack<Transform> CounterCones = new Stack<Transform>();
    public Stack<Transform> PickUpBoxes= new Stack<Transform>();

    public CollectController cc;
    public  EmployeeController emp;
    public Transform conePlacer;
 
    private int maxConesOnCounter = 6;

    public Transform trayPlacer;
    public Transform readyBox;
    public GameObject trayPrefab;

    bool isTrayCreated=false;

    

    public bool unlocked = false;

    private void Awake()
    {
        emp = FindAnyObjectByType<EmployeeController>();
    }
    private void Start()
    {
        instance = this;
        cc = FindAnyObjectByType<CollectController>();

      
        unlocked = true;

      
    }

    private void FixedUpdate()
    {
        emp = FindAnyObjectByType<EmployeeController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Check if the player is holding cones
            bool isHoldingCones = cc.collectedStuff.Count > 0;

            if (isHoldingCones)
            {
                // Calculate how many cones can be placed on the counter
                int availableSlots = Mathf.Min(maxConesOnCounter - conePlacer.childCount, cc.collectedStuff.Count);

                for (int i = 0; i < availableSlots; i++)
                {
                    Transform cone = cc.collectedStuff.Pop();
                    Vector3 targetPosition = conePlacer.position + Vector3.up * conePlacer.childCount * 0.50f;
                    CounterCones.Push(cone);
                    // Make the cone jump towards the counter's conePlacer
                    cone.DOJump(targetPosition, 1.0f, 1, 0.5f);

                    cone.SetParent(conePlacer);

                    cc._isCarryingCone = false;
                }
            }


        }
        if (other.CompareTag("Employee"))
        {
           
                // Check if the player is holding cones
                bool isHoldingCones = emp._employee1Cones.Count > 0;

                if (isHoldingCones)
                {
                    // Calculate how many cones can be placed on the counter
                    int availableSlots = Mathf.Min(maxConesOnCounter - conePlacer.childCount, emp._employee1Cones.Count);

                    for (int i = 0; i < availableSlots; i++)
                    {
                        Transform cone = emp._employee1Cones.Pop();
                        Vector3 targetPosition = conePlacer.position + Vector3.up * conePlacer.childCount * 0.50f;
                        CounterCones.Push(cone);
                        // Make the cone jump towards the counter's conePlacer
                        cone.DOJump(targetPosition, 1.0f, 1, 0.5f);

                        cone.SetParent(conePlacer);

                        cc._isCarryingCone = false;
                    }
                }


         }

       
    }

    public void Update()
    {
        if (CounterEmployee.instance.isAtCounter && CounterCones.Count > 0)
        {
            // Check if a tray is not already created
            if (!isTrayCreated)
            {
                CreateTrayAndMoveCones();
                isTrayCreated = true;
            }
        }
        else
        {
            // Reset the flag if the conditions are not met
            isTrayCreated = false;
        }

    }

    private void CreateTrayAndMoveCones()
    {
        // Create a new tray or box prefab
        GameObject tray = Instantiate(trayPrefab, trayPlacer.position, Quaternion.identity);

        // Use a coroutine to move the cones one by one into the tray
        StartCoroutine(MoveConesIntoTray(tray));
    }


    private IEnumerator MoveConesIntoTray(GameObject tray)
    {
        for(int i = 0; i < 4; i++)
        {
            // Get a cone from CounterCones stack
            Transform cone = CounterCones.Pop();

            // Calculate the position within the tray (you'll need to adjust these positions based on your tray's layout)
            Vector3 traySlotPosition = tray.transform.GetChild(i).position;

            // Move the cone to the tray slot with a delay between cones
            cone.DOJump(traySlotPosition, 0.5f, 1, 0.5f);

            // Parent the cone to the tray
            cone.SetParent(tray.transform);

            // Wait for a delay before moving the next cone
            yield return new WaitForSeconds(1f); // Adjust the delay as needed
        }

        // Continue with any other logic here
        PickUpBoxes.Push(tray.transform);

        // Calculate the final position of the tray in a stacking manner
        Vector3 finalTrayPosition = readyBox.position + Vector3.up * (tray.GetComponent<Renderer>().bounds.size.y * PickUpBoxes.Count+ 0.1f);

        // Move the entire tray to the final position
        tray.transform.DOMove(finalTrayPosition, 1.0f);
    }


}





