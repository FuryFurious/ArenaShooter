using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance { get; private set; }

    private GlobalCamera globalCam;

    public int TimeToStart = 5;
    public int MedPackHealthAmount;
    public float MedPackRespawnTime;
    private bool useRoundTimer = false;

    public float PlayerInviTime = 1.0f;

    public void RegisterCharacterImpulse(CharacterImpulse characterImpulse)
    {
        impulses.Add(characterImpulse);
        impulses.Sort((x, y) => x.PlayerIndex.CompareTo(y.PlayerIndex));
    }

    public float PlayerRespawnTime = 5.0f;
    public int PlayerHealth = 10;
    public Vector3 CameraOffset = new Vector3(0.0f, 15.0f, -5.0f);

    public string InputMoveHorizontalName = "MoveX_";
    public string InputMoveVerticalName = "MoveY_";
    public string InputFire0Name = "Fire0_";
    public string InputFire1Name = "Fire1_";
    public string InputLookHorizontalName = "LookX_";
    public string InputLookVerticalName = "LookY_";
    public string InputJumpName = "Jump_";
    public string InputStartName = "Start_";

    private PlayerInfo[] currentPlayers;
    private List<SpawnPoint> spawnPoints;
    private List<PlayerStartPoint> startPoint;
    private List<HealthReactivator> healthPacks;
    private List<CharacterImpulse> impulses;


    private float timeWhenReset;

    void Awake()
    {
        Debug.Assert(Instance == null);

        Instance = this;

        currentPlayers = new PlayerInfo[4];
        spawnPoints = new List<SpawnPoint>();
        startPoint = new List<PlayerStartPoint>();
        healthPacks = new List<HealthReactivator>();
        impulses = new List<CharacterImpulse>();

        InitGlobalCamera();
    }

    public PlayerInfo[] GetPlayerInfos()
    {
        return currentPlayers;
    }

    public PlayerInfo GetPlayerInfo(int id)
    {
        if (id < 0 && id >= 4)
            return null;

        else
            return currentPlayers[id];
    }

    public int GetNumPlayers()
    {
        return currentPlayers.Length;
    }

    public void RegisterHealthPack(HealthReactivator healthReactivator)
    {
        healthPacks.Add(healthReactivator);
    }

    public void ResetCharImpulses()
    {
        for (int i = 0; i < impulses.Count; i++)
        {
            impulses[i].Reset();

        }
    }

    public void RegisterPlayer(PlayerInfo player, int index)
    {
        Debug.Assert(startPoint.Count >= 4);

        player.Index = index;
        currentPlayers[index] = player;
        SetInputForPlayer(player);

        player.Controller.PlayerLayer = WorldManager.Instance.GetPlayerLayer(player.Index);
        player.Controller.BulletLayer = WorldManager.Instance.GetPlayerBulletLayer(player.Index);

        player.HealthManager.gameObject.layer = GetPlayerLayer(player.Index);
        player.Controller.WeaponRoot.Init(player.Index);

        player.Controller.AddWeaponFromPrefab(PrefabManager.Instance.DefaultWeaponPrefab);

        player.Controller.TheRenderer.material = PrefabManager.Instance.PlayerMaterial;
        player.Color = PrefabManager.Instance.GetPlayerColor(player.Index); ;
        player.Controller.TheRenderer.material.color = player.Color;

        player.Controller.PlayerLight.color = player.Color;

        player.HealthManager.SetMinMaxHealth(PlayerHealth);

        player.HealthManager.OnHealthChanged.AddListener(HealthChanged);

        HUDManager.Instance.RegisterPlayer(player);

        player.Controller.gameObject.transform.position = startPoint[player.Index].transform.position;


        
        for (int i = 0; i < impulses.Count; i++)
        {
            impulses[i].Init();
        }
       
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ResetGame();

        if (useRoundTimer)
        {
            float delta = TimeToStart - (Time.realtimeSinceStartup - timeWhenReset) + 0.5f;

            HUDManager.Instance.SetTimerTime((int)delta);

            if((int)delta <= 0)
            {
                Time.timeScale = 1.0f;
                useRoundTimer = false;
                HUDManager.Instance.DisableTimer();
            }
        }
    }

    private void ResetMedPacks()
    {
        for (int i = 0; i < healthPacks.Count; i++)
        {
            healthPacks[i].Reset();
        }
    }

    private void ResetPositions()
    {
        for (int i = 0; i < currentPlayers.Length; i++)
        {
            if(currentPlayers[i] != null)
                currentPlayers[i].Controller.gameObject.transform.position = startPoint[currentPlayers[i].Index].transform.position;
        }
            
        
    }

    public void ResetGame()
    {
        for (int i = 0; i < impulses.Count; i++)
        {
            impulses[i].Init();
        }

        ResetCharImpulses();

        ResetMedPacks();

        ClearSpawnPoints();

        for (int i = 0; i < currentPlayers.Length; i++)
        {
            if(currentPlayers[i] != null)
                currentPlayers[i].Reset();
        }
 
        ResetPositions();

        if (globalCam)
            globalCam.Reset();

        useRoundTimer = true;
        timeWhenReset = Time.realtimeSinceStartup;
        Time.timeScale = 0.0f;
    }

    private void ClearSpawnPoints()
    {
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            spawnPoints[i].ClearSpawnPoint();
        }
    }

    private void InitGlobalCamera()
    {
        globalCam = Instantiate(PrefabManager.Instance.GlobalPlayerCamPrefab.gameObject).GetComponent<GlobalCamera>();
    }

    private void HealthChanged(HealthManager manager, int delta)
    {
        for (int i = 0; i < currentPlayers.Length; i++)
        {
            if(currentPlayers[i] != null && currentPlayers[i].HealthManager == manager)
            {
                currentPlayers[i].HUD.LifeBar.value = manager.GetPercent();
                break;
            }
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

    


    public void OnPlayerDied(PlayerInfo player, PlayerInfo killer)
    {
        player.Deaths++;

        //player died, but it was by the surroundings:
        if(killer == null)
            player.Score = Mathf.Max(player.Score - 1, 0);

        player.Controller.gameObject.SetActive(false);

        SpawnPoint point = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)];

        point.SpawnPlayerIn(player, PlayerRespawnTime);

        if(killer != null)
        {
            killer.Kills++;
            killer.Score++;
        }
    }

    public void RegisterPlayerStartPoint(PlayerStartPoint playerStartPoint)
    {
        this.startPoint.Add(playerStartPoint);

        startPoint.Sort((x, y) => x.PlayerIndex.CompareTo(y.PlayerIndex));
    }
}
