using System.Collections.Generic;
using TMPro;
//using UnityEditor.Build.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreboadManger : MonoBehaviour
{
    public TextMeshProUGUI names, putts;

    private PlayerRecord playerRecord;
    
    //Scoreboard for all players with the total number of putts being made.
    void Start()
    {
        playerRecord = GameObject.Find("Player Record").GetComponent<PlayerRecord>();
        names.text = "";
        putts.text = "";
        foreach (var player in playerRecord.GetScoreboardList())
        {
            names.text += player.name + "\n";
            putts.text += player.totalPutts + "\n";
        }
    }

    void Update()
    {
        putts.fontSize = names.fontSize;
    }

    public void ButtonReturnMenu()
    {
        Destroy(playerRecord.gameObject);
        SceneManager.LoadScene("Menu");
    }
}
