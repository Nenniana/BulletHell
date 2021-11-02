using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeState : AnimationState, IState
{
    public ExplodeState(ProjectileControl owner) : base(owner) { }

    public override void Enter()
    {
        //Debug.Log("Entering ExplodeState state");
        explosion = GameObject.Instantiate(projectile.projectileUnit.projectileObject.explosionPrefab, owner.transform.position, Quaternion.identity);
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
        //Debug.Log("exiting ExplodeState state");
        base.Exit();
    }
}