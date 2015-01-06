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

		public enum TeamColours //This is just what team you are on
		{
				red,
				blue
		}
		void Awake () //Just setting up our static variable of this so that we don't need to drag in components
		{
				scores = this;
		}

		void Start ()
		{
				Initialize (); //Initializing things
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

		public void Initialize ()
		{
				Text[] txt = new Text[4]; //Foreach player if they key kills || deaths || assists does not exist then create one and set it to 0
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

		public void ClearData () //This is a scary function since it will reset scores but does display a warning incase it goes off
		{
				Debug.LogWarning ("RESETTING ALL DATA");
				foreach (Player p in players) {
						p.stats.Clear ();
				}
		}
	//The next three functions are all about getting data without going through the list + dictionary
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
	//This can be a different script but i decided since it is just for this script to reduce the load of scripts i added this here
	[System.Serializable]
	public class Player 
	{
		public string name;
		public int playerId;
		public GameObject prefab;
		public TeamColours playerTeam;
		public static int numberOfPlayers = 0;
		public Dictionary<string, int> stats = new Dictionary<string, int> (); 
		//Dictionaries are like lists but run slightly different on a key basis
		
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
			if (team == TeamColours.blue) { //if blue set up for a blue
				prefab = (GameObject)GameObject.Instantiate (scores.prefabB);
				prefab.name = name;
				prefab.transform.SetParent (scores.parentB);
			} else if (team == TeamColours.red) { //else set up for a red
				prefab = (GameObject)GameObject.Instantiate (scores.prefabR);
				prefab.name = name;
				prefab.transform.SetParent (scores.parentR);
			}
		}
	}
}
