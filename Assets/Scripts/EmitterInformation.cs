using System.Collections.Generic;
using UnityEngine;

public class EmitterInformation
{
    public ProjectileUnit projectileUnit;
    public float rotation = 0;
    public bool directionSwitch = false;
    public int timesRun = 0;

    public EmitterInformation (ProjectileUnit _projectileUnit)
    {
        this.projectileUnit = _projectileUnit;
    }
}