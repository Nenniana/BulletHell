using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "New Projectile Unit", menuName = "Projectiles/Projectile Unit")]
public class ProjectileUnit : ScriptableObject
{
    [Header("General Settings")]
    public int bulletsAmount = 2;
    public float spreadDistance = 20f;
    public int shotsPerSecond = 8;
    public float shotSpeedOffset = 0;
    public float rotateSpeed = 200;
    public float lifeTime = 10;
    public float lifeDistance = 30;
    public ProjectileObject projectileObject;

    [Header("Placement Settings")]
    public float offset = 0;

    [Header("Mouse Settings")]
    public bool mouseTarget = false;

    [Header("Mouse Settings")]
    public bool playerTarget = false;

    [Header("Direction Settings")]
    public bool directionSwitch = false;
    public float directionSwitchPoint = 180;

    [Header("Projectile General Settings")]
    public float projectileSpeed = 2f;
    public bool rotate;

    [Header("Projectile Hard Speed Switching Settings")]
    //Add list of speed settings so switching between more than two can be enabled.
    public bool switchSpeedsContinue = false;
    public float switchSpeedSpeed = 1;
    public float switchSpeedDistance = 2;
    public float switchSpeedDistanceSecond = 2;

    [Header("Projectile Gradual Speed Switching Settings")]
    public float speedUpper = 0;
    public float speedLower = -0;
    public float acceleration = 1;

    [Header("Projectile Mouse Settings")]
    public bool followMouse = false;
    public bool MouseAsLastDirection = false;
    public float followAfterDistance = 5;
    public float followFromDistance = 2;

    [Header("Projectile Bounce Settings")]
    public bool wallBounce = false;

    [Header("Projectile Pattern Settings")]
    public bool ProjectileOrbits = false;
    public float orbitRoationSpeed;
    public float orbitRadius;
    public float orbitCircle;
    public int numberOfOrbits;
    public bool hideOriginalProjectile;

    [Header("Misc. Settings")]
    public float weaponRange = 50f;
    public float hitForce = 100f;

    public int GetPoolSizeApprox ()
    {
        return shotsPerSecond * bulletsAmount * (((int)lifeTime / 30));
    }

    public int GetOrbitPoolSizeApprox()
    {
        return shotsPerSecond * bulletsAmount * numberOfOrbits * (int)lifeTime / 30;
    }
}