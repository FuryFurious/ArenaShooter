using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {

    public Vector3 Rotation;

	
	// Update is called once per frame
	void FixedUpdate () 
    {
        gameObject.transform.Rotate(Rotation * Time.deltaTime);
	}
}
