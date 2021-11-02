using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationState : IState
{
    protected ProjectileControl owner;
    protected Projectile projectile;
    protected GameObject explosion;

    public AnimationState(ProjectileControl owner) { this.owner = owner; this.projectile = owner.projectile; }

    public void ExplodeAnimation()
    {
        ParticleSystem exp = explosion.GetComponent<ParticleSystem>();
        exp.textureSheetAnimation.SetSprite(0, projectile.projectileUnit.projectileObject.projectileSprite);
        float explosionDuration = exp.main.duration;
        owner.spriteRenderer.enabled = false;
        owner.DestroyObjects(explosion, explosionDuration);
        owner.DisableObjects(explosionDuration);
    }

    public virtual void Enter()
    {
        ExplodeAnimation();
        owner.stateMachine.ChangeState(new DisabledState(owner));
    }

    public virtual void Execute()
    {
        //Debug.Log("updating ExplodeState state");
    }

    public virtual void FixedExecute()
    {
        //Debug.Log("updating test state");
    }

    public virtual void Exit()
    {
        owner.spriteRenderer.enabled = true;
    }
}