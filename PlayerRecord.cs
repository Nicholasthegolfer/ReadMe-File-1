using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Security.Cryptography;

public class PlayerRecord : MonoBehaviour
{
   public List<Player> playerList;
   public string[] levels;
   public Color[] playerColours;
   // public List<Color> playerColours;
   [HideInInspector] public int levelIndex;


   public void OnEnable()
   {
      playerList = new List<Player>();
      DontDestroyOnLoad(gameObject);
   }
 
   public void AddPlayer(string name)
   {
      playerList.Add(new Player(name, playerColours[playerList.Count], levels.Length));
   }


   public void AddPutts(int playerIndex, int puttCount)
   {
    playerList[playerIndex].putts[levelIndex] = puttCount;
   }


   public List<Player> GetScoreboardList()
   {
      foreach (var player in playerList)
      {
         foreach (var puttScore in player.putts)
         {
            player.totalPutts += puttScore;
         }
      }
      return (from p in playerList orderby p.totalPutts select p).ToList();
   }

   public class Player
   {
      public string name;
      public Color colour;
      public int[] putts;
      public int totalPutts;

      public Player(string newName, Color newColor, int levelCount)
      {
         name = newName;
         colour = newColor;
         putts = new int[levelCount];
      }
   }
}


