using UnityEngine;
using System.Collections;
using System;

public class DefaultWeapon : AWeapon
{
    public Transform BulletSpawnTransform;
 

    public float PrimaryBulletSpeed;
    public float PrimaryCooldown;
    public int PrimaryDamage;
    public float PrimaryOffsetDegree = 15f;

    public int NumSecondaryShoots = 5;
    public float SecondaryCooldown;
    public float SecondaryMinSpeed;
    public float SecondaryMaxSpeed;
    public int SecondaryDamage;
    public float SecondaryOffsetDegree = 45.0f;

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
            
            for (int i = 0; i < 2; i++)
            {
                GameObject spawnedObj = GameObject.Instantiate(Prefab.gameObject);
                BulletBehavior spawnedBullet = spawnedObj.GetComponent<BulletBehavior>();
                spawnedBullet.Speed = PrimaryBulletSpeed;

                Vector3 direction = BulletSpawnTransform.forward;
                direction = Quaternion.Euler(0.0f, PrimaryOffsetDegree * (UnityEngine.Random.value - 0.5f), 0.0f) * direction;

                spawnedBullet.Init(this, BulletSpawnTransform.position, direction);
                spawnedBullet.Damage = PrimaryDamage;
                curCooldown = PrimaryCooldown;
            }
            

            curCooldown = PrimaryCooldown;
        }
    }

    public override void OnRelease(FireMode mode)
    {
        
    }



}
