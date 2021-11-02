using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : AnimationState, IState
{
    public FallState(ProjectileControl owner) : base(owner) { }

    public override void Enter()
    {
        //Debug.Log("Entering FallState state");
        explosion = GameObject.Instantiate(projectile.projectileUnit.projectileObject.explosionPrefab, owner.transform.position, Quaternion.identity); //Change to fall prefab when implemented.
        base.Enter();
    }

    public override void Execute()
    {
        //Debug.Log("updating ExplodeState state");
    }

    public override void FixedExecute()
    {
        //Debug.Log("updating test state");
    }

    public override void Exit()
    {
        //Debug.Log("exiting FallState state");
        base.Exit();
    }
}