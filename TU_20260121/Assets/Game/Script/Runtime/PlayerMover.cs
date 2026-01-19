using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] private bool _manualOverride;
    [SerializeField] private float _speed;
    [SerializeField] private float _gravity;
    [SerializeField] private float _jumpSpeed;
    [SerializeField] private Rigidbody2D _rigidBody2D;
    [SerializeField] private GroundCheck _groundCheck;

    void Awake()
    {
        InputHandler.ins.OnClickOrTap += Jump;
    }

    void ODestroy()
    {
        InputHandler.ins.OnClickOrTap -= Jump;
    }

    void Start()
    {
        var vals = GameManager.ins.gameValues;
        if(_manualOverride)
        {
            _speed = vals.Speed;
            _rigidBody2D.gravityScale = vals.Gravity;
            _jumpSpeed = vals.JumpSpeed;
        }
   }

    void FixedUpdate()
    {
        _rigidBody2D.linearVelocity = new Vector2(_speed, _rigidBody2D.linearVelocity.y);
    }

    void Jump()
    {
        if(_groundCheck.CheckGround())
        {
            _rigidBody2D.linearVelocity = new Vector2(_rigidBody2D.linearVelocity.x, _jumpSpeed);
        }
    }
}
