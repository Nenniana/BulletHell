using System.Collections.Generic;
using UnityEngine;

public class LiveEmitters
{
    public List<EmittersHolder> emittersHolders = new List<EmittersHolder>();

    public LiveEmitters (ProjectileHolder projectileHolder)
    {
        for (int i = 0; i < projectileHolder.projectileGroups.Count; i++)
        {
            EmittersHolder currentHolder = new EmittersHolder();
            ProjectileGroup currentGroup = projectileHolder.projectileGroups[i];
            currentHolder.timesToRun = currentGroup.timesToRun;

            for (int j = 0; j < currentGroup.ProjectileUnits.Count; j++)
            {
                EmitterInformation currentEmitter = new EmitterInformation(currentGroup.ProjectileUnits[j]);
                currentHolder.emitterInformations.Add(currentEmitter);
            }

            this.emittersHolders.Add(currentHolder);
        }
    }
}