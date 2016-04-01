using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class BulletBehavior : AWeaponUse 
{
    public enum RemovePolicy { None, Distance, Lifetime }
    public RemovePolicy DeletePolicy;

    public float PolicyValue = 1.0f;
    public float Speed = 5.0f;

    public bool RotateToDirection;
    public bool RemoveOnCollision;
    public int Damage;

    private float curPolicyValue;

    private Vector3 direction;
    private Rigidbody myRigidBody;

    void Update() 
    {
        if (DeletePolicy == RemovePolicy.Distance)
            curPolicyValue -= Time.deltaTime * Speed;

        else if (DeletePolicy == RemovePolicy.Lifetime)
            curPolicyValue -= Time.deltaTime;

        if (curPolicyValue <= 0.0f)
            Destroy(gameObject);
    }

    public override void Init(AWeapon spawner, Vector3 spawnPos, Vector3 lookDirection)
    {
        if (myRigidBody == null)
            myRigidBody = GetComponent<Rigidbody>();

        if(RotateToDirection)
            gameObject.transform.forward = lookDirection;

        gameObject.layer = spawner.gameObject.layer;

        gameObject.transform.position = spawnPos;
        myRigidBody.velocity = lookDirection * Speed;

        if (DeletePolicy == RemovePolicy.None)
            curPolicyValue = 1.0f;

        else
            curPolicyValue = PolicyValue;
    }


    void OnTriggerEnter(Collider other)
    {
        HealthManager healthManager = other.GetComponent<HealthManager>();

        if (healthManager)
        {
            healthManager.ChangeHealth(-Damage);
        }

        if (RemoveOnCollision)
            Destroy(gameObject);
    }

}
