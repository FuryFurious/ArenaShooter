using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnPoint : MonoBehaviour
{
    private List<SpawnInfo> spawnPlayers = new List<SpawnInfo>();

    void Start()
    {
        WorldManager.Instance.RegistersSpawnPoint(this);
    }

    void Update()
    {
        if (spawnPlayers.Count > 0)
        {
            for (int i = 0; i < spawnPlayers.Count; i++)
            {
                spawnPlayers[i].time -= Time.deltaTime;

                if (spawnPlayers[i].time <= 0.0f)
                {
                    SpawnPlayer(i);
                }

             
            }

        }

    }

    private void SpawnPlayer(int i)
    {
        spawnPlayers[i].info.Controller.ResetForce();
        spawnPlayers[i].info.Controller.gameObject.transform.position = gameObject.transform.position;
        spawnPlayers[i].info.Controller.gameObject.SetActive(true);
        spawnPlayers[i].info.HealthManager.SetCurHealth(spawnPlayers[i].info.HealthManager.MaxHealth, true);
        spawnPlayers[i].info.HealthManager.StartInvi(WorldManager.Instance.PlayerInviTime);

        spawnPlayers.RemoveAt(i);
    }

    public void ClearSpawnPoint()
    {
        for (int i = 0; i < spawnPlayers.Count; i++)
        {
            SpawnPlayer(i);
        }
    }

    public void SpawnPlayerIn(PlayerInfo info, float time)
    {
        Debug.Assert(!info.Controller.gameObject.activeSelf);

        spawnPlayers.Add(new SpawnInfo(info, time));

    }

    private class SpawnInfo
    {
        public PlayerInfo info;
        public float time;

        public SpawnInfo(PlayerInfo info, float time)
        {
            this.info = info;
            this.time = time;
        }
    }
}
