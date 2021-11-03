using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileControl : MonoBehaviour, Ipool
{

    internal StateMachine stateMachine = new StateMachine();
    internal SpriteRenderer spriteRenderer;
    internal Projectile projectile;
    internal Rigidbody2D rb2D;
    public bool active { get; set; }

    public void Initialize()
    {
        //Debug.Log("Initialize fired.");
        active = false;
        rb2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        /*DestroyAllProjectiles destroyAll = player.GetComponent<DestroyAllProjectiles>();
        destroyAll.OnDestoyAllProjectiles += DestroySelf;*/
    }

    /*private void OnDestroy()
    {
        if (player != null)
        {
            DestroyAllProjectiles destroyAll = player.GetComponent<DestroyAllProjectiles>();
            destroyAll.OnDestoyAllProjectiles -= DestroySelf;
        }
        
    }*/

    public void ExplodeDisable ()
    {
        stateMachine.ChangeState(new ExplodeState(this));
    }

    public void SpawnObj(Projectile _projectile, Vector3 position)
    {
        //Debug.Log("SpawnObj fired.");
        
        projectile = _projectile;
        projectile.startPosition = position;
        SetRendererAndCollider();

        if (projectile.isOrbit)
        {
            transform.position = projectile.midPosition + projectile.startPosition;

            if (projectile.projectileUnit.rotate)
                stateMachine.ChangeState(new OrbitState(this));
            else
                stateMachine.ChangeState(new MoveOffsetState(this));
        }
        else
        {
            transform.position = projectile.startPosition;
            stateMachine.ChangeState(new MoveSpeedState(this));
        }

        active = true;
    }

    private void DestroySelf (object sender, EventArgs e)
    {
        if (active)
            stateMachine.ChangeState(new DisabledState(this));
    }

    private void OnBecameInvisible()
    {
        DisableObjects(1);
    }

    private void OnBecameVisible()
    {
        StopCoroutine("RemoveAfterSeconds");
    }

    private void Update()
    {
        if (active)
        {
            stateMachine.Update();
            TickLife();
            DisableBasedOnDistance();
        }
    }

    private void FixedUpdate()
    {
        if (active)
        {
            stateMachine.FixedUpdate();
        }
    }

    public float GetDistancedTraveled(Vector3 fromPosition, Vector2 toPosition)
    {
        return Vector2.Distance(fromPosition, toPosition);
    }

    public void DisableBasedOnDistance()
    {
        if (projectile.projectileUnit.lifeDistance != 0 && GetDistancedTraveled(projectile.startPosition, transform.position) >= projectile.projectileUnit.lifeDistance)
            stateMachine.ChangeState(new DisabledState(this));
    }

    private void SetRendererAndCollider()
    {
        spriteRenderer.enabled = true;
        spriteRenderer.sprite = projectile.projectileUnit.projectileObject.projectileSprite;
    }

    private void TickLife() // Implement fall, add bool?
    {
        if (projectile.projectileUnit.lifeTime != 0)
            projectile.lifeTime -= Time.deltaTime;
        if (projectile.lifeTime < 0)
            stateMachine.ChangeState(new DisabledState(this));
    }

    public void DestroyObjects(GameObject _objectToDestroy, float lifeTime = 0)
    {
        GameObject.Destroy(_objectToDestroy, lifeTime);
    }

    public void DisableObjects(float lifeTime = 0)
    {
        StopCoroutine("RemoveAfterSeconds");
        if (gameObject.activeInHierarchy)
            StartCoroutine(RemoveAfterSeconds(lifeTime));
    }
    IEnumerator RemoveAfterSeconds(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        stateMachine.ChangeState(new DisabledState(this));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            if (projectile.projectileUnit.wallBounce)
            {
                Vector2 inNormal = collision.contacts[0].normal;
                projectile.direction = Vector2.Reflect(transform.position, inNormal);

                stateMachine.ChangeState(new MoveDirectionState(this));
            }
            else
            {
                stateMachine.ChangeState(new DisabledState(this));
            }
        }
    }

    /*IEnumerator Reflect(Collision2D collision)
    {
        float saveSpeed = projectile.currentMovementSpeed;
        projectile.currentMovementSpeed = 0f;
        Vector2 inNormal = collision.contacts[0].normal;
        projectile.direction = Vector2.Reflect(transform.position, inNormal);
        projectile.currentMovementSpeed = saveSpeed;
        yield return null; 
    }*/
}
