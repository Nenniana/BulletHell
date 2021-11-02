using UnityEngine;
using System;

public class Projectile
{
    public Vector2 startPosition;
    public Vector2 lastPosition;
    public Vector2 midPosition;
    public Vector2 originalDirection;
    public Vector2 direction;
    public Transform centerTransform;
    public Quaternion rotation; // Use?
    public float currentMovementSpeed;
    public ProjectileUnit projectileUnit;
    public float theta = 0;
    public float lifeTime;
    public bool isOrbit;

    public bool moveFoward = false; //Remove
    public bool accelerate = true;
    public bool switchSpeedOnce = true;

    public Projectile SetupProjectileFromProjectile(Projectile _Projectile, float _theta, Transform _centerTransform)
    {
        Projectile currentProjectile = new Projectile(_Projectile.projectileUnit, _Projectile.direction, _Projectile.startPosition);
        currentProjectile.centerTransform = _centerTransform;
        currentProjectile.theta = _theta;
        return currentProjectile;
    }

    public Projectile SetupOrbit(Projectile _projectile, Vector2 _midPosition, float _theta)
    {
        Projectile currentProjectile = new Projectile(_projectile.projectileUnit, _projectile.direction, _projectile.startPosition);
        currentProjectile.isOrbit = true;
        currentProjectile.midPosition = _midPosition;
        currentProjectile.theta = _theta;
        return currentProjectile;
    }

    public Projectile SetupProjectile(Projectile _Projectile, float _theta)
    {
        Projectile currentProjectile = new Projectile(_Projectile.projectileUnit, _Projectile.direction, _Projectile.startPosition);
        currentProjectile.isOrbit = true;
        currentProjectile.theta = _theta;
        return currentProjectile;
    }

    public Projectile (ProjectileUnit _projectileUnit, Vector2 _direction, Vector2 _startLocation)
    {
        this.originalDirection = _direction;
        this.direction = _direction;
        this.startPosition = _startLocation;
        this.lastPosition = _startLocation;
        this.lifeTime = _projectileUnit.lifeTime;
        this.currentMovementSpeed = _projectileUnit.projectileSpeed;
        this.projectileUnit = _projectileUnit;
    }
}