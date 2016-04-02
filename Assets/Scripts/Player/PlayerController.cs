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
    public float InviFlickerTime = 0.25f;

    public Light PlayerLight;
    public WeaponRoot WeaponRoot;
    public MeshRenderer TheRenderer;
    public List<AWeapon> weapons = new List<AWeapon>();

    public PlayerInfo PlayerInfo { get; private set; }

    private Vector3 lastForce;

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

    private float flickerCooldown;

	public void CreatePlayer (int index) 
    {
        charController = GetComponent<ThirdPersonController>();

        this.PlayerInfo = new PlayerInfo("Player " + (index + 1), this, healthManager);
        WorldManager.Instance.RegisterPlayer(this.PlayerInfo, index);

        healthManager.OnInviEnter.AddListener(this.OnInviEnter);
        healthManager.OnInviUpdate.AddListener(this.OnInviUpdate);
        healthManager.OnInviEnd.AddListener(this.OnInviEnd);
    }


    public void AddForce(Vector3 force, bool startParticles)
    {
        AddForce(force.x, force.y, force.z, startParticles);
    }

    private void AddForce(float x, float y, float z, bool startParticles)
    {
        if (!healthManager.IsInvincible)
        {
            charController.AddForce(x, y, z);

            lastForce = new Vector3(x, y, z);
        }
      
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
        charController.SetMovement(movement.x, movement.z);
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
        AWeapon weapon = newWeapon.GetComponent<AWeapon>();
        weapon.OwningPlayer = this.PlayerInfo;
        weapons.Add(weapon);
    }

    public void Die(PlayerInfo killer)
    {
        //to prevent "multi kill"
        if (this.gameObject.activeSelf)
        {
            healthManager.SetCurHealth(0, true);
            WorldManager.Instance.OnPlayerDied(PlayerInfo, killer);

            PlayerDeathParticles partices = Instantiate(PrefabManager.Instance.PlayerDeathParticlesPrefab).GetComponent<PlayerDeathParticles>();
            partices.gameObject.transform.position = gameObject.transform.position;

            if(killer != null)
                partices.Init(PlayerInfo.Color, lastForce * 0.5f);

            else
                partices.Init(PlayerInfo.Color, Vector3.zero);

        }
    }

    public void ResetForce()
    {
        charController.SetForce(0.0f, 0.0f, 0.0f);
        charController.SetMovement(0.0f, 0.0f, 0.0f);
    }

    public void OnInviUpdate()
    {
        flickerCooldown -= Time.deltaTime;

        if(flickerCooldown < 0.0f)
        {
            TheRenderer.enabled = !TheRenderer.enabled;
            flickerCooldown = InviFlickerTime;
        }
         
    }

    public void OnInviEnter()
    {
        flickerCooldown = InviFlickerTime;
        TheRenderer.enabled = true;
    }

    public void OnInviEnd()
    {
        TheRenderer.enabled = true;
        flickerCooldown = 0.0f;
    }
}
