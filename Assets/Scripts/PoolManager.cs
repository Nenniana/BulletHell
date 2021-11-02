using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public List<GameObject> prefabs = new List<GameObject>();
    public static PoolManager instance = null;

    [SerializeField] 
    private Dictionary<string, Queue<ObjectInstance>> poolDictionary = new Dictionary<string, Queue<ObjectInstance>>();

    void Start()
    {
        instance = this;
    }

    private GameObject GetPrefabByName (string prefabName)
    {
        foreach (GameObject prefab in prefabs)
        {
            if (prefab.name == prefabName)
                return prefab;
        }
        return null;
    }

    public void CreatePoolRe(string prefabName, int poolsize)
    {
        string key = prefabName;

        if (!poolDictionary.ContainsKey(key))
        {
            poolDictionary.Add(key, new Queue<ObjectInstance>());
        }

        for (int i = 0; i < poolsize; i++)
        {
            ObjectInstance newObject = new ObjectInstance(Instantiate(GetPrefabByName(prefabName)) as GameObject);
            poolDictionary[key].Enqueue(newObject);
        }
    }

    public GameObject SpawnRe(Projectile projectile, Vector3 position, string prefabName)
    {

        string key = prefabName;

        if (poolDictionary.ContainsKey(key))
        {
            ObjectInstance objectToReuse;

            if (poolDictionary[key].Count != 0)
            {
                objectToReuse = poolDictionary[key].Dequeue();
            }
            else
            {
                Debug.Log("New object created.");
                objectToReuse = new ObjectInstance(Instantiate(GetPrefabByName(prefabName)) as GameObject);
            }
            objectToReuse.Reuse(projectile, position);
            return objectToReuse.gameObject;
        }
        else
        {
            Debug.Log("Key wasn't recognized");
        }
        return null;
    }

    public void CreatePool(GameObject prefab, int poolsize)
    {
        string key = prefab.name;

        if (!poolDictionary.ContainsKey(key))
        {
            poolDictionary.Add(key, new Queue<ObjectInstance>());
        }

        //GameObject poolHolder = new GameObject(prefab.name + " pool");
        //poolHolder.transform.parent = transform;

        for (int i = 0; i < poolsize; i++)
        {
            ObjectInstance newObject = new ObjectInstance(Instantiate(prefab) as GameObject);
            poolDictionary[key].Enqueue(newObject);
            //newObject.gameObject.transform.parent = poolHolder.transform;
        }
    }

    public void CreatePoolInPosition(GameObject prefab, int poolsize, Vector3 position)
    {
        string key = prefab.name;

        if (!poolDictionary.ContainsKey(key))
        {
            poolDictionary.Add(key, new Queue<ObjectInstance>());
        }

        //GameObject poolHolder = new GameObject(prefab.name + " pool");
        //poolHolder.transform.parent = transform;

        for (int i = 0; i < poolsize; i++)
        {
            ObjectInstance newObject = new ObjectInstance(GameObject.Instantiate(prefab, position, Quaternion.identity) as GameObject);
            poolDictionary[key].Enqueue(newObject);
            //newObject.gameObject.transform.parent = poolHolder.transform;
        }
    }

    public GameObject Spawn(Projectile projectile, string prefabName, Vector3 position)
    {

        string key = prefabName;
        //Debug.Log("key spawn " + key);
        if (poolDictionary.ContainsKey(key))
        {
            ObjectInstance objectToReuse;

            /*if (poolDictionary[key].Count != 0)
            {
                objectToReuse = poolDictionary[key].Dequeue();
            }
            else
            {
                Debug.Log("New object created!");
                objectToReuse = new ObjectInstance(Instantiate(prefab) as GameObject);
            }*/

            //Debug.Log("Key was recognized");
            objectToReuse = poolDictionary[key].Dequeue();
            //poolDictionary[key].Enqueue(objectToReuse);
            objectToReuse.Reuse(projectile, position);
            return objectToReuse.gameObject;
        }
        else
        {
            //Debug.Log("Key wasn't recognized");
        }
        return null;
    }

    public GameObject SpawnInfinite(Projectile projectile, GameObject prefab, Vector3 position)
    {

        string key = prefab.name;

        if (poolDictionary.ContainsKey(key))
        {
            ObjectInstance objectToReuse;

            if (poolDictionary[key].Count != 0)
            {
                objectToReuse = poolDictionary[key].Dequeue();
            }
            else
            {
                //Debug.Log("New object created.");
                objectToReuse = new ObjectInstance(Instantiate(prefab) as GameObject);
            }
            objectToReuse.Reuse(projectile, position);
            return objectToReuse.gameObject;
        }
        else
        {
            Debug.Log("Key wasn't recognized");
        }
        return null;
    }

    public void ReQueueObject (GameObject _objectToReuse, string key)
    {
        //Debug.Log("key reuse " + key);
        if (poolDictionary.ContainsKey(key))
        {
            ObjectInstance objectToReuse = new ObjectInstance(_objectToReuse);
            poolDictionary[key].Enqueue(objectToReuse);
        }
    }

    public class ObjectInstance
    {

        public GameObject gameObject { get; private set; }
        Ipool poolObjectScript;

        public ObjectInstance(GameObject objectInstance)
        {
            gameObject = objectInstance;
            gameObject.SetActive(false);
            poolObjectScript = gameObject.GetComponent<Ipool>();
            poolObjectScript.Initialize();
        }

        public void Reuse(Projectile projectile, Vector3 position)
        {
            poolObjectScript.SpawnObj(projectile, position);
            gameObject.SetActive(true);
        }
    }
}