using UnityEngine;
using System.Collections;

public class SinusAnimationGlobal : MonoBehaviour {

    public enum SinusMode { Relative, Absolute}

    public bool Randomize;

    public SinusMode Mode;
    public Vector3 Axis;
    public float TimeFullCircle;

    private float curTime = 0.0f;
    private Vector3 startPos;

    void Start()
    {
        startPos = gameObject.transform.position;

        if(Mode == SinusMode.Relative)
        {
            startPos = gameObject.transform.localPosition;
        }

        if (Randomize)
        {
            curTime = Random.Range(0.0f, TimeFullCircle);
        }
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

        if(Mode == SinusMode.Absolute)
            gameObject.transform.position = startPos + Axis * (Mathf.Sin(t * Mathf.PI * 2.0f) * 0.5f + 0.5f);

        else
        {
            gameObject.transform.localPosition = startPos + Axis * (Mathf.Sin(t * Mathf.PI * 2.0f) * 0.5f + 0.5f);
        }

    }
}
