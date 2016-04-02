using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class BulletBehavior : AWeaponUse 
{
    public float DecalLifeTime = 1.0f;
    public float LifeTime = 1.0f;
    public float Speed = 5.0f;

    public bool RotateToDirection;
    public bool RemoveOnCollision;
    public bool GravityOnCollision;
    public int Damage;

    public MeshRenderer BulletRenderer;

    private float lifeTime;
    private float decalLifeTime;

    private Vector3 direction;
    public Rigidbody MyRigidBody;

    private bool collided = false;

    private AWeapon spawner;

    void Update() 
    {
        if (!collided)
        {
            lifeTime -= Time.deltaTime;
            if (lifeTime <= 0.0f)
            {
                Destroy(gameObject);
            }
        }

        else
        {
            if (MyRigidBody.velocity == Vector3.zero)
            {
                gameObject.layer = LayerMask.NameToLayer("None");
                MyRigidBody.isKinematic = true;
                gameObject.isStatic = true;
            }
            

            decalLifeTime -= Time.deltaTime;
            float t = decalLifeTime / DecalLifeTime;

            if (t >= 0.0f)
            {
               // Color color = BulletRenderer.material.color;
               // color.a = t;
               // BulletRenderer.material.color = color;
            }

            else
                Destroy(gameObject);
        }
          
    }

    public override void Init(AWeapon spawner, Vector3 spawnPos, Vector3 lookDirection)
    {
        BulletRenderer.material.color = spawner.OwningPlayer.Color * new Color(0.75f, 0.75f, 0.75f, 1.0f);

        this.spawner = spawner;

        if(RotateToDirection)
            gameObject.transform.forward = lookDirection;

        gameObject.layer = spawner.gameObject.layer;

        this.direction = lookDirection;

        gameObject.transform.position = spawnPos;
        MyRigidBody.velocity = Vector3.zero;
        MyRigidBody.AddForce(lookDirection * Speed, ForceMode.VelocityChange);

        lifeTime = LifeTime;
    }


    void OnCollisionEnter(Collision other)
    {
        if (!collided)
        {
            if (GravityOnCollision)
                MyRigidBody.useGravity = true;
     
            HealthManager healthManager = other.collider.GetComponent<HealthManager>();

            if (healthManager)
            {
                healthManager.ChangeHealth(-Damage, spawner.OwningPlayer);
            }

            PlayerControllerRef controllerRef = other.collider.GetComponent<PlayerControllerRef>();

            if (controllerRef)
            {         
                controllerRef.Controller.AddForce(direction * Speed * 0.33f, healthManager && healthManager.Health == 0);
      
            }

            if (RemoveOnCollision)
                Destroy(gameObject);


            decalLifeTime = DecalLifeTime;
            collided = true;

            gameObject.layer = LayerMask.NameToLayer("Decal");
        }
            
             
    }

}
