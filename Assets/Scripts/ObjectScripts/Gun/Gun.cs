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
            Shoot(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

        GunCoolDown();
    }

    public void Shoot(Vector2 dir)
    {
        WeaponCoolDown = 0.3f;
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

    void GunCoolDown()
    {
        if(WeaponCoolDown > 0)
        {
            WeaponCoolDown -= Time.deltaTime;
        }
    }
}
