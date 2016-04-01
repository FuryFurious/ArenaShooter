using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonController : MonoBehaviour 
{
    public float GravityScale = 1.0f;
    public bool UseGravity = true;
    public int MaxJumpCount = 1;

    private int jumpCount = 0;
    private CharacterController characterController;
    private Vector3 force;

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

    public void SetForce(float x, float z)
    {
        force.x = x;
        force.z = z;
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
            this.force.y += jumpForce;
            jumpCount++;
        } 
    }

    void FixedUpdate()
    {
        CollisionFlags collisionFlag = characterController.Move(force * Time.deltaTime);

        if (UseGravity)
        {
            if ((collisionFlag & CollisionFlags.Below) != 0)
            {
                jumpCount = 0;
                force.y = 0.0f;

            }

            else
            {
                force.y += Physics.gravity.y * Time.deltaTime * GravityScale;
            }
        }
    }
}
