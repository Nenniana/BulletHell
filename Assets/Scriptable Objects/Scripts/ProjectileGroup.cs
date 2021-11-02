using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New Projectile Group", menuName = "Projectiles/Projectile Group")]
public class ProjectileGroup : ScriptableObject
{
    //List containing inventory rooms.
    public List<ProjectileUnit> ProjectileUnits = new List<ProjectileUnit>();
    public int timesToRun = 10;
}
