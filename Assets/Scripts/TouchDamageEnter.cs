using UnityEngine;
using System.Collections;

public class TouchDamageEnter : MonoBehaviour {

    public int Damage = 50;

    void OnCollisionEnter(Collision collision)
    {
        HealthManager manager = collision.collider.gameObject.GetComponent<HealthManager>();

        if (manager)
        {
            manager.ChangeHealth(-50, null);
        }
    }

}
