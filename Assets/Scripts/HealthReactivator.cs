using UnityEngine;
using System.Collections;

public class HealthReactivator : MonoBehaviour {

    public HealthPickUp PickUp;
    private float RespawnTime = 30.0f;
    private float curRespawnTime = 0.0f;

    void Start()
    {
        RespawnTime = WorldManager.Instance.MedPackRespawnTime;

        WorldManager.Instance.RegisterHealthPack(this);
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if(curRespawnTime > 0.0f)
        {
            curRespawnTime -= Time.deltaTime;

            if(curRespawnTime <= 0.0f)
            {
                PickUp.gameObject.SetActive(true);
            }
        }
	}

    public void StartRespawn()
    {
        curRespawnTime = RespawnTime;
    }

    public void Reset()
    {
        curRespawnTime = 0.0f;
        PickUp.gameObject.SetActive(true);
    }
}
