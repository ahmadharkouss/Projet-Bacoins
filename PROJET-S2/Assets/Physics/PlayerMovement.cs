using System;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Timers;
using TMPro;
//using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class PlayerMovement : NetworkBehaviour
{
    protected float fallSpeed;
    protected float jumpForce;
    protected float secondJumpForce;
    protected float maxSpeed;
    protected float maxWalkSpeed;
    protected float initialDash;
    protected float airSpeed;
    protected float airAcceleration;
    protected float gravity;
    protected float weight;
    protected float size;
    protected int remainJump;
    protected int health;

    protected PlayerMovement ennemy;
    
    protected int ennemyhealth = 0;
    
    protected int _remainJump;
    
    protected PhysicMaterial _pm;
    
    protected Rigidbody _rb;

    protected Animator _an;

    protected bool _invincible = false;
    
    protected bool _intangible = false;
    
    private bool _isRunning = false;
    
    private bool _isOnGround = false;
    
    private bool _crouch = false;
    
    private float _maxSpeed;
    
    private float _acceleration;
    
    private float _fallSpeed;
    
    private int _isOnLedge = 1;

    protected bool _damageTaken = false;

    protected Rigidbody _canAttack;

    // Start is called before the first frame update
    void Start()
    {
        _pm = GetComponent<BoxCollider>().material;
        _rb = GetComponent<Rigidbody>();
        _an = GetComponent<Animator>();
        transform.localScale = new Vector3(size * gameObject.transform.localScale.x, size * gameObject.transform.localScale.y, size * gameObject.transform.localScale.z);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "TGround")
        {
            if (_intangible == false)
            {
                Physics.IgnoreCollision(other.gameObject.GetComponent<BoxCollider>(), GetComponent<BoxCollider>(),false);
            }
        }
        if (other.gameObject.tag == "Joueur")
        {
            ennemy = other.GetComponent<PlayerMovement>();
            _canAttack = other.GetComponent<Rigidbody>();
        }
    }
    private void OnTriggerExit(Collider other)
    { 
        if (other.gameObject.tag == "Joueur")
        {
            _canAttack = null;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        //detect if player on ground
        if (other.gameObject.tag == "Ground" || other.gameObject.tag == "TGround")
        {
            _remainJump = remainJump;
            _isOnGround = true;
            changeVelocity(_rb.velocity.x, 0);
            _maxSpeed = maxSpeed;
            _acceleration = 1;
        }
        //detect if player on ledge
        else if (other.gameObject.tag == "Ledge")
        {
            _remainJump = remainJump;
            _isOnLedge = 0;
            changeVelocity(_rb.velocity.x, 0);
            _rb.useGravity = false;
            _invincible = true;
        }

    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "TGround")
        {
            if (_intangible)
            {
                Physics.IgnoreCollision(other.gameObject.GetComponent<BoxCollider>(), GetComponent<BoxCollider>(),true);
            }
        }
    }

    void OnCollisionExit(Collision other)
    {
        //detect if player on ground
        if (other.gameObject.tag == "Ground"|| other.gameObject.tag == "TGround")
        {
            _isOnGround = false;
            _maxSpeed = airSpeed;
            _acceleration = airAcceleration / 10;
        }
        //detect if player on ledge
        else if (other.gameObject.tag == "Ledge")
        {
            _isOnLedge = 1;
            _rb.useGravity = true;
            _invincible = false;
        }
    }

    void Update()
    {
        HandleMovment();
    }

    // Update is called once per frame
    void HandleMovment()
    {
        if (isLocalPlayer)
        {
            if (_crouch && Input.GetKey(KeyCode.Q) == false && Input.GetKey(KeyCode.D) == false)
            {
                _intangible = true;
            }
            else
            {
                _intangible = false;
            }

            if (_damageTaken)
            {
                _pm.bounciness = 1;
            }
            else
            {
                _pm.bounciness = 0;
            }

            if (_rb.velocity.x <= maxSpeed && _rb.velocity.x >= -maxSpeed && _rb.velocity.y <= maxSpeed &&
                _rb.velocity.y >= -maxSpeed)
            {
                _damageTaken = false;
            }

            //if player not on ground: make it fall down
            if (_isOnGround == false)
            {
                Vector2 movement = new Vector2(0, _isOnLedge * (-gravity * weight - _fallSpeed - (float) 0.1));
                _rb.AddForce(movement);
            }

            //detect if player is crouching
            if (Input.GetKey(KeyCode.S))
            {
                _crouch = true;
                _fallSpeed = fallSpeed;
                _isOnLedge = 1;
                _rb.useGravity = true;
            }
            //if player isn't crouching
            else
            {
                _crouch = false;
                _fallSpeed = 0;
            }

            //detect if player isn't moving
            if (Input.GetKey(KeyCode.D) == false && Input.GetKey(KeyCode.Q) == false)
            {
                _an.SetBool("isrunning", false);
                _isRunning = false;
                changeVelocity(_rb.velocity.x / (float) 1.01, _rb.velocity.y);
            }

            //detect if player is running to the left
            if (_isRunning && Math.Abs(_rb.velocity.x) < _maxSpeed && Input.GetKey(KeyCode.D))
            {
                _an.SetBool("isrunning", true);
                changeVelocity(_rb.velocity.x + _acceleration, _rb.velocity.y);
            }

            //detect if player is running to the right
            if (_isRunning && Math.Abs(_rb.velocity.x) < _maxSpeed && Input.GetKey(KeyCode.Q))
            {
                _an.SetBool("isrunning", true);
                changeVelocity(_rb.velocity.x - _acceleration, _rb.velocity.y);
            }

            //detect if player start moving to the left
            if (Input.GetKeyDown(KeyCode.D) && _isOnGround && _crouch == false)
            {
                _an.SetBool("isrunning", true);
                changeVelocity(initialDash, _rb.velocity.y);
            }

            if (Input.GetKeyDown(KeyCode.Q) && _isOnGround && _crouch == false)
            {
                _an.SetBool("isrunning", true);
                changeVelocity(-initialDash, _rb.velocity.y);
            }

            if (Input.GetKey(KeyCode.D))
            {
                gameObject.transform.rotation = new Quaternion(-90, 90, 90, 90);
                if (_isOnGround && _crouch)
                {
                    _maxSpeed = maxWalkSpeed;
                }
                else if (_isOnGround && _crouch == false)
                {
                    _maxSpeed = maxSpeed;
                }

                _isRunning = true;
            }

            //detect if player start moving to the right
            if (Input.GetKey(KeyCode.Q))
            {
                gameObject.transform.rotation = new Quaternion(-90, -90, -90, 90);
                if (_isOnGround && _crouch)
                {
                    _maxSpeed = maxWalkSpeed;
                }
                else if (_isOnGround && _crouch == false)
                {
                    _maxSpeed = maxSpeed;
                }

                _isRunning = true;
            }

            //detect if player try to jump
            if (Input.GetKeyDown(KeyCode.Space) && _remainJump > 0)
            {
                _an.SetTrigger("isjumping");
                if (_remainJump == remainJump)
                {
                    changeVelocity(_rb.velocity.x, jumpForce);
                    _remainJump--;
                }
                else
                {
                    changeVelocity(_rb.velocity.x, secondJumpForce);
                    _remainJump--;
                }
            }

            //detect if player want to do a melee attack
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                meleeAttack();
                ennemyhealth += 5;
                ennemy._damageTaken = true;
            }

            //detect if player want to do a range attack
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                rangeAttack();
            }

            //detect if player want to do a down attack
            if (Input.GetKeyDown(KeyCode.DownArrow) && _isOnGround == false)
            {
                downAttack();
            }

            //detect if player want to do a special attack
            if (Input.GetKeyDown(KeyCode.UpArrow) && _isOnGround)
            {
                specialAttack();
            }
            //detect if player want to do a air special attack
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && _isOnGround == false)
            {
                airSpecialAttack();
            }
        }
    }
    //function to set the velocity to x and y
    public void changeVelocity(float x, float y)
    {
        _rb.velocity = new Vector2(x, y);
    }

    public abstract void meleeAttack();
    
    public abstract void rangeAttack();
    
    public abstract void downAttack();
    
    public abstract void specialAttack();

    public abstract void airSpecialAttack();
}
