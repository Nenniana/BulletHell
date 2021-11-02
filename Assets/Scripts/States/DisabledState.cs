using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisabledState : IState
{
    protected ProjectileControl owner;
    protected Projectile projectile;

    public DisabledState(ProjectileControl owner) { this.owner = owner; this.projectile = owner.projectile; }

    public void Enter()
    {
        //Debug.Log("Entering DisabledState state");
        owner.active = false;
        owner.gameObject.SetActive(false);
        PoolManager.instance.ReQueueObject(owner.gameObject, "ProjectileObject 1");
    }

    public void Execute()
    {
        //Debug.Log("updating test state");
    }

    public void FixedExecute()
    {
        //Debug.Log("updating test state");
    }

    public void Exit()
    {
        //Debug.Log("exiting DisabledState state");
    }
}