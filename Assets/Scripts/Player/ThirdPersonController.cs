using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonController : MonoBehaviour 
{
    public float ForcePercentZero = 0.15f;
    public float GravityScale = 1.0f;
    public float Damping = 0.5f;
    public bool UseGravity = true;
    public int MaxJumpCount = 1;

    private int jumpCount = 0;
    private CharacterController characterController;

    private Vector3 force;
    private Vector3 movement;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    public void SetForce(Vector3 force)
    {
        SetForce(force.x, force.y, force.z);
    }

    public void SetForce(float x, float y, float z)
    {
        force.x = x;
        force.y = y;
        force.z = z;
    }

    public void SetMovement(float x, float z)
    {
        this.movement.x = x;
        this.movement.z = z;
    }


    public void SetMovement(float x, float y, float z)
    {
        this.movement.x = x;
        this.movement.y = y;
        this.movement.z = z;
    }

    public void SetMovement(Vector3 movement)
    {
        SetMovement(movement.x, movement.y, movement.z);
    }


    public void AddForce(Vector3 force)
    {
        AddForce(force.x, force.y, force.z);
    }

    public void AddForce(float x, float y, float z)
    {
        this.force.x += x;
        this.force.y += y;
        this.force.z += z;
    }

    public Vector3 GetForce()
    {
        return this.force;
    }

    public void Jump(float jumpForce)
    {
        if (jumpCount < MaxJumpCount)
        {
            this.movement.y = jumpForce;
            jumpCount++;
        } 
    }

    void Update()
    {

        Vector3 finalMovement = new Vector3();

        float forceLength = force.magnitude;

        if(movement.sqrMagnitude != 0.0f)
            finalMovement += movement;

        if (forceLength != 0.0f)
        {
            finalMovement += force;

            Vector3 toZero = -force.normalized * Time.deltaTime * Damping * forceLength;

            force += toZero;

            if (forceLength < ForcePercentZero)
                force = Vector3.zero;
        }
          



        CollisionFlags collisionFlag = characterController.Move(finalMovement * Time.deltaTime);

        
        if (UseGravity)
        {
            if ((collisionFlag & CollisionFlags.Below) != 0)
            {
                jumpCount = 0;
                movement.y = 0.0f;

            }

            else
            {
                movement.y += Physics.gravity.y * Time.deltaTime * GravityScale;
            }
        }
    }

}
