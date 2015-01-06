using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour
{
		public List<Player> players = new List<Player> ();
		public Transform parentR;
		public Transform parentB;
		public GameObject prefabB;
		public GameObject prefabR;
		public static ScoreManager scores;

		public enum TeamColours
		{
				red,
				blue
		}
		void Awake ()
		{
				scores = this;
		}

		void Start ()
		{
				Initialize ();
				AddPlayer ("Shadow", TeamColours.blue);
				AddPlayer ("Fang", TeamColours.red);
				IncrementStats (0, "Kills", 3);
				IncrementStats (0, "Kills", -1);
				SetStats (1, "Deaths", 3);
				
		}

		[System.Serializable]
		public class Player
		{
				public string name;
				public int playerId;
				public GameObject prefab;
				public TeamColours playerTeam;
				public static int numberOfPlayers = 0;
				public Dictionary<string, int> stats = new Dictionary<string, int> ();

				public Player (string Name, TeamColours team)
				{
						name = Name;
						playerId = numberOfPlayers;
						playerTeam = team;
						numberOfPlayers++;
						stats.Clear ();
						stats.Add ("Kills", 0);
						stats.Add ("Deaths", 0);
						stats.Add ("Assists", 0);
						if (team == TeamColours.blue) {
								prefab = (GameObject)GameObject.Instantiate (scores.prefabB);
								prefab.name = name;
								prefab.transform.SetParent (scores.parentB);
						} else if (team == TeamColours.red) {
								prefab = (GameObject)GameObject.Instantiate (scores.prefabR);
								prefab.name = name;
								prefab.transform.SetParent (scores.parentR);
						}
				}
		}
		public void AddPlayer (string name, TeamColours team)
		{
				players.Add (new Player (name, team));
				Initialize ();
		}

		public void SetStats (int playerID, string stat, int amount)
		{
				players [playerID].stats [stat] = amount;
				Initialize ();
		}

		public void IncrementStats (int playerID, string stat, int amount)
		{
				players [playerID].stats [stat] += amount;
				Initialize ();
		}

		public int GetStatData (int playerID, string stat)
		{
				return players [playerID].stats [stat];
		}

		public string GetName (int playerID)
		{
				return players [playerID].name;
		}

		public TeamColours GetTeam (int playerID)
		{
				return players [playerID].playerTeam;
		}

		public void Initialize ()
		{
				Text[] txt = new Text[4];
				foreach (Player p in players) {
						if (!p.stats.ContainsKey ("Kills")) {
								p.stats.Add ("Kills", 0);
								if (!p.stats.ContainsKey ("Deaths")) {
										p.stats.Add ("Deaths", 0);
										if (!p.stats.ContainsKey ("Assists")) {
												p.stats.Add ("Assists", 0);
										}
								}
						}
						txt = p.prefab.GetComponentsInChildren<Text> (); //Name, Kills, Deaths, Assists order for text components
						txt [0].text = p.name;
						txt [1].text = GetStatData (p.playerId, "Kills").ToString ();
						txt [2].text = GetStatData (p.playerId, "Deaths").ToString ();
						txt [3].text = GetStatData (p.playerId, "Assists").ToString ();
				}
		}

		public void ClearData ()
		{
				Debug.LogWarning ("RESETTING ALL DATA");
				foreach (Player p in players) {
						p.stats.Clear ();
				}
		}
}
