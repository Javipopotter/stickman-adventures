using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public GameObject[] objects;
    [SerializeField] bool Spawner = true;
    [SerializeField] float probability = 100;

    private void Awake()
    {
        if (Spawner)
        {
            if (RandomBoolean())
            {
                int rand = Random.Range(0, objects.Length);
                Instantiate(objects[rand], transform.position, Quaternion.identity, transform.parent);
            }
            Destroy(gameObject);
        }
        else if(!RandomBoolean())
        {
            Destroy(gameObject);
        }
        else
        {
            Destroy(this);
        }
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
