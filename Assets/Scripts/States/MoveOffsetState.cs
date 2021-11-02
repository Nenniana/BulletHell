using UnityEngine;

public class MoveOffsetState : MoveSpeedState, IState
{
    protected Vector2 offset;
    private Vector2 movingCenter = Vector2.zero;

    public MoveOffsetState(ProjectileControl owner) : base(owner) { }

    public void UpdateCenter ()
    {
        movingCenter += projectile.direction * projectile.currentMovementSpeed * Time.deltaTime;
    }

    public void MoveWithOffset ()
    {
        owner.rb2D.MovePosition((movingCenter + offset));
        //owner.transform.position = movingCenter + offset;
    }

    public override void Enter()
    {
        //Debug.Log("entering MoveOffsetState state");
        base.Enter();
        movingCenter = projectile.midPosition;
        offset = projectile.startPosition;
    }

    public override void Execute()
    {
        //Debug.Log("updating MoveOffsetState state");
        UpdateCenter();
        ChangeSpeedGradually(ref projectile.currentMovementSpeed);
        SwitchSpeedBasedOnDistanceTraveled(movingCenter);
    }

    public override void FixedExecute()
    {
        MoveWithOffset();
    }

    public override void Exit()
    {
        //Debug.Log("exiting MoveOffsetState state");
    }
}