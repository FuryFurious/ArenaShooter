using UnityEngine;
using System.Collections;

public class HealthManager : MonoBehaviour {

    public GameObject RootObject;
    public int startHealth;

    public int Health { get; private set; }
    public int MaxHealth { get; private set; }

	// Use this for initialization
	void Start () 
    {
        this.Health = startHealth;
        this.MaxHealth = startHealth;

        if (RootObject == null)
            RootObject = gameObject;
	}

    public void ChangeHealth(int delta)
    {
        this.Health += delta;

        if (this.Health <= 0)
        {
            this.Health = 0;
            Destroy(RootObject);
        }

        else if (this.Health > MaxHealth)
        {
            this.Health = MaxHealth;
        }
    }

    public float GetPercent()
    {
        return (float)Health / (float)MaxHealth;
    }


}
