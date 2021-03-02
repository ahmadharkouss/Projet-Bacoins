using System;
using System.Collections;
using System.Collections.Generic;
//using TMPro.EditorUtilities;
using UnityEngine;

namespace Player
{
    public class PlayerTest : PlayerMovement
    {
        public PlayerTest()
        {
            fallSpeed = 5;
            jumpForce = 10;
            secondJumpForce = 7;
            maxSpeed = 15;
            maxWalkSpeed = 7;
            initialDash = 12;
            airSpeed = 10;
            airAcceleration = 1;
            gravity = 0;
            weight = 0;
            size = 1;
            remainJump = 2;
            health = 0;
        }

        public override void downAttack()
        {
            throw new NotImplementedException();
        }

        public override void meleeAttack()
        {
            _canAttack.velocity = (_canAttack.transform.position - _rb.transform.position).normalized * ennemyhealth;
            _canAttack.velocity = new Vector2(_canAttack.velocity.x, _canAttack.velocity.y + (5 * (ennemyhealth * (float)0.05)));
            
        }

        public override void rangeAttack()
        {
            throw new NotImplementedException();
        }

        public override void specialAttack()
        {
            throw new NotImplementedException();
        }

        public override void airSpecialAttack()
        {
            throw new NotImplementedException();
        }
    }
    
    public class Dummy : PlayerMovement
    {
        public Dummy()
        {
            fallSpeed = 5;
            jumpForce = 0;
            secondJumpForce = 0;
            maxSpeed = 0;
            maxWalkSpeed = 0;
            initialDash = 0;
            airSpeed = 0;
            airAcceleration = 0;
            gravity = 0;
            weight = 0;
            size = 1;
            remainJump = 0;
            health = 0;
        }

        public override void downAttack()
        {
            throw new NotImplementedException();
        }

        public override void meleeAttack()
        {
            throw new NotImplementedException();
        }

        public override void rangeAttack()
        {
            throw new NotImplementedException();
        }

        public override void specialAttack()
        {
            throw new NotImplementedException();
        }

        public override void airSpecialAttack()
        {
            throw new NotImplementedException();
        }
    }
}
