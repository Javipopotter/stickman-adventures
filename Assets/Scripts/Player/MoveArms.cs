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
    public float force;
    [SerializeField]float rotOffset;
    [SerializeField] bool OnlyPunches;
    // Start is called before the first frame update
    void Awake()
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
            rb.MoveRotation(rotz + rotOffset);
        }
    }

    private void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0) && punch)
        {
            Punch(SoundManager.SoundMan.SwordSwings);
        }
    }

    public void Punch(AudioClip[] sound)
    {
        dir = transform.InverseTransformDirection(dir);
        rotz = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        NormalizeFloat(rotz);
        SoundManager.SoundMan.PlaySound(sound);
        rb.AddTorque(NormalizeFloat(rotz) * force);
    }

    public void Punch(float dir, AudioClip[] sound)
    {
        SoundManager.SoundMan.PlaySound(sound);
        rb.AddTorque(force * NormalizeFloat(dir));
    }

    float NormalizeFloat(float f)
    {

        if (transform.rotation.z <= f)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }
}
