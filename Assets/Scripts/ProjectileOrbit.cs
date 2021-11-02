using Unity.Mathematics;
using UnityEngine;

public class ProjectileOrbit : ProjectileBase, Ipool
{
    private Vector3 position;
    public bool active { get; protected set; }
    public void Initialize()
    {
        active = false;
        //Debug.Log("Is initialized!");
    }

    public void SpawnObj(Projectile _projectile, Vector3 _position)
    {
        //Debug.Log("Is Spawned!");
        projectile = _projectile;
        position = _position;
        transform.GetComponent<SpriteRenderer>().sprite = projectile.projectileUnit.projectileObject.projectileSprite;
        DisableObjects(gameObject, projectile.lifeTime);
        active = true;
    }

    // Start is called before the first frame update
/*    public void Initialize(Projectile _projectile, float _theta, Vector3 _position, Transform _centerTransform)
    {
        position = _position;
        projectile = _projectile.SetupProjectileFromProjectile(_projectile, _theta);
        transform.GetComponent<SpriteRenderer>().sprite = projectile.projectileUnit.projectileObject.projectileSprite;
    }*/

    // Update is called once per frame
    void Update()
    {
        if (active)
            if (!projectile.moveFoward)
                OrbitSource();
            else
                MoveFoward();
    }

    public override void OnDisable()
    {
        
    }

    private void FixedUpdate()
    {
        
    }

    public void OrbitSource()
    {
        if (projectile.centerTransform != null)
        {
            if (!projectile.projectileUnit.rotate)
                transform.position = projectile.centerTransform.position + position;
            else
            {
                Vector3 offset = new Vector3(Mathf.Sin(projectile.theta) * projectile.projectileUnit.orbitRadius, Mathf.Cos(projectile.theta) * projectile.projectileUnit.orbitRadius);

                transform.position = projectile.centerTransform.position + offset;

                projectile.theta += projectile.projectileUnit.orbitRoationSpeed * Time.deltaTime;
            }
        }
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Obstacle"))
        {

            if (projectile.projectileUnit.wallBounce)
            {
                
                Vector2 inNormal = collision.contacts[0].normal;
                projectile.direction = Vector2.Reflect(transform.position, inNormal);
                projectile.currentMovementSpeed = projectile.currentMovementSpeed * 0.8f;
                projectile.moveFoward = true;
            }
            else
            {
                projectile.centerTransform.GetComponent<ProjectileController>().DestroyIfNoOrbits(); // Remove maybe?

                ExplodeAnimation();
            }
        }
    }
}
