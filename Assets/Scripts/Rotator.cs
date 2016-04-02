using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {

    public Vector3 Rotation;

    public Space RotationSpace = Space.World;

    public bool RandomizeStart;

    void Start()
    {
        if (RandomizeStart)
        {
            gameObject.transform.Rotate(Rotation.normalized * Random.Range(0.0f, 360.0f), RotationSpace);
        }
    }
	
	// Update is called once per frame
	void Update () 
    {
        gameObject.transform.Rotate(Rotation * Time.deltaTime, RotationSpace);
	}
}
