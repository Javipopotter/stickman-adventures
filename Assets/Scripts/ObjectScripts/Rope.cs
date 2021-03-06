using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    public GameObject RopeBase;
    [SerializeField] GameObject RopeInstance;
    [SerializeField] GameObject LastRope;
    [SerializeField] bool RandomLength;
    [SerializeField] bool DoubleHolded;
    public float rot;
    public float RopeLength;
    public float minLength;
    public float maxLength;

    void Awake()   
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
        if(DoubleHolded)
        {
            LastRope.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
        }
        StartCoroutine(AutoconfigureConnectedAnchorIsAShit());
    }

    IEnumerator AutoconfigureConnectedAnchorIsAShit()
    {
        yield return new WaitForEndOfFrame();
        for(int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).TryGetComponent(out FixedJoint2D fj))
            {
                fj.autoConfigureConnectedAnchor = false; 
            }
        }
    }
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
