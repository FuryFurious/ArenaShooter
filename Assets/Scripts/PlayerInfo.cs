using UnityEngine;
using System.Collections;
using System;

public class PlayerInfo 
{
    public string Name { get; private set; }
    public int Score { get { return score; } set { SetScore(value); } }
    public int Health { get { return healthManager.Health; } }
    public int Index { get; set; }
    public PlayerController Controller { get; private set; }

    private Action<PlayerInfo> onScoreChanged;
    private HealthManager healthManager;

    private int score;

    public PlayerInfo(string name, PlayerController controller, HealthManager health)
    {
        this.Name = name;
        this.healthManager = health;
        this.Controller = controller;

        Debug.Assert(this.healthManager);
        Debug.Assert(this.Controller);
    }

    private void SetScore(int newScore)
    {
        score = newScore;

        if (onScoreChanged != null)
            onScoreChanged.Invoke(this);
    }

    public void AddOnScoreChanged(Action<PlayerInfo> callback)
    {
        this.onScoreChanged += callback;
    }

    public void RemoveOnScoreChanged(Action<PlayerInfo> callback)
    {
        this.onScoreChanged -= callback;
    }

}
