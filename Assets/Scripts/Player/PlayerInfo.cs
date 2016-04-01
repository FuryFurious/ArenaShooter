using UnityEngine;
using System.Collections;
using System;

public class PlayerInfo 
{
    public string Name { get; private set; }
    public int CurHealth { get { return HealthManager.Health; } }
    public int Index { get; set; }
    public PlayerController Controller { get; private set; }
    public FollowPlayer PlayerCam { get; private set; }
    public Camera Cam { get; private set; }
   
    public void SetCamera(FollowPlayer follower)
    {
        this.PlayerCam = follower;

        this.Cam = follower.GetComponent<Camera>();

    }

    public HealthManager HealthManager
    {
        get; private set;
    }

    public PlayerInfo(string name, PlayerController controller, HealthManager health)
    {
        this.Name = name;
        this.HealthManager = health;
        this.Controller = controller;

        Debug.Assert(this.HealthManager);
        Debug.Assert(this.Controller);
    }

}
