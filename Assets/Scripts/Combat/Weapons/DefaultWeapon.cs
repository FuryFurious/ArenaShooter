using UnityEngine;
using System.Collections;
using System;

public class DefaultWeapon : AWeapon
{
    public Transform BulletSpawnTransform;
    public float SecondaryOffsetDegree = 45.0f;
    public int NumSecondaryShoots = 5;

    public float PrimaryBulletSpeed;
    public float PrimaryCooldown;
    public int PrimaryDamage;

    public float SecondaryCooldown;
    public float SecondaryMinSpeed;
    public float SecondaryMaxSpeed;
    public int SecondaryDamage;

    [SerializeField]
    private BulletBehavior Prefab;
    private float curCooldown;
    private bool IsReady { get { return curCooldown <= 0.0f; } }

    void Start()
    {
        this.curCooldown = 0.0f;
    }

    void Update()
    {
        if(curCooldown > 0.0f)
            curCooldown -= Time.deltaTime;
    }

    public override void OnClick(FireMode mode)
    {
       

        if (mode == FireMode.Secondary && IsReady)
        {
            for (int i = 0; i < NumSecondaryShoots; i++)
            {
                GameObject spawnedObj = GameObject.Instantiate(Prefab.gameObject);
                BulletBehavior spawnedBullet = spawnedObj.GetComponent<BulletBehavior>();

                spawnedBullet.Speed = UnityEngine.Random.Range(SecondaryMinSpeed, SecondaryMaxSpeed);



                Vector3 direction = BulletSpawnTransform.forward;
                direction = Quaternion.Euler(0.0f, SecondaryOffsetDegree * (UnityEngine.Random.value - 0.5f), 0.0f) * direction;

                spawnedBullet.Damage = SecondaryDamage;
                spawnedBullet.Init(this, BulletSpawnTransform.position, direction);


            }

            curCooldown = SecondaryCooldown;
        }
  
    }

    public override void OnPress(FireMode mode)
    {
        if (mode == FireMode.Primary && IsReady)
        {
            GameObject spawnedObj = GameObject.Instantiate(Prefab.gameObject);
            BulletBehavior spawnedBullet = spawnedObj.GetComponent<BulletBehavior>();
            spawnedBullet.Speed = PrimaryBulletSpeed;
            spawnedBullet.Init(this, BulletSpawnTransform.position, BulletSpawnTransform.forward);
            spawnedBullet.Damage = PrimaryDamage;
            curCooldown = PrimaryCooldown;
        }
    }

    public override void OnRelease(FireMode mode)
    {
        
    }



}
