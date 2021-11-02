using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToMouseState : IState
{
    protected ProjectileControl owner;
    protected Projectile projectile;
    protected Vector2 target;

    public MoveToMouseState(ProjectileControl owner) { this.owner = owner; this.projectile = owner.projectile; }
    //private bool hasTraveledFar = false; // flip bool in owner to avoid distance call

    public void FollowMouse ()
    {
        owner.transform.position = Vector2.MoveTowards(owner.transform.position, target, projectile.currentMovementSpeed * Time.deltaTime);

        if (projectile.projectileUnit.MouseAsLastDirection)
        {
            projectile.direction = target;
        }
        else
        {
            projectile.direction = projectile.originalDirection;
            owner.stateMachine.ChangeState(new MoveSpeedState(owner));
        }
    }

    public void Enter()
    {

    }

    public void Execute()
    {
        //Debug.Log("updating MoveToMouseState state");
        target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void FixedExecute()
    {
        //Debug.Log("updating test state");
        FollowMouse();
    }

    public void Exit()
    {
        
    }
}