using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Collider))]
public class HealthManager : MonoBehaviour
{
    public int Health;
    public int MaxHealth = 1;

    public KillEvent OnZeroHealth;
    public HealthChangedEvent OnHealthChanged;
    public VoidEvent OnInviEnter;
    public VoidEvent OnInviUpdate;
    public VoidEvent OnInviEnd;

    public bool IsInvincible;

    private float inviTime;

    private Collider relatedCollider;

	// Use this for initialization
	void Start() 
    {
        relatedCollider = GetComponent<Collider>();
        this.Health = MaxHealth;
	}

    void Update()
    {
        if(inviTime > 0.0 && this.IsInvincible)
        {
            InviUpdate();

            if (inviTime <= 0.0f)
            {
                InviEnd();
            }
              
        }

    }

    public void SetMinMaxHealth(int health)
    {
        this.Health = health;
        this.MaxHealth = health;
    }

    public void SetCurHealth(int newVal, bool invokeChangedEvent)
    {
        int delta = newVal - Health;

        if (delta < 0 && IsInvincible)
            delta = 0;

        this.Health = Mathf.Clamp(newVal, 0, MaxHealth);

        if(invokeChangedEvent)
            OnHealthChanged.Invoke(this, delta);
    }

    public void StartInvi(float playerInviTime)
    {
        InviStart(playerInviTime);
    }

    public void ChangeHealth(int delta, PlayerInfo hitter)
    {
        if (delta < 0 && IsInvincible)
            delta = 0;

        this.Health += delta;
  
        if (this.Health <= 0)
        {
            this.Health = 0;
            OnZeroHealth.Invoke(hitter);
        }

        else if (this.Health > MaxHealth)
        {
            this.Health = MaxHealth;
        }

        OnHealthChanged.Invoke(this, delta);
    }


    public float GetPercent()
    {
        return (float)Health / (float)MaxHealth;
    }


    void InviStart(float time)
    {
        this.IsInvincible = true;
        this.inviTime = time;

        if (relatedCollider)
            relatedCollider.enabled = false;

        OnInviEnter.Invoke();
    }

    void InviUpdate()
    {
        inviTime -= Time.deltaTime;

        OnInviUpdate.Invoke();
    }

    void InviEnd()
    {
        this.IsInvincible = false;

        inviTime = 0;

        if (relatedCollider)
            relatedCollider.enabled = true;

        OnInviEnd.Invoke();
    }

    public void Reset()
    {
        this.SetCurHealth(this.MaxHealth, true);
        InviEnd();
    }

}
