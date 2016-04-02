using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class CharacterImpulse : MonoBehaviour {

    public int PlayerIndex = -1;
    public MeshRenderer MyRenderer;
    public Rigidbody MyRigidBody;

    private PlayerInfo info;
    private HealthManager relatedHealthManager;

    public float MaxVelocity = 10.0f;
    public float DamageFactor = 25.0f;

    private Vector3 startPos;

    public void Start()
    {
        WorldManager.Instance.RegisterCharacterImpulse(this);
        startPos = gameObject.transform.position;
    }

    public void Init()
    {
        info = WorldManager.Instance.GetPlayerInfo(PlayerIndex);

        if(info == null)
        {
            gameObject.SetActive(false);
        }

        else
        {
            gameObject.SetActive(true);
            MyRenderer.material.color = info.Color;
            relatedHealthManager = info.HealthManager;
        }
       
    }

    public void Reset()
    {
        gameObject.transform.position = startPos;
        MyRigidBody.velocity = Vector3.zero;
    }

    void OnCollisionEnter(Collision other)
    {
        PlayerControllerRef refe = other.collider.gameObject.GetComponent<PlayerControllerRef>();

        if (refe)
        {
            Vector3 toPlayer = other.collider.gameObject.transform.position - gameObject.transform.position;
            refe.Controller.AddForce(Vector3.Reflect(MyRigidBody.velocity, toPlayer.normalized), false);
        }


        HealthManager manager = other.collider.gameObject.GetComponent<HealthManager>();

        if (manager && relatedHealthManager != manager)
        {
            int damage = (int)(DamageFactor * MyRigidBody.velocity.magnitude);

            manager.ChangeHealth(-damage, info);
        }

    }

    void Update()
    {
        if(MyRigidBody.velocity.magnitude > MaxVelocity)
        {
            MyRigidBody.velocity = MyRigidBody.velocity.normalized * MaxVelocity;
        }
    }
}
