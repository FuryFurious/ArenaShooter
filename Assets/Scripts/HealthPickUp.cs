using UnityEngine;
using System.Collections;

public class HealthPickUp : MonoBehaviour
{
    public HealthReactivator Reactivator;
    public int HealthAmount = 1;

    void OnTriggerEnter(Collider other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();
    
        if(controller != null)
        {
            controller.Info.HealthManager.ChangeHealth(HealthAmount);
            gameObject.SetActive(false);
            Reactivator.StartRespawn();
        }


    }
}
