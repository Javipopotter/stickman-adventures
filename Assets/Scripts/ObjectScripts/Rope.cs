using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    GameObject RopeBase;
    [SerializeField] GameObject RopeInstance;
    [SerializeField] GameObject LastRope;
    [SerializeField] bool RandomLength;
    public float rot;
    public float RopeLength;
    public float minLength;
    public float maxLength;

    void Start()   
    {
        if (RandomLength) //You fell for it fool! Thunder cross split attack! 
            RopeLength = Random.Range(minLength, maxLength);
        for (int i = 0; i < RopeLength; i++)
        {
            RopeBase = Instantiate(RopeInstance, LastRope.transform.position - new Vector3(0, 1.25f, 0), transform.rotation, this.transform) as GameObject;
            RopeBase.GetComponent<HingeJoint2D>().connectedBody = LastRope.GetComponent<Rigidbody2D>();
            LastRope = RopeBase;
        }
        transform.eulerAngles = Vector3.forward * rot;
    }

    /*public void PickUpRope(bool t)
    {
        foreach(Balance b in ropeParts)
        {
            if (t)
            {
                b.force = Mathf.Infinity;
            }
            else
            {
                b.force = 0;
            }
        }
    }*/
}
