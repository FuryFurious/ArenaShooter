using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour
{

    private PlayerInfo spawnPlayer;
    private float cooldown;

    void Start()
    {
        WorldManager.Instance.RegistersSpawnPoint(this);
    }

    void Update()
    {
        if (spawnPlayer != null)
        {
            cooldown -= Time.deltaTime;

            if(cooldown < 0.0f)
            {
                spawnPlayer.Controller.gameObject.transform.position = gameObject.transform.position;
                spawnPlayer.Controller.gameObject.SetActive(true);
                spawnPlayer.HealthManager.ResetHealth();
                spawnPlayer = null;

            }
        }

    }

    public void SpawnPlayerIn(PlayerInfo info, float time)
    {
        Debug.Assert(!info.Controller.gameObject.activeSelf);
        spawnPlayer = info;
        cooldown = time;
    }

}
