using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSpeedState : MoveDirectionState, IState
{
    private bool movedFarEnough = false;
    private bool accelerate = true;
    private bool switchSpeedOnce = true;

    public MoveSpeedState(ProjectileControl owner) : base(owner) { }

    public void SwitchSpeedBasedOnDistanceTraveled(Vector3 centerPosition)
    {
        
        if (switchSpeedOnce)
        {
            if ((projectile.currentMovementSpeed == projectile.projectileUnit.projectileSpeed && owner.GetDistancedTraveled(centerPosition, projectile.lastPosition) >= projectile.projectileUnit.switchSpeedDistance) || (projectile.currentMovementSpeed == projectile.projectileUnit.switchSpeedSpeed && owner.GetDistancedTraveled(centerPosition, projectile.lastPosition) >= projectile.projectileUnit.switchSpeedDistanceSecond))
            {
                projectile.lastPosition = centerPosition;

                if (projectile.projectileUnit.switchSpeedsContinue)
                    projectile.currentMovementSpeed = SwitchBetweenSpeeds();
                else
                {
                    projectile.currentMovementSpeed = projectile.projectileUnit.switchSpeedSpeed;
                    switchSpeedOnce = false;
                }
            }
        }
    }

    public float SwitchBetweenSpeeds()
    {
        if (projectile.currentMovementSpeed == projectile.projectileUnit.projectileSpeed)
            return projectile.projectileUnit.switchSpeedSpeed;
        else
            return projectile.projectileUnit.projectileSpeed;
    }

    public void ChangeSpeedGradually(ref float speedToChange)
    {
        if (projectile.projectileUnit.speedUpper != 0 && projectile.projectileUnit.speedLower != 0)
        {
            DetermineSpeedDirection(ref speedToChange);
            if (accelerate)
                speedToChange += projectile.projectileUnit.acceleration * Time.deltaTime;
            else
                speedToChange -= projectile.projectileUnit.acceleration * Time.deltaTime;
        }
    }

    public void DetermineSpeedDirection(ref float speedToChange)
    {
        if (projectile.projectileUnit.speedUpper <= speedToChange)
            accelerate = false;
        else if (projectile.projectileUnit.speedLower >= speedToChange)
            accelerate = true;
    }

    public void CheckMouse()
    {
        if (movedFarEnough || projectile.projectileUnit.followMouse && owner.GetDistancedTraveled(owner.transform.position, projectile.startPosition) >= projectile.projectileUnit.followAfterDistance)
        {
            movedFarEnough = true;

            if (owner.GetDistancedTraveled(owner.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition)) <= projectile.projectileUnit.followFromDistance)
                owner.stateMachine.ChangeState(new MoveToMouseState(owner));

            else
                MoveInDirection();
        }
        else
            MoveInDirection();
    }

    public override void Enter()
    {
        //Debug.Log("Entering MoveSpeedState state");
        base.Enter();
    }

    public override void Execute()
    {
        //Debug.Log("Execute MoveSpeedState state");
        ChangeSpeedGradually(ref projectile.currentMovementSpeed);
        SwitchSpeedBasedOnDistanceTraveled(owner.gameObject.transform.position);
    }

    public override void FixedExecute()
    {
        //Debug.Log("Execute MoveSpeedState state");
        CheckMouse();
    }
}