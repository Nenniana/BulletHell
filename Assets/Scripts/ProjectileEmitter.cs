using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ProjectileEmitter : MonoBehaviour
{

    public ProjectileHolder projectileHolder;
    public GameObject projectileObjectPrefab;
    public GameObject projectileOrbitPrefab;
    private LiveEmitters liveEmitters;
    private float angle = 0;
    private int currentGroupCounter = 0;
    private ObjectPooler2 objectPooler;

    void Start()
    {
        objectPooler = GameObject.FindGameObjectWithTag("ProjectilePool").GetComponent<ObjectPooler2>();
        liveEmitters = new LiveEmitters(projectileHolder);
        reloadAllEmitters();
        SelectProjectileGroup();
    }

    public void reloadAllEmitters()
    {
        for (int i = 0; i < liveEmitters.emittersHolders.Count; i++)
        {
            EmittersHolder emittersHolder = liveEmitters.emittersHolders[i];
            for (int j = 0; j < emittersHolder.emitterInformations.Count; j++)
            {
                //objectPooler.LoadPooler(emittersHolder.emitterInformations[j].projectileUnit.bulletsAmount);
                
                PoolManager.instance.CreatePool(projectileObjectPrefab, emittersHolder.emitterInformations[j].projectileUnit.GetPoolSizeApprox());

                if (emittersHolder.emitterInformations[j].projectileUnit.numberOfOrbits >= 1)
                    PoolManager.instance.CreatePool(projectileOrbitPrefab, emittersHolder.emitterInformations[j].projectileUnit.GetOrbitPoolSizeApprox());
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

    public void SwitchToNextEmitter ()
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

    private void CheckEmitterRunTimes (EmittersHolder currentHolder, bool continueToNextWhenDone = true)
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

    private int GetNextProjectileGroup ()
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
        if (currentEmitter.timesRun < currentHolder.timesToRun || currentHolder.timesToRun == 0 || projectileHolder.projectileGroups.Count == 1)
        {
            Fire(currentEmitter);
            currentEmitter.timesRun++;
            yield return new WaitForSeconds((1.0f / currentEmitter.projectileUnit.shotsPerSecond) + currentEmitter.projectileUnit.shotSpeedOffset);
            StartCoroutine(runProjectileGroup(currentEmitter, currentHolder, continueToNextWhenDone));
        }
        else
        {
            CheckEmitterRunTimes(currentHolder, continueToNextWhenDone);
        }
    }

    public void Fire(EmitterInformation currentEmitter)
    {
        if (currentEmitter.projectileUnit.mouseTarget)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            angle = SetFireAngle(currentEmitter, mousePosition);
        }
        else if (currentEmitter.projectileUnit.playerTarget && GameObject.FindGameObjectWithTag("Player") != null)
        {
            Vector2 playerPosition = GameObject.FindGameObjectWithTag("Player").gameObject.transform.position;
            angle = SetFireAngle(currentEmitter, playerPosition);
        }
        else
        {
            SetSpreadRotation(currentEmitter);
            angle = transform.eulerAngles.y + currentEmitter.rotation;
        }

        //SetAngleWithinRange(angle);

        for (int i = 0; i < currentEmitter.projectileUnit.bulletsAmount; i++)
        {
            //Debug.Log("Shots fired times: " + counter);
            //Debug.Log("Angle is: " + angle + " Anglestep is: " + spreadDistance);
            float bulletDirectionX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f + currentEmitter.projectileUnit.offset);
            float bulletDirectionY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f + currentEmitter.projectileUnit.offset);

            Vector3 bulletMoveVector = new Vector3(bulletDirectionX, bulletDirectionY, 0f);
            Vector3 bulletDirection = (bulletMoveVector - transform.position).normalized;

            //objectPooler.SpawnFromPool(currentEmitter.projectileUnit, transform.position, transform.rotation, bulletDirection);

            //objectPooler.SpawnFromPool(new Projectile(currentEmitter.projectileUnit, bulletDirection, transform.position), transform.position);

            Projectile newProjectile = new Projectile(currentEmitter.projectileUnit, bulletDirection, transform.position);

            PoolManager.instance.Spawn(newProjectile, "ProjectileObject", transform.position);
            angle += currentEmitter.projectileUnit.spreadDistance;
        }

        //objectPooler.LoadPooler(currentEmitter.projectileUnit.bulletsAmount);

    }

    private void SetAngleWithinRange (float angle)
    {
        Debug.Log("angle before: " + angle);
        Debug.Log("angle after: " + 0 + (angle % 360));
    }

    private void SetSpreadRotation (EmitterInformation currentEmitter)
    {
        if (currentEmitter.projectileUnit.rotateSpeed != 0)
        {
            if (currentEmitter.projectileUnit.directionSwitch)
            {
                if (currentEmitter.rotation >= currentEmitter.projectileUnit.directionSwitchPoint)
                {
                    currentEmitter.rotation -= (currentEmitter.rotation % currentEmitter.projectileUnit.directionSwitchPoint);
                    currentEmitter.directionSwitch = true;
                    //Debug.Log("Current Rotation: " + currentRotation);
                }
                else if (currentEmitter.rotation <= 0)
                {
                    currentEmitter.rotation = 0 + (currentEmitter.rotation % currentEmitter.projectileUnit.directionSwitchPoint);
                    currentEmitter.directionSwitch = false;
                }
            }
            else if (currentEmitter.rotation >= 360)
            {
                currentEmitter.rotation = 0 + (currentEmitter.rotation % 360);
            }

            if (currentEmitter.directionSwitch)
                currentEmitter.rotation -= currentEmitter.projectileUnit.rotateSpeed * (currentEmitter.projectileUnit.spreadDistance * currentEmitter.projectileUnit.bulletsAmount);
            else
                currentEmitter.rotation += currentEmitter.projectileUnit.rotateSpeed * (currentEmitter.projectileUnit.spreadDistance * currentEmitter.projectileUnit.bulletsAmount);

            //Debug.Log("Current Rotation: " + currentRotation);

        }
    }

    public float SetFireAngle (EmitterInformation currentEmitter, Vector2 targetPosition)
    {
        float degrees = Mathf.Atan2(targetPosition.y - transform.position.y, targetPosition.x - transform.position.x) * Mathf.Rad2Deg;

        if (degrees > 0)
            degrees -= 360;

        degrees = degrees * -1;
        if (IsOdd(currentEmitter.projectileUnit.bulletsAmount))
            angle = (degrees + 90) - (currentEmitter.projectileUnit.spreadDistance * (currentEmitter.projectileUnit.bulletsAmount / 2));
        else
            angle = (degrees + 90) - (currentEmitter.projectileUnit.spreadDistance * (currentEmitter.projectileUnit.bulletsAmount / 2)) + currentEmitter.projectileUnit.spreadDistance / 2;

        return angle;
    }

    public static bool IsOdd(int value)
    {
        return value % 2 != 0;
    }
}
