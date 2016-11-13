using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Playing,
    Lost
}

public class GameManager : MonoBehaviour
{
	public static GameManager instance { get; private set; }

	[HideInInspector]
	public int Multiplier = 1, Score = 0;
	[HideInInspector]
	public GameObject[] Players = new GameObject[2];
	public GameObject[] Prefabs;

	public int StartingLifes = 5;
	public int ArenaWidth = 45, ArenaHeight = 20, ArenaPadding = 3;
	private int player1BulletTier = 0, player2BulletTier = 0;
	private int player1PickUpCounter, player2PickUpCounter;
	private int lifesLeft;

    private int killcount = 0;

	public GameObject AimerPrefab;

    public GameState currentGameState;

	protected void Awake ()
	{
        instance = this;
		lifesLeft = StartingLifes;
		GameObject[] playersInScene = GameObject.FindGameObjectsWithTag ("Player");
		for (int i = 0; i < playersInScene.Length; i++) {
			Players [(int)playersInScene [i].GetComponent<PlayerController> ().playerType] = playersInScene [i];
		}

        Prefabs = Resources.LoadAll<GameObject>("PlayerPrefabs");
	}

	protected void Start ()
	{		
		GameObject p1, p2;
		p1 = Instantiate (Prefabs [PlayerPrefs.GetInt ("Player1Choice")], Players [0].transform.position, Quaternion.identity) as GameObject;
		p1.GetComponent<PlayerController> ().playerType = PlayerEnum.Player1;

		p2 = Instantiate (Prefabs [PlayerPrefs.GetInt ("Player2Choice")], Players [1].transform.position, Quaternion.identity) as GameObject;
		p2.GetComponent<PlayerController> ().playerType = PlayerEnum.Player2;

		Destroy (Players [0]);
		Destroy (Players [1]);
		Players [0] = p1;
		Players [1] = p2;

        currentGameState = GameState.Playing;
	}

	public void PlayerDied (PlayerEnum type)
	{
		lifesLeft--;
		if (lifesLeft > 0) {
			RespawnPlayer (type);
		} else if (lifesLeft == -1) {
			LoseGame ();
		}
	}

	public void AddScore (int score)
	{
		Score += score * Multiplier;
	}

    public void IncrementScoreModifier()
    {
        Multiplier++;
    }

    public void IncrementKillCount()
    {
        killcount++;

        if (killcount >= 50 && player1BulletTier <= 0)
        {
            player1BulletTier = 1;
            player2BulletTier = player1BulletTier;
        }
        else if (killcount >= 200 && player1BulletTier <= 1)
        {
            player1BulletTier = 2;
            player2BulletTier = player1BulletTier;
        }
        else if (killcount >= 500 && player1BulletTier <= 2)
        {
            player1BulletTier = 3;
            player2BulletTier = player1BulletTier;
        }
    }

    public void LoseGame ()
	{
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject go in allEnemies)
        {
            Destroy(go);
        }

        currentGameState = GameState.Lost;

        GameEndController.instance.Activate ("You Lose");
	}

	public void WinGame ()
	{
		GameEndController.instance.Activate ("You Win");
	}

	private void RespawnPlayer (PlayerEnum type)
	{
		Players [(int)type] = Instantiate (Prefabs [PlayerPrefs.GetInt (type.ToString()+"Choice")]);
        Players[(int)type].GetComponent<PlayerController>().playerType = type;
        Players [(int)type].SetActive (true);
	}

	public static void OnBulletTierUp (PlayerEnum type)
	{
		switch (type) {
		case PlayerEnum.Player1:
			if (++instance.player1BulletTier >= AimerController.BulletTier.Length) {
				instance.player1BulletTier--;
			}
			break;
		case PlayerEnum.Player2:
			if (++instance.player2BulletTier >= AimerController.BulletTier.Length) {
				instance.player2BulletTier--;
			}
			break;
		}
	}

	public float GetLifesLeftPercentage ()
	{
		return ((float)lifesLeft / (float)StartingLifes);
	}

	public static int GetBulletTier (PlayerEnum type)
	{
		switch (type) {
		case PlayerEnum.Player1:
			return instance.player1BulletTier;
		case PlayerEnum.Player2:
			return instance.player2BulletTier;
		default:
			return 0;
		}
	}

	public static GameObject GetClosetPlayer (Vector3 v)
	{
		GameObject nearest = null;
       
		for (int i = 0; i < instance.Players.Length; i++) {
			if (instance.Players [i] != null) {
				if (nearest == null) {
					nearest = instance.Players [i];
				} else {
					if ((nearest.transform.position - v).magnitude > (instance.Players [i].transform.position - v).magnitude) {
						nearest = instance.Players [i];
					}
				}
			}
		}
		return nearest;
	}

	public static Vector3 ForceWithInBonds (Vector3 pos)
	{
		Vector3 toReturn = Vector3.zero;

		if ((-instance.ArenaWidth / 2) + instance.ArenaPadding < pos.x && pos.x < (instance.ArenaWidth / 2) - instance.ArenaPadding) {
			toReturn += new Vector3 (pos.x, 0, 0);
		} else if ((-instance.ArenaWidth / 2) + instance.ArenaPadding < pos.x) {
			toReturn += new Vector3 ((instance.ArenaWidth / 2) - instance.ArenaPadding, 0, 0);
		} else if (pos.x < (instance.ArenaWidth / 2) - instance.ArenaPadding) {
			toReturn += new Vector3 ((-instance.ArenaWidth / 2) + instance.ArenaPadding, 0, 0);
		}

		if ((-instance.ArenaHeight / 2) + instance.ArenaPadding < pos.z && pos.z < (instance.ArenaHeight / 2) - instance.ArenaPadding) {
			toReturn += new Vector3 (0, 0, pos.z);
		} else if ((-instance.ArenaHeight / 2) + instance.ArenaPadding < pos.z) {
			toReturn += new Vector3 (0, 0, (instance.ArenaHeight / 2) - instance.ArenaPadding);
		} else if (pos.z < (instance.ArenaHeight / 2) - instance.ArenaPadding) {
			toReturn += new Vector3 (0, 0, (-instance.ArenaHeight / 2) + instance.ArenaPadding);
		}

		return toReturn;
	}
}
