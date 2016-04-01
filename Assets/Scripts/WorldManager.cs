using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class WorldManager : MonoBehaviour {


    public static WorldManager Instance { get; private set; }

    public float PlayerRespawnTime = 5.0f;
    public int PlayerHealth = 10;
    public Vector3 CameraOffset = new Vector3(0.0f, 15.0f, -5.0f);
    public float CameraForwardOffset = 2.0f;

    public string InputMoveHorizontalName = "MoveX_";
    public string InputMoveVerticalName = "MoveY_";
    public string InputFire0Name = "Fire0_";
    public string InputFire1Name = "Fire1_";
    public string InputLookHorizontalName = "LookX_";
    public string InputLookVerticalName = "LookY_";
    public string InputJumpName = "Jump_";

    private List<PlayerInfo> currentPlayers;
    private List<SpawnPoint> spawnPoints;
    

    void Awake()
    {
        Debug.Assert(Instance == null);

        Instance = this;
        currentPlayers = new List<PlayerInfo>();
        spawnPoints = new List<SpawnPoint>();
    }

    public List<PlayerInfo> GetPlayerInfos()
    {
        return currentPlayers;
    }

    public PlayerInfo GetPlayerInfo(int id)
    {
        return currentPlayers[id];
    }

    public int GetNumPlayers()
    {
        return currentPlayers.Count;
    }

    public void RegisterPlayer(PlayerInfo player)
    {
        Debug.Assert(!currentPlayers.Contains(player));

        player.Index = currentPlayers.Count;
        currentPlayers.Add(player);
        SetInputForPlayer(player);

        player.Controller.PlayerLayer = WorldManager.Instance.GetPlayerLayer(player.Index);
        player.Controller.BulletLayer = WorldManager.Instance.GetPlayerBulletLayer(player.Index);

        player.HealthManager.gameObject.layer = GetPlayerLayer(player.Index);
        player.Controller.WeaponRoot.Init(player.Index);

        player.Controller.AddWeaponFromPrefab(PrefabManager.Instance.DefaultWeaponPrefab);

        player.Controller.TheRenderer.material = PrefabManager.Instance.GetMaterial(player.Index);
        player.Controller.PlayerLight.color = player.Controller.TheRenderer.material.color;

        player.HealthManager.SetHealth(PlayerHealth);

        InitCamera(player);
    }

    private void InitCamera(PlayerInfo player)
    {
        GameObject playercam = Instantiate(PrefabManager.Instance.PlayerCam.gameObject);
        FollowPlayer follower = playercam.GetComponent<FollowPlayer>();
        follower.Init(player.Controller, CameraOffset);
        player.SetCamera(follower);

        CameraAdded();
    }

    private void CameraAdded()
    {
        int numCams = currentPlayers.Count;

        for (int i = 0; i < currentPlayers.Count; i++)
        {
            currentPlayers[i].Cam.rect = GetViewPortRect(i, numCams);
        }
    }


    //TODO: somehow better:
    private Rect GetViewPortRect(int id, int maxCams)
    {
        Debug.Assert(maxCams > 0);

        if(maxCams == 1)
        {
            return new Rect(0.0f, 0.0f, 1.0f, 1.0f);
        }

        else
        {
            Debug.Assert(maxCams == 2);

            if (id == 0)
                return new Rect(0.0f, 0.0f, 0.5f, 1.0f);

            else
                return new Rect(0.5f, 0.0f, 0.5f, 1.0f);
        }

    }

 

    public void RegistersSpawnPoint(SpawnPoint point)
    {
        spawnPoints.Add(point);
    }

    private void SetInputForPlayer(PlayerInfo info)
    {
        info.Controller.InputFire0Name = InputFire0Name + info.Index;
        info.Controller.InputFire1Name = InputFire1Name + info.Index;
        info.Controller.InputJumpName = InputJumpName + info.Index;
        info.Controller.InputLookHorizontalName = InputLookHorizontalName + info.Index;
        info.Controller.InputLookVerticalName = InputLookVerticalName + info.Index;
        info.Controller.InputMoveHorizontalName = InputMoveHorizontalName + info.Index;
        info.Controller.InputMoveVerticalName = InputMoveVerticalName + info.Index;
    }

    public int GetPlayerLayer(int index)
    {
        return LayerMask.NameToLayer("P" + (index + 1));
    }


    public int GetPlayerBulletLayer(int index)
    {
        return LayerMask.NameToLayer("P" + (index + 1) + "Bullet");
    }

    


    public void OnPlayerDied(PlayerInfo player)
    {
        player.Controller.gameObject.SetActive(false);

        SpawnPoint point = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)];

        point.SpawnPlayerIn(player, PlayerRespawnTime);
    }

}
