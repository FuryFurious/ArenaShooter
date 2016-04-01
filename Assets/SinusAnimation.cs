using UnityEngine;
using System.Collections;

public class SinusAnimation : MonoBehaviour {

    public Vector3 Axis;
    public float TimeFullCircle;

    private float curTime = 0.0f;
    private Vector3 startPos;

    void Start()
    {
        startPos = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        curTime += Time.deltaTime;

        float t = curTime / TimeFullCircle;

        if(t >= 1.0f)
        {
            curTime = 0.0f;

          
        }

        gameObject.transform.position = startPos + Axis * (Mathf.Sin(t * Mathf.PI * 2.0f) * 0.5f + 0.5f);

    }
}
