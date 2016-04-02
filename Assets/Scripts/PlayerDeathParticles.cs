using UnityEngine;
using System.Collections;
using System;

public class PlayerDeathParticles : MonoBehaviour {

    public ParticleSystem myParticles;
    public Rigidbody myRigidBody;

    private bool running = false;

    // Use this for initialization
    public void Init (Color color, Vector3 velocity)
    {
        Renderer renderer = myParticles.GetComponent<Renderer>();
        renderer.material.color = color;

        myRigidBody = gameObject.GetComponent<Rigidbody>();

        myRigidBody.velocity = velocity;

        myParticles.Play();

        running = true;
    }

    void Update()
    {
        if (running && !myParticles.isPlaying)
        {
            Destroy(gameObject);
        }
         
    }

}
