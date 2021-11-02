using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler2 : MonoBehaviour
{

    public GameObject projectilePrefab;
    public int pooledAmount;
    public bool willGrow = true;

    private List<GameObject> pooledObjects = new List<GameObject>();

    public void LoadPooler (int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject projectile = Instantiate(projectilePrefab);
            projectile.SetActive(false);
            pooledObjects.Add(projectile);
        }
    }

    public GameObject GetPooledObject ()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (pooledObjects[i] != null && !pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        if (willGrow)
        {
            GameObject projectile = Instantiate(projectilePrefab);
            pooledObjects.Add(projectile);
            return projectile;
        }

        return null;
    }

    public void SpawnFromPool(Projectile _projectile, Vector3 spawnPosition)
    {
        GameObject objectToSpawn = GetPooledObject();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = spawnPosition;
        objectToSpawn.GetComponent<ProjectileController>().SpawnObj(_projectile, spawnPosition);
    }
}
