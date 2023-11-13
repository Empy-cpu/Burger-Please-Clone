using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool _isGameStarted;

    private void Awake()
    {
       
            instance = this;    
       
    }
    void Start()
    {
        _isGameStarted = false;

    }

    private void Update()
    {

        Tutorial.instance.StartTutorial();
    }


}

