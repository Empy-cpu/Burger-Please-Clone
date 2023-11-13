using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerAnimState
{
    Idle,
    Walk,
    Sit
}
public class PlayerAnimationHandler : MonoBehaviour
{
        [ReadOnly][SerializeField] private PlayerAnimState playerAnimState;
        [ReadOnly][SerializeField] private Animator playerAnim;
        [SerializeField] private string idleString, walkString, sitString;

        private void Start()
        {
            playerAnim = GetComponentInChildren<Animator>();
        }

        [Button(ButtonSizes.Small)]
        public void MakeCharacterIdle()
        {
            if (playerAnimState == PlayerAnimState.Idle) return;
            playerAnimState = PlayerAnimState.Idle;
            playerAnim.SetTrigger(idleString);
        }

        [Button(ButtonSizes.Small)]
        public void MakeCharacterWalk()
        {
            if (playerAnimState == PlayerAnimState.Walk) return;
            playerAnimState = PlayerAnimState.Walk;
            playerAnim.SetTrigger(walkString);
        }

        [Button(ButtonSizes.Small)]
        public void MakeCharacterSit()
        {
            if (playerAnimState == PlayerAnimState.Sit) return;
            playerAnimState = PlayerAnimState.Sit;
            playerAnim.SetTrigger(sitString);
        }


}


