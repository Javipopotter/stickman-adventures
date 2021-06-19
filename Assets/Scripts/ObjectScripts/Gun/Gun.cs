using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : PickableObject
{
    public GameObject Bullet;
    Rigidbody2D InstantiatedBulletRb;
    GameObject InstantiatedBullet;
    [SerializeField] GameObject Zubroska;
    [SerializeField]float BulletForce;
    public override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        if(Input.GetMouseButton(0) && WeaponCoolDown <= 0 && Holded && !PickedByAI)
        {
            Shoot();
        }

        GunCoolDown();
    }

    public void Shoot()
    {
        WeaponCoolDown = 0.3f;
        InstantiatedBullet = ObjectPooler.pool.GetPooledObject(0);
        if(PickedByAllie)
            InstantiatedBullet.gameObject.layer = 9;
        else
            InstantiatedBullet.gameObject.layer = 6;
        InstantiatedBulletRb = InstantiatedBullet.GetComponent<Rigidbody2D>();
        InstantiatedBullet.transform.SetPositionAndRotation(Zubroska.transform.position, Zubroska.transform.rotation);
        InstantiatedBullet.GetComponent<Bullet>().PickedByAI = PickedByAI;
        InstantiatedBulletRb.velocity = Zubroska.transform.right * BulletForce;
    }

    void GunCoolDown()
    {
        if(WeaponCoolDown > 0)
        {
            WeaponCoolDown -= Time.deltaTime;
        }
    }
}
