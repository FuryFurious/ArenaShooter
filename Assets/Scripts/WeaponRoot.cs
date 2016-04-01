using UnityEngine;
using System.Collections;

public class WeaponRoot : MonoBehaviour {

    [SerializeField]
    private PlayerController player;

	// Use this for initialization
	public void Init (int index)
    {
        gameObject.layer = WorldManager.Instance.GetPlayerLayer(index);
	}
	

}
