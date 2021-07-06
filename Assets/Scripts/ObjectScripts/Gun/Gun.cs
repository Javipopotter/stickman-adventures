using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : PickableObject
{
    public GameObject Bullet;
    Rigidbody2D InstantiatedBulletRb;
    GameObject InstantiatedBullet;
    public float cadence = 0.3f;
    [SerializeField] GameObject Zubroska;
    [SerializeField]float BulletForce;
    public override void Awake()
    {
        base.Awake();
    }

    void Update()
    {
        GunCoolDown();
    }

    public void Shoot(Vector2 dir)
    {
        if (WeaponCoolDown <= 0)
        {
            WeaponCoolDown = cadence;
            InstantiatedBullet = ObjectPooler.pool.GetPooledObject(0);
            InstantiatedBullet.layer = gameObject.layer;
            InstantiatedBulletRb = InstantiatedBullet.GetComponent<Rigidbody2D>();
            InstantiatedBullet.transform.SetPositionAndRotation(Zubroska.transform.position, transform.rotation);
            InstantiatedBullet.GetComponent<Bullet>().holder = Holder;
            InstantiatedBullet.GetComponent<Bullet>().PickedByAI = PickedByAI;
            dir = new Vector2(InstantiatedBullet.transform.position.x, InstantiatedBullet.transform.position.y) - dir;
            dir.Normalize();
            InstantiatedBulletRb.velocity = -dir * BulletForce; 
        }
    }

    void GunCoolDown()
    {
        if(WeaponCoolDown > 0)
        {
            WeaponCoolDown -= Time.deltaTime;
        }
    }
}
