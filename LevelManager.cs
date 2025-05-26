using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager: MonoBehaviour
{
    public BallController ball;
    
    public TextMeshProUGUI labelPlayerName;

    private PlayerRecord playerRecord;
    private int playerIndex;

    //void Awake()
    //{
    //DontDestroyOnLoad(gameObject);
     //}

    void Start()
    {
        Debug.Log(GameObject.Find("PlayerRecord"));
        playerRecord = GameObject.Find("Player Record").GetComponent<PlayerRecord>();
        playerIndex = 0;
        SetupPlayer();
    }

    private void SetupPlayer()
    {
        ball.SetupBall(playerRecord.playerColours[playerIndex]);
        labelPlayerName.text = playerRecord.playerList[playerIndex].name;
    }

    //Swiching to the next player.
    public void NextPlayer(int previousPutts)
    {
        Debug.Log($"NextPlayer called. Player {playerIndex} scored {previousPutts} putts.");

        playerRecord.AddPutts(playerIndex, previousPutts);

        if (playerIndex < playerRecord.playerList.Count - 1)
        {
            playerIndex++;
            Debug.Log("Switching to next player: " + playerIndex);
            SetupPlayer();
        }
        else
        {
            Debug.Log("All players finished. Moving to next level.");
            if (playerRecord.levelIndex == playerRecord.levels.Length - 1)
            {
                SceneManager.LoadScene("Scoreboard");
            }
            else
            {
                playerRecord.levelIndex++;
                SceneManager.LoadScene(playerRecord.levels[playerRecord.levelIndex]);
            }
        }
    }  
}