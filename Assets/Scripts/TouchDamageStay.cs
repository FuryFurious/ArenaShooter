using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TouchDamageStay : MonoBehaviour {

    public int Damage = 2;
    public float Cooldown = 0.25f;
    private float curCooldown;
    private List<PlayerController> currentTouchingPlayers = new List<PlayerController>();

	// Use this for initialization
	void Start () {
        curCooldown = Cooldown;
    }
	
	// Update is called once per frame
	void Update ()
    {
        curCooldown -= Time.deltaTime;

        if(curCooldown <= 0.0f)
        {
            for (int i = currentTouchingPlayers.Count - 1; i >= 0 ; i--)
            {
                if(currentTouchingPlayers[i].gameObject.activeSelf)
                    currentTouchingPlayers[i].PlayerInfo.HealthManager.ChangeHealth(-Damage, null);

                else
                {
                    currentTouchingPlayers.RemoveAt(i);
                }
            }

            curCooldown = Cooldown;
        }
       
	}

    void OnTriggerEnter(Collider col)
    {
        PlayerController player = col.gameObject.GetComponent<PlayerController>();

        if (player)
        {

            if (currentTouchingPlayers.Contains(player))
                return;

            else
                currentTouchingPlayers.Add(player);
        }
    }

    void OnTriggerExit(Collider col)
    {
        PlayerController player = col.gameObject.GetComponent<PlayerController>();

        if (player)
        {
            if (currentTouchingPlayers.Contains(player))
                currentTouchingPlayers.Remove(player);
        }
    }
}
