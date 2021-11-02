using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileBase : MonoBehaviour
{
    [HideInInspector]
    public Projectile projectile;
    private string objectName;

    private void Start()
    {
        objectName = gameObject.name.Replace("(Clone)", "");
    }

    public void RotateSelf()
    {
        if (projectile.projectileUnit.rotate)
            transform.Rotate(new Vector3(0, 0, projectile.projectileUnit.orbitRoationSpeed));
    }

    public void MoveFoward()
    {
        if (projectile.moveFoward)
            transform.Translate(projectile.direction * projectile.currentMovementSpeed * Time.deltaTime, Space.World);
    }

    public void DestroyObjects(GameObject _objectToDestroy, float lifeTime = 0)
    {
        GameObject.Destroy(_objectToDestroy, lifeTime);
    }

    public void DisableObjects(GameObject _objectToDestroy, float lifeTime = 0)
    {
        StopCoroutine("RemoveAfterSeconds");
        if (gameObject.activeInHierarchy)
            StartCoroutine(RemoveAfterSeconds(_objectToDestroy, lifeTime));
    }
    IEnumerator RemoveAfterSeconds(GameObject objectToDisable, float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        objectToDisable.SetActive(false);
    }

    private void OnBecameInvisible()
    {
        PoolManager.instance.ReQueueObject(gameObject, objectName);
        DisableObjects(gameObject, 0);
    }

    private void Update()
    {
        TickLife();
    }

    private void TickLife()
    {
        if (projectile.lifeTime != 0)
            projectile.lifeTime -= Time.deltaTime;
    }

    public void ResetProjectile()
    {
        /*projectile.moveFoward = false;
        projectile.accelerate = true;*/
    }

    public float GetDistancedTraveled(Vector2 startPostion)
    {
        return Vector2.Distance(transform.position, startPostion);
    }
    public void ChangeSpeedGradually()
    {
        if (projectile.projectileUnit.speedUpper != 0 && projectile.projectileUnit.speedLower != 0)
        {
            DetermineSpeedDirection();
            if (projectile.accelerate)
                projectile.currentMovementSpeed += projectile.projectileUnit.acceleration * Time.deltaTime;
            else
                projectile.currentMovementSpeed -= projectile.projectileUnit.acceleration * Time.deltaTime;
        }
    }
    public void DetermineSpeedDirection()
    {
        if (projectile.projectileUnit.speedUpper <= projectile.currentMovementSpeed)
            projectile.accelerate = false;
        else if (projectile.projectileUnit.speedLower >= projectile.currentMovementSpeed)
            projectile.accelerate = true;
    }

    public void ExplodeAnimation()
    {
        SpriteRenderer spriteRenderer = transform.GetComponent<SpriteRenderer>();
        float explosionDuration = 0;
        if (spriteRenderer.enabled)
        {
            GameObject explosion = Instantiate(projectile.projectileUnit.projectileObject.explosionPrefab, transform.position, Quaternion.identity);
            ParticleSystem exp = explosion.GetComponent<ParticleSystem>();
            exp.textureSheetAnimation.SetSprite(0, projectile.projectileUnit.projectileObject.projectileSprite);
            explosionDuration = exp.main.duration;

            spriteRenderer.enabled = false;
            DestroyObjects(explosion, explosionDuration);
        }
        DestroyObjects(gameObject, explosionDuration);
    }

    public static float Angle(Vector2 p_vector2)
    {
        if (p_vector2.x < 0)
        {
            return 360 - (Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg * -1);
        }
        else
        {
            return Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg;
        }
    }

    public abstract void OnCollisionEnter2D(Collision2D collision);
    public abstract void OnDisable();

}
