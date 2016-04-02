using UnityEngine;
using System.Collections;

public class HealthPickUp : MonoBehaviour
{
    public HealthReactivator Reactivator;
    private int HealthAmount = 1;

    void Start()
    {
        HealthAmount = WorldManager.Instance.MedPackHealthAmount;
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();
    
        if(controller != null)
        {
            controller.PlayerInfo.HealthManager.ChangeHealth(HealthAmount, null);
            gameObject.SetActive(false);
            Reactivator.StartRespawn();
        }


    }
}
