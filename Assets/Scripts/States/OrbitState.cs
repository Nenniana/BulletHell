using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitState : MoveOffsetState, IState
{
    public OrbitState(ProjectileControl owner) : base(owner) { }

    public override void Enter()
    {
        //Debug.Log("Entering Orbit state");
        base.Enter();
    }

    public override void Execute()
    {
        ChangeSpeedGradually(ref projectile.projectileUnit.orbitRoationSpeed);
        projectile.theta += projectile.projectileUnit.orbitRoationSpeed * Time.deltaTime;

        offset = new Vector2(Mathf.Sin(projectile.theta), Mathf.Cos(projectile.theta)) * projectile.projectileUnit.orbitRadius;
        base.Execute();
    }

    public override void FixedExecute()
    {
        base.FixedExecute();
    }
}