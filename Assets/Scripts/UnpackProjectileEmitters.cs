using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UnpackProjectileEmitters : MonoBehaviour
{
    public ProjectileHolder projectileHolder;
    public GameObject projectileObjectPrefab;

    private LiveEmitters liveEmitters;
    private int currentGroupCounter;

    public event EventHandler<OnFireEventArgs> OnEmitterTurn;

    /*public ProgressBar ProgressBar;
    public TMP_Text skillText;*/

    public class OnFireEventArgs : EventArgs
    {
        public EmitterInformation currentEmitter;
    }

    void Start()
    {
        ReloadAllEmitters();
        //SelectProjectileGroup();
    }

    private void Awake()
    {
        liveEmitters = new LiveEmitters(projectileHolder);
    }

    public void ReloadAllEmitters()
    {
        for (int i = 0; i < liveEmitters.emittersHolders.Count; i++)
        {
            EmittersHolder emittersHolder = liveEmitters.emittersHolders[i];
            for (int j = 0; j < emittersHolder.emitterInformations.Count; j++)
            {
                //objectPooler.LoadPooler(emittersHolder.emitterInformations[j].projectileUnit.bulletsAmount);
                if (emittersHolder.emitterInformations[j].projectileUnit.numberOfOrbits <= 0)
                    PoolManager.instance.CreatePoolInPosition(projectileObjectPrefab, emittersHolder.emitterInformations[j].projectileUnit.GetPoolSizeApprox(), transform.position);
                else
                    PoolManager.instance.CreatePoolInPosition(projectileObjectPrefab, emittersHolder.emitterInformations[j].projectileUnit.GetOrbitPoolSizeApprox(), transform.position);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchToNextEmitter();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            SelectProjectileGroup(GetNextProjectileGroup(), false);
        }
    }

    public void SwitchToNextEmitter()
    {
        SelectProjectileGroup(GetNextProjectileGroup());
    }

    public void SelectProjectileGroup(int switchTo = -1, bool continueToNextWhenDone = true)
    {
        StopAllCoroutines();

        if (switchTo == -1)
            EmitterTimer(liveEmitters.emittersHolders[GetNextProjectileGroup()]);
        else
        {
            EmitterTimer(liveEmitters.emittersHolders[switchTo], continueToNextWhenDone);
        }
    }

    private void EmitterTimer(EmittersHolder currentHolder, bool continueToNextWhenDone = true)
    {
        for (int i = 0; i < currentHolder.emitterInformations.Count; i++)
        {
            StartCoroutine(runProjectileGroup(currentHolder.emitterInformations[i], currentHolder, continueToNextWhenDone));
        }
    }

    private void CheckEmitterRunTimes(EmittersHolder currentHolder, bool continueToNextWhenDone = true)
    {
        int counter = 0;
        for (int i = 0; i < currentHolder.emitterInformations.Count; i++)
        {
            if (currentHolder.emitterInformations[i].timesRun == currentHolder.timesToRun)
            {
                counter++;
                currentHolder.emitterInformations[i].timesRun = 0;
            }
        }

        if (currentHolder.emitterInformations.Count <= counter && continueToNextWhenDone)
        {
            /*for (int i = 0; i < currentHolder.emitterInformations.Count; i++)
            {
                currentHolder.emitterInformations[i].timesRun = 0;
            }*/
            SelectProjectileGroup();
        }
    }

    private int GetNextProjectileGroup()
    {
        if (currentGroupCounter < liveEmitters.emittersHolders.Count - 1)
        {
            currentGroupCounter++;
        }
        else
        {
            currentGroupCounter = 0;
        }
        return currentGroupCounter;
    }

    IEnumerator runProjectileGroup(EmitterInformation currentEmitter, EmittersHolder currentHolder, bool continueToNextWhenDone = true)
    {
        /*skillText.text = currentEmitter.projectileUnit.name;
        ProgressBar.maximum = currentHolder.timesToRun;
        ProgressBar.current = currentEmitter.timesRun;*/

        if (currentEmitter.timesRun < currentHolder.timesToRun || currentHolder.timesToRun == 0)
        {
            //Debug.Log("Current Emitter: " + currentEmitter.projectileUnit.name);

            //Event to fire currently loaded projectile.
            OnEmitterTurn?.Invoke(this, new OnFireEventArgs { currentEmitter = currentEmitter });
            
            //Wait based on shotspeed
            yield return new WaitForSeconds((1.0f / currentEmitter.projectileUnit.shotsPerSecond) + currentEmitter.projectileUnit.shotSpeedOffset);
            currentEmitter.timesRun++;
            StartCoroutine(runProjectileGroup(currentEmitter, currentHolder, continueToNextWhenDone));
        }
        else
        {
            //Debug.Log("Switching to next holder");
            CheckEmitterRunTimes(currentHolder, continueToNextWhenDone);
        }
    }
}
