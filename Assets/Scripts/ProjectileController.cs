using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ProjectileController : ProjectileBase, Ipool
{
    public GameObject projectileOrbitPrefab;
    public GameObject projectileObjectPrefab;
    private List<GameObject> centerObjects;

    public bool active { get; protected set; }
    public void Initialize()
    {
        active = false;
        //Debug.Log("Is initialized!");
    }

    public void SpawnObj (Projectile _projectile, Vector3 startLocation)
    {
        //transform.position = startLocation;

        projectile = _projectile;

        transform.position = projectile.startPosition;

        SetRenderer();

        InitializeOrbitProjectiles();

        active = true;
        Invoke("StartMovement", projectile.projectileUnit.shotSpeedOffset);

        DisableObjects(gameObject, projectile.lifeTime);
    }

    public override void OnDisable()
    {
        if (projectile != null && projectile.projectileUnit.ProjectileOrbits)
            for (int i = 0; i < centerObjects.Count; i++)
            {
                if (centerObjects[i] != null)
                    centerObjects[i].GetComponent<ProjectileOrbit>().projectile.moveFoward = true;
            }
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            SwitchSpeedBasedOnDistanceTraveled();

            if (projectile.projectileUnit.followMouse)
            {
                if (GetDistancedTraveled(projectile.startPosition) >= projectile.projectileUnit.followAfterDistance || !projectile.moveFoward)
                    projectile.moveFoward = FollowMouse();
            }

            MoveFoward();

            RotateSelf();

            ChangeSpeedGradually();
        }
    }

    public void DestroyIfNoOrbits () // Remove?
    {
        int notNull = centerObjects.Count(s => s != null);

        if (notNull == 1 && projectile.projectileUnit.hideOriginalProjectile)
            DestroyObjects(gameObject);
    }
    
    //transform.RotateAround(transform.position, projectile.projectileDirection, 180 * Time.deltaTime); - Evolves on own axis.
    //transform.RotateAround(transform.position, new Vector2( 1, 1), 180 * Time.deltaTime); //Cool effect.

    public bool FollowMouse ()
    {
        Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (GetDistancedTraveled(target) <= projectile.projectileUnit.followFromDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, projectile.currentMovementSpeed * Time.deltaTime);

            if (projectile.projectileUnit.MouseAsLastDirection)
            {
                projectile.direction = target;
            }
            else
            {
                projectile.direction = projectile.originalDirection;
            }

            return false;
        }

        return true;
    }

    public void SwitchSpeedBasedOnDistanceTraveled ()
    {
        if (projectile.projectileUnit.projectileSpeed != 0)
            if (projectile.switchSpeedOnce)
            {
                if ((projectile.currentMovementSpeed == projectile.projectileUnit.projectileSpeed && GetDistancedTraveled(projectile.lastPosition) >= projectile.projectileUnit.switchSpeedDistance) || (projectile.currentMovementSpeed == projectile.projectileUnit.switchSpeedSpeed && GetDistancedTraveled(projectile.lastPosition) >= projectile.projectileUnit.switchSpeedDistanceSecond))
                {
                    projectile.lastPosition = transform.position;

                    if (projectile.projectileUnit.switchSpeedsContinue)
                        projectile.currentMovementSpeed = SwitchBetweenSpeeds();
                    else
                    {
                        projectile.currentMovementSpeed = projectile.projectileUnit.switchSpeedSpeed;
                        projectile.switchSpeedOnce = false;
                    }
                }
            }
    }

    public float SwitchBetweenSpeeds ()
    {
        if (projectile.currentMovementSpeed == projectile.projectileUnit.projectileSpeed)
            return projectile.projectileUnit.switchSpeedSpeed;
        else
            return projectile.projectileUnit.projectileSpeed;
    }

    private void StartMovement()
    {
        projectile.moveFoward = true;
    }

    private void SetRenderer ()
    {
        SpriteRenderer spriteRenderer = transform.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = projectile.projectileUnit.projectileObject.projectileSprite;
        spriteRenderer.enabled = true;

        if (projectile.projectileUnit.hideOriginalProjectile)
        {
            spriteRenderer.sprite = null;
            gameObject.GetComponent<Collider2D>().enabled = false;
        }
    }

    private void OnDestroy()
    {
        Debug.Log("I was destroyed!");
        ExplodeAnimation();
    }  

    public void InitializeOrbitProjectiles()
    {
        if (projectile.projectileUnit.ProjectileOrbits)
        {
            //PoolManager.instance.CreatePool(projectile.projectileUnit, projectileOrbitPrefab, projectile.projectileUnit.GetOrbitPoolSizeApprox());
            centerObjects = new List<GameObject>();

            if (projectile.projectileUnit.rotate)
            {

                for (int i = 0; i < projectile.projectileUnit.numberOfOrbits; i++)
                {
                    float theta = (projectile.projectileUnit.orbitCircle * Mathf.PI / projectile.projectileUnit.numberOfOrbits) * i;

                    Vector2 orbitLocation = new Vector2(Mathf.Sin(theta) * projectile.projectileUnit.orbitRadius, Mathf.Cos(theta) * projectile.projectileUnit.orbitRadius);

                    Projectile newProjectile = projectile.SetupProjectileFromProjectile(projectile, theta, transform);

                    centerObjects.Add(PoolManager.instance.Spawn(newProjectile, "ProjectileOrbit", orbitLocation));

                    /*centerObjects.Add(Instantiate(projectileOrbitPrefab, orbitLocation, transform.rotation));

                    centerObjects[i].GetComponent<ProjectileOrbit>().Initialize(projectile, theta, orbitLocation, transform);*/
                    //DestroyObjects(centerObjects[i], 20);
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

                    float theta = (projectile.projectileUnit.orbitCircle * Mathf.PI / projectile.projectileUnit.numberOfOrbits) * i;

                    float x = Mathf.Sin(theta) * projectile.projectileUnit.orbitRadius;
                    float z = Mathf.Cos(theta) * projectile.projectileUnit.orbitRadius;
                    Vector3 orbitLocation;

                    if ((directionAngle >= 0 && directionAngle < 90) || (directionAngle >= 271 && directionAngle < 360))
                        orbitLocation = new Vector3(0, x, z);
                    else if (directionAngle < 91)
                        orbitLocation = new Vector3(x, 0, z);
                    else if (directionAngle < 270)
                        orbitLocation = new Vector3(0, -x, z);
                    else
                        orbitLocation = new Vector3(-x, 0, z);

                    orbitLocation = centerDirection * orbitLocation;

                    Projectile newProjectile = projectile.SetupProjectileFromProjectile(projectile, theta, transform);

                    centerObjects.Add(PoolManager.instance.Spawn(newProjectile, "ProjectileOrbit", orbitLocation));

                    /*centerObjects.Add(Instantiate(projectileOrbitPrefab, transform.position + pos, transform.rotation));

                    centerObjects[i].GetComponent<ProjectileOrbit>().Initialize(projectile, angle, pos, transform);*/
                    //DestroyObjects(centerObjects[i], 20); //Change to onenable

                }
            }
        }
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            if(!projectile.projectileUnit.ProjectileOrbits && !projectile.projectileUnit.hideOriginalProjectile)
            {
                if (projectile.projectileUnit.wallBounce)
                {
                    Vector2 inNormal = collision.contacts[0].normal;
                    projectile.direction = Vector2.Reflect(projectile.direction, inNormal);
                }
                else
                {
                    //DestroyObjects(gameObject);
                    ExplodeAnimation();
                }
            }

        }
    }
}
