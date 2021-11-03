using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    private float angle;
    private float rotationSpeed;
    private GameObject player;
    private EmitterInformation emitter;

    public GameObject projectilePrefab;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        UnpackProjectileEmitters unpackProjectileEmitters = GetComponent<UnpackProjectileEmitters>();
        unpackProjectileEmitters.OnEmitterTurn += Fire;
    }

    private void Update()
    {
        if (emitter != null)
        {
            InvertRotation();
            emitter.rotation += rotationSpeed * Time.deltaTime;
        }

    }

    public void Fire(object sender, UnpackProjectileEmitters.OnFireEventArgs currentEmitter)
    {

        emitter = currentEmitter.currentEmitter;
        rotationSpeed = emitter.projectileUnit.rotateSpeed;


        DetermineAngle();

        for (int i = 0; i < emitter.projectileUnit.bulletsAmount; i++)
        {
            float projectileDirectionX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);
            float projectileDirectionY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);

            Vector3 projectileDirection = (new Vector3(projectileDirectionX, projectileDirectionY, 0f) - transform.position).normalized;
            Projectile newProjectile = new Projectile(emitter.projectileUnit, projectileDirection, transform.position);

            if (!emitter.projectileUnit.ProjectileOrbits)
                PoolManager.instance.SpawnInfinite(newProjectile, projectilePrefab, transform.position);
            else
                InitializeOrbitProjectiles(newProjectile);

            angle += emitter.projectileUnit.spreadDistance;
        }
    }

    private void DetermineAngle()
    {
        if (emitter.projectileUnit.mouseTarget)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            SetFireAngle(mousePosition);
        }
        else if (emitter.projectileUnit.playerTarget && player != null)
        {
            Vector2 playerPosition = GameObject.FindGameObjectWithTag("Player").gameObject.transform.position;
            SetFireAngle(playerPosition);
        }
        else
        {
            angle = emitter.rotation;
        }

        angle += emitter.projectileUnit.offset;
    }

    private void InitializeOrbitProjectiles(Projectile projectile)
    {
        if (projectile.projectileUnit.ProjectileOrbits)
        {
            if (projectile.projectileUnit.rotate)
            {

                for (int i = 0; i < projectile.projectileUnit.numberOfOrbits; i++)
                {
                    float theta = (projectile.projectileUnit.orbitCircle * Mathf.PI / projectile.projectileUnit.numberOfOrbits) * i;

                    Vector2 orbitLocation = new Vector2(Mathf.Sin(theta) * projectile.projectileUnit.orbitRadius, Mathf.Cos(theta) * projectile.projectileUnit.orbitRadius);

                    Projectile newProjectile = projectile.SetupOrbit(projectile, transform.position, theta);

                    PoolManager.instance.SpawnInfinite(newProjectile, projectilePrefab, orbitLocation);
                }
            }
            else
            {
                Vector3 posA = new Vector3(projectile.direction.y, -projectile.direction.x) + transform.position;
                Vector3 posB = new Vector3(-projectile.direction.y, projectile.direction.x) + transform.position;

                var centerDirection = Quaternion.LookRotation((posB - posA).normalized);
                float directionAngle = Angle(projectile.direction);

                for (int i = 0; i < projectile.projectileUnit.numberOfOrbits; i++)
                {

                    float theta = (projectile.projectileUnit.orbitCircle * Mathf.PI / (projectile.projectileUnit.numberOfOrbits + 1)) * (i + 1);

                    float x = Mathf.Sin(theta) * projectile.projectileUnit.orbitRadius;
                    float z = Mathf.Cos(theta) * projectile.projectileUnit.orbitRadius;
                    Vector3 orbitLocation;

                    if ((directionAngle >= 0 && directionAngle < 89) || (directionAngle >= 270 && directionAngle < 360))
                        orbitLocation = new Vector3(0, x, z);
                    else if (directionAngle >= 89 && directionAngle < 90)
                        orbitLocation = new Vector3(0, x, -z);
                    else
                        orbitLocation = new Vector3(0, -x, z);


                    orbitLocation = centerDirection * orbitLocation;

                    Projectile newProjectile = projectile.SetupOrbit(projectile, transform.position, theta);

                    PoolManager.instance.SpawnInfinite(newProjectile, projectilePrefab, orbitLocation);
                }
            }
        }
    }

    private static float Angle(Vector2 p_vector2)
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

    private void InvertRotation()
    {
        if (emitter != null && emitter.projectileUnit.directionSwitch && emitter.projectileUnit.directionSwitchPoint != 0)
        {

            float currentAngle = (emitter.projectileUnit.spreadDistance * emitter.projectileUnit.bulletsAmount) + emitter.projectileUnit.offset;

            if (angle >= (currentAngle + emitter.projectileUnit.directionSwitchPoint))
            {
                rotationSpeed = -emitter.projectileUnit.rotateSpeed;
            }
            else if (angle <= currentAngle)
            {
                rotationSpeed = emitter.projectileUnit.rotateSpeed;
            }
        }
    }

    private void SetFireAngle(Vector2 targetPosition)
    {
        float degrees = Mathf.Atan2(targetPosition.y - transform.position.y, targetPosition.x - transform.position.x) * Mathf.Rad2Deg;

        if (degrees > 0)
            degrees -= 360;
        degrees = degrees * -1;

        if (emitter.projectileUnit.bulletsAmount % 2 != 0)
            angle = (degrees + 90) - (emitter.projectileUnit.spreadDistance * (emitter.projectileUnit.bulletsAmount / 2));
        else
            angle = (degrees + 90) - (emitter.projectileUnit.spreadDistance * (emitter.projectileUnit.bulletsAmount / 2)) + emitter.projectileUnit.spreadDistance / 2;
    }
}
