using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GlobalCamera : MonoBehaviour
{


    public float MinDistance;
    public float MaxDistance;
    private Vector3 offset;


    // Use this for initialization
    void Start()
    {
        offset = WorldManager.Instance.CameraOffset;

        gameObject.transform.position = offset;

        gameObject.transform.LookAt(Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInfo[] players = WorldManager.Instance.GetPlayerInfos();


            Vector3 min = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
            Vector3 max = new Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);

            int aliveCount = 0;

            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] != null && players[i].IsAlive)
                {

                    aliveCount++;

                    Vector3 playerPos = players[i].Controller.transform.position;

                    if (playerPos.x < min.x)
                        min.x = playerPos.x;

                    if (playerPos.y < min.y)
                        min.y = playerPos.y;

                    if (playerPos.z < min.z)
                        min.z = playerPos.z;


                    if (playerPos.x > max.x)
                        max.x = playerPos.x;

                    if (playerPos.y > max.y)
                        max.y = playerPos.y;

                    if (playerPos.z > max.z)
                        max.z = playerPos.z;

                }
            

            if (aliveCount != 0)
            {

                Vector3 center = (max + min) * 0.5f;

                float xEdge = Mathf.Abs(max.x - min.x);
                float yEdge = Mathf.Abs(max.y - min.y);
                float zEdge = Mathf.Abs(max.z - min.z);

                float maxEdge = Mathf.Max(xEdge, Mathf.Max(yEdge, zEdge));

                maxEdge = Mathf.Sqrt(Mathf.Sqrt(maxEdge));
                maxEdge = Mathf.Clamp(maxEdge, MinDistance, MaxDistance);



                Vector3 targetPos = center + offset * maxEdge;
                Vector3 dir = targetPos - gameObject.transform.position;

                gameObject.transform.position += dir * Time.deltaTime;
            }
        }
    }

    public void Reset()
    {
        PlayerInfo[] players = WorldManager.Instance.GetPlayerInfos();

        Vector3 min = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
        Vector3 max = new Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);

        int aliveCount = 0;

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] != null && players[i].IsAlive)
            {
                aliveCount++;

                Vector3 playerPos = players[i].Controller.transform.position;

                if (playerPos.x < min.x)
                    min.x = playerPos.x;

                if (playerPos.y < min.y)
                    min.y = playerPos.y;

                if (playerPos.z < min.z)
                    min.z = playerPos.z;


                if (playerPos.x > max.x)
                    max.x = playerPos.x;

                if (playerPos.y > max.y)
                    max.y = playerPos.y;

                if (playerPos.z > max.z)
                    max.z = playerPos.z;

            }

        }

        if(aliveCount != 0)
        {
            Vector3 center = (max + min) * 0.5f;
            Vector3 targetPos = center + offset;

            gameObject.transform.position = targetPos;
        }

        else
        {
            gameObject.transform.position = offset;
        }
   

    }


}
