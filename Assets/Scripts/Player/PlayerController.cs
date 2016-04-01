using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(ThirdPersonController))]
public class PlayerController : MonoBehaviour 
{
    [HideInInspector]
    public int PlayerLayer;
    [HideInInspector]
    public int BulletLayer;

    private int curWeaponIndex = 0;

    public float MoveSpeed = 2.0f;
    public float JumpHeight = 5.0f;
    public float RotationSpeed = 2.0f;

    public Light PlayerLight;
    public WeaponRoot WeaponRoot;
    public MeshRenderer TheRenderer;
    public List<AWeapon> weapons = new List<AWeapon>();

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
    public string InputFire0Name;
    [HideInInspector]
    public string InputFire1Name;
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
        HandleWeapon(weapons[curWeaponIndex], InputFire0Name, AWeapon.FireMode.Primary);
        HandleWeapon(weapons[curWeaponIndex], InputFire1Name, AWeapon.FireMode.Secondary);

        if (Input.GetButtonDown(InputJumpName))
            charController.Jump(JumpHeight);

        HandleMovement();

        HandleRotation();
    }

    private void HandleRotation()
    {

        Vector3 rotation = new Vector3(Input.GetAxis(InputLookHorizontalName), 0.0f, Input.GetAxis(InputLookVerticalName));
 
        if (rotation.x != 0.0f || rotation.z != 0.0f)
        {
            rotation.Normalize();
            float rotationDistance = Mathf.Clamp(Vector3.Distance(rotation, gameObject.transform.forward), 0.0f, 1.0f);
            rotationDistance *= rotationDistance;
            gameObject.transform.forward = Vector3.RotateTowards(gameObject.transform.forward, rotation, Time.deltaTime * RotationSpeed * rotationDistance, 0.0f);
        }
    }

    private void HandleMovement()
    {
        Vector3 movement = new Vector3(Input.GetAxis(InputMoveHorizontalName), 0.0f, Input.GetAxis(InputMoveVerticalName));
        // movement.Normalize()
        movement.x *= MoveSpeed;
        movement.z *= MoveSpeed;
        charController.SetForce(movement.x, movement.z);
    }

    void HandleWeapon(AWeapon weapon, string buttonName, AWeapon.FireMode mode)
    {
        if (Input.GetButtonDown(buttonName))
            weapon.OnClick(mode);

        else if (Input.GetButton(buttonName))
            weapon.OnPress(mode);

        else if (Input.GetButtonUp(buttonName))
            weapon.OnRelease(mode);
    }

    public void AddWeaponFromPrefab(AWeapon weaponPrefab)
    {
        GameObject newWeapon = Instantiate(weaponPrefab.gameObject);

        newWeapon.gameObject.layer = BulletLayer;

        newWeapon.transform.parent = WeaponRoot.transform;
        newWeapon.transform.position = WeaponRoot.transform.position;
        newWeapon.transform.forward = WeaponRoot.transform.forward;
       
        weapons.Add(newWeapon.GetComponent<AWeapon>());
    }

    public void Die()
    {
        WorldManager.Instance.OnPlayerDied(Info);
    }

    public void OnHit(HealthManager health, int delta)
    {
        if(delta < 0)
        {
            Info.PlayerCam.SetZoom(0.75f);
        }
    }
}
