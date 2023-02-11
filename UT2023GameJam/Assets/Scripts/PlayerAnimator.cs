using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerMovement player;
    private int IDLE = 0, RUN = 1, JUMP = 2, FALL = 3;

    private void Update()
    {
        if (player.IsJumping)
            animator.SetInteger("MoveState", JUMP);
        else if (player.IsFalling)
            animator.SetInteger("MoveState", FALL);
        else if (player.IsRunning)
            animator.SetInteger("MoveState", RUN);
        else
            animator.SetInteger("MoveState", IDLE);
    }
}
