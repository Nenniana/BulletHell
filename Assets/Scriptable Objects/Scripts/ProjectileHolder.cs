using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New Projectile Holder", menuName = "Projectiles/Projectile Holder")]
public class ProjectileHolder : ScriptableObject
{
    public List<ProjectileGroup> projectileGroups = new List<ProjectileGroup>();
}
