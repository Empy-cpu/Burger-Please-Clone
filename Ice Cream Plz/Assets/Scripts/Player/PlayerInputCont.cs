using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerInputCont : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private VariableJoystick _joystick;
    [SerializeField] private PlayerAnimationHandler playerAnimationHandler;

    public float _moveSpeed=9;
  
   
    private void Start()
    {
        _rigidbody=GetComponentInChildren<Rigidbody>();
        playerAnimationHandler=GetComponentInParent<PlayerAnimationHandler>();
      
    }
    private void FixedUpdate()
    {
        
        _rigidbody.velocity = new Vector3(_joystick.Horizontal * _moveSpeed, _rigidbody.velocity.y, _joystick.Vertical * _moveSpeed);

        if (_joystick.Horizontal != 0 || _joystick.Vertical != 0)
        {
            transform.rotation = Quaternion.LookRotation(_rigidbody.velocity);
            playerAnimationHandler.MakeCharacterWalk();
        }
        else
            playerAnimationHandler.MakeCharacterIdle();
    }

  

  
}
