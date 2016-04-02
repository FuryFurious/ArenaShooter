using UnityEngine;
using System.Collections;
using System;

public class PrefabManager : MonoBehaviour
{
    public static PrefabManager Instance { get; private set; }

    public GameObject PlayerPrefab;

    public AWeapon DefaultWeaponPrefab;

    public GlobalCamera GlobalPlayerCamPrefab;
    public PlayerDeathParticles PlayerDeathParticlesPrefab;
    

    public Material PlayerMaterial;

    [SerializeField]
    private Color[] playerColors;

    private int numSpawnedPlayers = 0;
    private bool[] playerSpawned;

    void Awake()
    {
        Debug.Assert(!Instance);
        Instance = this;

        playerSpawned = new bool[4];
    }

    public Color GetPlayerColor(int index)
    {
        return playerColors[index];
    }

    void Update()
    {
        if (numSpawnedPlayers < 4)
        {
            for (int i = 0; i < 4; i++)
            {
                if (!playerSpawned[i] && Input.GetButtonDown(WorldManager.Instance.InputStartName + i))
                    SpawnPlayer(i);
            }
        }
    }

    private void SpawnPlayer(int index)
    {
        playerSpawned[index] = true;
        GameObject playerObj = Instantiate(PlayerPrefab);
        PlayerController playerController = playerObj.GetComponent<PlayerController>();

        playerController.CreatePlayer(index);

        numSpawnedPlayers++;

        if(numSpawnedPlayers >= 2)
        {
            HUDManager.Instance.PressStartRoot.SetActive(false);
            WorldManager.Instance.ResetGame();
        }
    }

}
