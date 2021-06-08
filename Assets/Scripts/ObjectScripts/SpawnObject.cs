using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public GameObject[] objects;
    [SerializeField] bool isItThere;
    [SerializeField] float probability;

    private void Awake()
    {
        if (isItThere)
        {
            if (RandomBoolean())
            {
                int rand = Random.Range(0, objects.Length);
                Instantiate(objects[rand], transform.position, Quaternion.identity, transform.parent);
            }
        }
        else
        {
            int rand = Random.Range(0, objects.Length);
            Instantiate(objects[rand], transform.position, Quaternion.identity, transform.parent);
        }
        Destroy(gameObject);
    }

    bool RandomBoolean()
    {
        if (Random.Range(0, 100) <= probability)
        {
            return true;
        }
        return false;
    }
}
