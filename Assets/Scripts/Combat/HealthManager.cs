using UnityEngine;
using System.Collections;
using System;

public class HealthManager : MonoBehaviour
{
    public int startHealth;

    public int Health { get; private set; }
    public int MaxHealth { get; private set; }

    public VoidEvent OnZeroHealth;
    public HealthChangedEvent OnHealthChanged;

	// Use this for initialization
	void Start() 
    {
        this.Health = startHealth;
        this.MaxHealth = startHealth;
	}

    public void SetHealth(int health)
    {
        this.Health = health;
        this.MaxHealth = health;
    }

    public void ChangeHealth(int delta)
    {
        this.Health += delta;

        OnHealthChanged.Invoke(this, delta);

        if (this.Health <= 0)
        {
            this.Health = 0;

            OnZeroHealth.Invoke();
        }

        else if (this.Health > MaxHealth)
        {
            this.Health = MaxHealth;
        }
    }

    public void ResetHealth()
    {
        this.Health = MaxHealth;
    }

    public float GetPercent()
    {
        return (float)Health / (float)MaxHealth;
    }


}
