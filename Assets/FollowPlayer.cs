using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour
{
    private float zoom = 1.0f;
    private PlayerController player;
    private Vector3 offset;

    public void Init(PlayerController player, Vector3 offset)
    {
        this.player = player;
        this.offset = offset;

        gameObject.transform.position = player.transform.position + offset;
        gameObject.transform.LookAt(player.transform);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(zoom != 1.0f)
        { 
            zoom += (1.0f - zoom) * Time.deltaTime;

            if (zoom >= 1.0f)
                zoom = 1.0f;
        }

        Vector3 pos = player.transform.position;
        pos.y = 0.0f;

        gameObject.transform.position = pos + offset * zoom;
    }

    public void SetZoom(float zoom)
    {
        this.zoom = zoom;
    }
}
