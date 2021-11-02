using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public GameObject projectilePrefab;

    private Queue<GameObject> projectilePool = new Queue<GameObject>();

    public void LoadPooler(int poolSize = 1)
    {

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(projectilePrefab);
            obj.SetActive(false);
            projectilePool.Enqueue(obj);
        }
    }

    public void SpawnFromPool (ProjectileUnit projectileUnit, Vector3 spawnPosition, Quaternion spawnRotation, Vector2 moveDirection)
    {
        GameObject objectToSpawn = projectilePool.Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = spawnPosition;
        objectToSpawn.transform.rotation = spawnRotation;
        //objectToSpawn.GetComponent<ProjectileController>().Initialize(projectileUnit, moveDirection);
    }

}
