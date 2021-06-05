using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveArms : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 dir;
    float rotz;
    public bool punch = true;
    public bool armLock;
    [SerializeField] float force;
    [SerializeField] bool OnlyPunches;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        dir = GameManager.Gm.GetMouseVector(gameObject.transform.position);
        rotz = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        if ((Input.GetMouseButton(0) || armLock) && !OnlyPunches)
        {
            rb.MoveRotation(rotz);
        }
    }

    private void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0) && punch)
        {
            dir = transform.InverseTransformDirection(dir);
            rotz = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            NormalizeFloat(rotz);
            rb.AddTorque(NormalizeFloat(rotz) * force);
        }
    }

    float NormalizeFloat(float f)
    {

        if (transform.rotation.z < f)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }
}
        /*if(Input.GetMouseButtonDown(0))
        {
            rb.AddForce(dir * force);
        }*/
