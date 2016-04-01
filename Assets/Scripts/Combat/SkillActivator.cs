using UnityEngine;
using System.Collections;

public class SkillActivator : MonoBehaviour 
{
    public float Cooldown = 0.5f;
    public bool AttachToSpawner = false;
    public bool InstantiatePrefab = true;

    public bool IsReady { get { return curCooldown <= 0.0f; } }

    [SerializeField]
    private ASkill AttackPrefab;
    private ASkill lastAttack;

    private float curCooldown;


	// Use this for initialization
	void Start () 
    {
        this.curCooldown = this.Cooldown;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (!IsReady)
        {
            this.curCooldown -= Time.deltaTime;
        }
	}

    public bool TryUse()
    {
        if (IsReady)
        {
            if(InstantiatePrefab || lastAttack == null)
                lastAttack = Instantiate(AttackPrefab.gameObject).GetComponent<ASkill>();

            if (AttachToSpawner)
                lastAttack.transform.SetParent(this.gameObject.transform);

            lastAttack.Init(this, gameObject.transform.position, gameObject.transform.forward);

            this.curCooldown = this.Cooldown;
            return true;
        }

        else
            return false;
    }
}
