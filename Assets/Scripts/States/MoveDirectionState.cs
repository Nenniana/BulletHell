using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDirectionState : IState
{
    protected ProjectileControl owner;
    protected Projectile projectile;
    
    public MoveDirectionState(ProjectileControl owner) { this.owner = owner; this.projectile = owner.projectile; }

    public void MoveInDirection()
    {
        //Debug.Log("Moving RigidBody");
        //rigidbody2D.AddForce(projectile.direction * 150 * Time.deltaTime);
        //rigidbody2D.velocity = projectile.direction * 150 * Time.deltaTime;
        owner.rb2D.MovePosition ((Vector2)owner.transform.position + (projectile.direction * projectile.currentMovementSpeed * Time.deltaTime));
        //owner.transform.Translate(projectile.direction * projectile.currentMovementSpeed * Time.deltaTime, Space.World);
    }

    public virtual void Enter()
    {
        //Debug.Log("entering MoveDirectionState state");
    }

    public virtual void Execute()
    {
        //Debug.Log("updating MoveDirectionState state");
    }

    public virtual void FixedExecute()
    {
        //Debug.Log("updating test state");
        MoveInDirection();
    }

    public virtual void Exit()
    {
        //Debug.Log("exiting MoveDirectionState state");
    }
}