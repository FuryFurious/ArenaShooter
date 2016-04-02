using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class PlayerInfo 
{
    public string Name { get; private set; }
    public int CurHealth { get { return HealthManager.Health; } }
    public int Index { get; set; }
    public PlayerController Controller { get; private set; }

    public bool IsAlive { get { return CurHealth != 0.0f; } }

    public PlayerHUD HUD;

    private int kills;
    private int deaths;
    private int score;

    public int Score { get { return score; } set { SetScore(value); } }
    public int Kills { get { return kills; } set { SetKills(value); } }
    public int Deaths { get { return deaths; } set { SetDeaths(value); } }
   
    private void SetKills(int val)
    {
        kills = val;
    }

    private void SetDeaths(int val)
    {
        deaths = val;
    }

    private void SetScore(int val)
    {
        score = val;
        UpdateText();
    }

    private void UpdateText()
    {
        HUD.SetScoreText(score);
    }


    public HealthManager HealthManager
    {
        get; private set;
    }
    public Color Color { get; set; }

    public PlayerInfo(string name, PlayerController controller, HealthManager health)
    {
        this.Name = name;
        this.HealthManager = health;
        this.Controller = controller;

        Debug.Assert(this.HealthManager);
        Debug.Assert(this.Controller);
    }

    public void Reset()
    {
        Kills = 0;
        Deaths = 0;
        Score = 0;

        HealthManager.Reset();
    }
}
