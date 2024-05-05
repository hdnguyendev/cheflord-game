using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerAnimator : NetworkBehaviour
{
    private const String IS_WALKING = "IsWalking";
    [SerializeField] private Player player;
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Update(){
        if (!IsOwner) {
            return;
        }
        animator.SetBool(IS_WALKING, player.IsWalking());
    }
}
