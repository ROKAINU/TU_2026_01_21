using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    private float _gravity;
    private float _jumpSpeed;
    private int _maxJumpCount;
    private float _deathZoneOffset;
    [SerializeField] private float wallCheckDistance = 0.1f;
    [SerializeField] private Rigidbody2D _rigidBody2D;
    [SerializeField] private GroundCheck _groundCheck;
    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private PhysicsMaterial2D _noFrictionMaterial;

    private float _speed;
    private int _jumpCount = 0;

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
        var vals = GameSettingValues.ins;
        _gravity = vals.Gravity;
        _jumpSpeed = vals.JumpSpeed;
        _speed = vals.Speed;
        _rigidBody2D.gravityScale = _gravity;
        _maxJumpCount = vals.MaxJumpCount;
        _deathZoneOffset = vals.DeathZoneOffset;

        if(_noFrictionMaterial != null)
        {
            _rigidBody2D.sharedMaterial = _noFrictionMaterial;
        }
   }

    private bool IsWallTouch()
    {
        Vector2 origin = (Vector2)transform.position + Vector2.right * 0.3f;
        RaycastHit2D hit = Physics2D.Raycast
            (origin, 
            Vector2.right, 
            wallCheckDistance, 
            _wallLayer);

        return hit.collider != null;
    }

    void FixedUpdate()
    {
        var yVelocity = _rigidBody2D.linearVelocity.y;
        _rigidBody2D.linearVelocity = new Vector2(0, yVelocity);
    }

    void Update()
    {
        if(_groundCheck.CheckGround())
            _jumpCount = _maxJumpCount;

        var xSpeed = _speed;
        if(IsWallTouch())
            xSpeed = 0;
        transform.position = new Vector2(transform.position.x + xSpeed, transform.position.y);

        if(transform.position.x - GameController.ins.GameCenter < _deathZoneOffset)
        {
            SceneTransitionerToResultFromPlay.ToResultFromPlay();
        }
    }

    void Jump()
    {
        if(_groundCheck.CheckGround() || _jumpCount > 1)
        {
            _jumpCount--;
            _rigidBody2D.linearVelocity = new Vector2(0, _jumpSpeed);
        }
    }
}
