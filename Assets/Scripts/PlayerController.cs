using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ThirdPersonController))]
public class PlayerController : MonoBehaviour 
{
    public float MoveSpeed = 2.0f;
    public float JumpHeight = 5.0f;
    public float RotationSpeed = 2.0f;

    public SkillActivator shootSkill;

    public PlayerInfo Info { get; private set; }

    [HideInInspector]
    public string InputMoveHorizontalName;
    [HideInInspector]
    public string InputMoveVerticalName;
    [HideInInspector]
    public string InputLookHorizontalName;
    [HideInInspector]
    public string InputLookVerticalName;
    [HideInInspector]
    public string InputFireName;
    [HideInInspector]
    public string InputJumpName;

    [SerializeField]
    private HealthManager healthManager;
    private ThirdPersonController charController;

	void Start () 
    {
        charController = GetComponent<ThirdPersonController>();

        this.Info = new PlayerInfo(gameObject.name, this, healthManager);
        WorldManager.Instance.RegisterPlayer(this.Info);
	}

    void Update()
    {
        if (Input.GetButton(InputFireName))
            shootSkill.TryUse();

        if (Input.GetButtonDown(InputJumpName))
        {
            charController.Jump(JumpHeight);
        }


       
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(Input.GetAxis(InputMoveHorizontalName), 0.0f, Input.GetAxis(InputMoveVerticalName));
        movement.Normalize();

        movement.x *= MoveSpeed;
        movement.z *= MoveSpeed;

        Vector3 rotation = new Vector3(Input.GetAxisRaw(InputLookHorizontalName), 0.0f, Input.GetAxisRaw(InputLookVerticalName));

        if (rotation.x != 0.0f | rotation.z != 0.0f)
            gameObject.transform.forward = Vector3.RotateTowards(gameObject.transform.forward, rotation.normalized, Time.deltaTime * RotationSpeed, 0.0f);

        charController.SetForce(movement.x, movement.z);
    }
}
