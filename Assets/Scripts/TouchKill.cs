using UnityEngine;
using System.Collections;

public class TouchKill : MonoBehaviour {


	void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player)
        {
            player.Die(null);
        }

        else
        {
            Destroy(other.gameObject);
        }
    }
}
