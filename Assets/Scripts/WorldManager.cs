using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class WorldManager : MonoBehaviour {

    public static WorldManager Instance { get; private set; }

    public string InputMoveHorizontalName = "MoveX_";
    public string InputMoveVerticalName = "MoveY_";
    public string InputFireName = "Fire_";
    public string InputLookHorizontalName = "LookX_";
    public string InputLookVerticalName = "LookY_";
    public string InputJumpName = "Jump_";

    private List<PlayerInfo> currentPlayers;
    [SerializeField]
    private List<Text> playerScores;


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
        player.AddOnScoreChanged(this.ScoreChanged);

        playerScores[player.Index].gameObject.SetActive(true);
        playerScores[player.Index].text = "0";
    }

	void Awake () 
    {
        Debug.Assert(Instance == null);

        Instance = this;
        currentPlayers = new List<PlayerInfo>();
	}


    private void SetInputForPlayer(PlayerInfo info)
    {
        info.Controller.InputFireName = InputFireName + info.Index;
        info.Controller.InputJumpName = InputJumpName + info.Index;
        info.Controller.InputLookHorizontalName = InputLookHorizontalName + info.Index;
        info.Controller.InputLookVerticalName = InputLookVerticalName + info.Index;
        info.Controller.InputMoveHorizontalName = InputMoveHorizontalName + info.Index;
        info.Controller.InputMoveVerticalName = InputMoveVerticalName + info.Index;
    }


    private void ScoreChanged(PlayerInfo info)
    {
        playerScores[info.Index].text = info.Score.ToString();
    }
}
