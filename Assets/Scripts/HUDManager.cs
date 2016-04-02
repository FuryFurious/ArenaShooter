using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {


    public static HUDManager Instance { get; private set; }

    public GameObject PressStartRoot;
    public Text Timer;
    public GameObject TimerRoot;

    private List<PlayerHUD> huds = new List<PlayerHUD>();

    public void Awake()
    {
        Debug.Assert(Instance == null);
        Instance = this;

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            PlayerHUD hud = gameObject.transform.GetChild(i).GetComponent<PlayerHUD>();

            if (hud)
            {
                huds.Add(hud);
                hud.gameObject.SetActive(false);
            }
        }


        TimerRoot.SetActive(false);
    }

    public void SetTimerTime(int time)
    {
        if (!TimerRoot.activeSelf)
            TimerRoot.SetActive(true);

        Timer.text = time.ToString();
    }

    public void DisableTimer()
    {
        TimerRoot.SetActive(false);
     
    }

    public void RegisterPlayer(PlayerInfo player)
    {
        player.HUD = huds[player.Index];
        huds[player.Index].gameObject.SetActive(true);
        huds[player.Index].SetColorScheme(player.Controller.TheRenderer.material.color);
    }
}
