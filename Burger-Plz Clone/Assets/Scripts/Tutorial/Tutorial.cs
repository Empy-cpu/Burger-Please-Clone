
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public static Tutorial instance;

    public GameObject[] Tables;
    public GameObject[] IceCreamStoves;
    public GameObject[] SecondArea;


    public bool tutorialCompleted = false;
    
    public void Awake()
    {
        instance = this;
    }

    public void StartTutorial()
    {
        if (Cashier.instance != null && Cashier.instance.unlocked )
        {

            TakeToTable();
           

        }


        if (tutorialCompleted)
        {

            GameManager.instance._isGameStarted = true;
            ActivateRestOfTables();
        }
    }

    public void TakeToTable()
    {
        Tables[0].gameObject.SetActive(true);
        if (Tables[0].transform.GetChild(0).gameObject.activeSelf)
        {
            TakeToStove();
        }
    }

    public void TakeToStove()
    {
        IceCreamStoves[0].gameObject.SetActive(true);
        if (IceCreamMachine.instance!=null &&IceCreamMachine.instance.unlocked)
        {
            tutorialCompleted = true;
            ActivateRestOfTables();
        }
    }
    private void ActivateRestOfTables()
    {
        // Loop through the Tables array, starting from index 1 (assuming Tables[0] is already active)
        for (int i = 1; i < Tables.Length; i++)
        {
            if (!Tables[i].activeSelf)
            {
                Tables[i].SetActive(true);
            }
        }

        ActivateSecondArea();
    }

    private void ActivateSecondArea()
    {
        for (int i = 0; i < SecondArea.Length; i++)
        {
            if (!SecondArea[i].activeSelf)
            {
                SecondArea[i].SetActive(true);
            }
        }
    }

}




