using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler pool;
    public GameObject[] ObjectsToPool;
    public List<List<GameObject>> PooledObjects = new List<List<GameObject>>();
    List<GameObject> auxiliaryList = new List<GameObject>();

    private void Awake()
    {
        pool = this;
    }

    void Start()
    {
        foreach (GameObject objectToPool in ObjectsToPool)
        {
            GameObject tmp;
            for (int i = 0; i < 20; i++)
            {
                tmp = Instantiate(objectToPool);
                tmp.SetActive(false);
                auxiliaryList.Add(tmp);
            }
            PooledObjects.Add(auxiliaryList);
            auxiliaryList = new List<GameObject>();
        }
    }

    public GameObject GetPooledObject(int index)
    {
        foreach(GameObject g in PooledObjects[index])
        {
            if(g.activeInHierarchy == false)
            {
                g.SetActive(true);
                return g;
            }
        }
        return null;
    }
}
