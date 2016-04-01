using UnityEngine;
using System.Collections;

public class CoinPickup : MonoBehaviour {


    void OnTriggerEnter(Collider collider)
    {
        //Debug.Log(collider.gameObject);

        PlayerController controller = collider.GetComponent<PlayerController>();

        if (controller)
        {
            controller.Info.Score++;
            Destroy(gameObject);
        }

      
    }
}
