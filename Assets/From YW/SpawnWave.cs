using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SpawnType
{
	Random,
	Line,
	Grid,
	Edge
}

public class SpawnWave : MonoBehaviour
{
	//All Spawn Settings
	public int SpawnAmount = 50;
	public float SpawnInterval = 0.5f;

	//Spawn Line Settings
	public int AmountPerLine = 20;
	public int OrientationSeed = 85956;
	private System.Random orientationRandom;

	//Spawn Edge Settings
	public int AmountPerEdgeSeed = 4554;
	public int SpawnPosiSeed = 7864;
	private System.Random amountPerEdgeRandom, spawnPosiRandom;

	//Spawn Variables
	public SpawnType Type;
	public int XSeed = 8754, ZSeed = 2558;
	private System.Random xRandom, zRandom;

	//Enemy Prefab
	public GameObject EnemyPrefab;

	//Variables
	[HideInInspector]
	public bool StartSpawn = false;

	private int enemiesLeft;
	private int spawnCounter = 0;
	private float xWidth, zHeight;
	private float spawnTimer = 0;

	protected void Awake ()
	{
		xRandom = new System.Random (XSeed);
		zRandom = new System.Random (ZSeed);
		orientationRandom = new System.Random (OrientationSeed);
		amountPerEdgeRandom = new System.Random (AmountPerEdgeSeed);
		spawnPosiRandom = new System.Random (SpawnPosiSeed);
	}

	protected void Start ()
	{
		xWidth = GameManager.instance.ArenaWidth / 2;
		zHeight = GameManager.instance.ArenaHeight / 2;
	}

	// Update is called once per frame
	protected void Update ()
	{
		if (StartSpawn == true) {
			spawnTimer += Time.deltaTime;

			if (spawnTimer > SpawnInterval && spawnCounter < SpawnAmount) {
				spawnTimer = 0;
				Spawn ();
			}
		}
	}

	private void Spawn ()
	{
        if (GameManager.instance.currentGameState == GameState.Playing)
        {
            switch (Type)
            {
                case SpawnType.Edge:
                    SpawnEdge();
                    break;
                case SpawnType.Grid:
                    SpawnGrid();
                    break;
                case SpawnType.Line:
                    SpawnLine();
                    break;
                case SpawnType.Random:
                    SpawnRandom();
                    break;
            }
        }
	}

	private void SpawnRandom ()
	{
		var xPoint = (float)xRandom.NextDouble () * xWidth * 2;
		var zPoint = (float)zRandom.NextDouble () * zHeight * 2;

		xPoint -= (int)xWidth;
		zPoint -= (int)zHeight;
		spawnCounter++;
		enemiesLeft++;
		GameObject tmpEnemy = Instantiate (EnemyPrefab, new Vector3 (xPoint, 0, zPoint), Quaternion.identity) as GameObject;
		 
	}

	private void SpawnLine ()
	{
		//0 is Hori, 1 is Verti
		GameObject tmpEnemy;
		var orientation = orientationRandom.Next (0, 2);
		var xPoint = (float)xRandom.NextDouble () * xWidth * 2;
		var zPoint = (float)zRandom.NextDouble () * zHeight * 2;

		xPoint -= (int)xWidth;
		zPoint -= (int)zHeight;

		if (orientation == 0) {
			//X consistent
			for (int i = 0; i < AmountPerLine; i++) {
				tmpEnemy = Instantiate (EnemyPrefab,
                    new Vector3 (xPoint, 0, 0.25f + (zHeight * 2 / AmountPerLine * i - zHeight)),
                    Quaternion.LookRotation(Vector3.right)) as GameObject;
				 
			}
		} else {
			//Z consistent
			for (int i = 0; i < AmountPerLine; i++) {
				tmpEnemy = Instantiate (EnemyPrefab, new Vector3 (0.25f + (xWidth * 2 / AmountPerLine * i - xWidth),
                    0, zPoint), Quaternion.identity) as GameObject;
				 
			}
		}
		enemiesLeft += AmountPerLine;
		spawnCounter += AmountPerLine;
	}

	private void SpawnGrid ()
	{
		//Get 1 point
		var xPoint = (float)xRandom.NextDouble () * xWidth * 2;
		var zPoint = (float)zRandom.NextDouble () * zHeight * 2;

		xPoint -= (int)xWidth;
		zPoint -= (int)zHeight;

		//Flip to get the other point
		var xPointOther = xPoint * -1;
		var zPointOther = zPoint * -1;

		//To get grid
		GameObject tmpEnemy;

		//FirstPoint
		//X consistent
		for (int i = 0; i < AmountPerLine; i++) {
			tmpEnemy = Instantiate (EnemyPrefab, new Vector3 (xPoint, 0, 0.25f + (zHeight * 2 / AmountPerLine * i - zHeight)), Quaternion.identity) as GameObject;
		}
		//Z consistent
		for (int i = 0; i < AmountPerLine; i++) {
			tmpEnemy = Instantiate (EnemyPrefab, new Vector3 (0.25f + (xWidth * 2 / AmountPerLine * i - xWidth), 0, zPoint), Quaternion.identity) as GameObject; 
		}

		//Other Point
		//X consistent
		for (int i = 0; i < AmountPerLine; i++) {
			tmpEnemy = Instantiate (EnemyPrefab, new Vector3 (xPointOther, 0, 0.25f + (zHeight * 2 / AmountPerLine * i - zHeight)), Quaternion.identity) as GameObject;
			 
		}
		//Z consistent
		for (int i = 0; i < AmountPerLine; i++) {
			tmpEnemy = Instantiate (EnemyPrefab, new Vector3 (0.25f + (xWidth * 2 / AmountPerLine * i - xWidth), 0, zPointOther), Quaternion.identity) as GameObject;
			 
		}
		spawnCounter += AmountPerLine * 4;
		enemiesLeft += AmountPerLine * 4;
	}

	private void SpawnEdge ()
	{
		//0 is Hori, 1 is Verti
		//0 is bottom, 1 is top
		var amount = SpawnAmount / 6;
		var orientation = orientationRandom.Next (0, 2);
		var spawnPosi = spawnPosiRandom.Next (0, 2);
		var xPoint = 5.0f;
		var zPoint = 5.0f;

		if (spawnPosi == 0) {
			xPoint = -xWidth + 0.25f;
			zPoint = -zHeight + 0.25f;
		} else {
			xPoint = xWidth - 0.25f;
			zPoint = zHeight - 0.25f;
		}

		GameObject tmpEnemy;

		if (orientation == 0) {
			//X consistent
			for (int i = 0; i < amount; i++) {
				tmpEnemy = Instantiate (EnemyPrefab, new Vector3 (xPoint, 0, 0.25f + (zHeight * 2 / amount * i - zHeight)), Quaternion.identity) as GameObject;
				 
			}
		} else {
			//Z consistent
			for (int i = 0; i < amount; i++) {
				tmpEnemy = Instantiate (EnemyPrefab, new Vector3 (0.25f + (xWidth * 2 / amount * i - xWidth), 0, zPoint), Quaternion.identity) as GameObject;
				 
			}
		}

		enemiesLeft += amount;
		spawnCounter += amount;
	}

	public void EnemyDied ()
	{
		enemiesLeft--;
	}

	public bool CanMoveToNextWave ()
	{
		if (SpawnAmount <= spawnCounter) {
			if (enemiesLeft == 0) {
				return true;
			}
		}
		return false;
	}

	//For repeating the last wave
	public void SpawnThisAgain ()
	{
		spawnCounter = 0;	
		xRandom = new System.Random (XSeed);
		zRandom = new System.Random (ZSeed);
		orientationRandom = new System.Random (OrientationSeed);
		amountPerEdgeRandom = new System.Random (AmountPerEdgeSeed);
		spawnPosiRandom = new System.Random (SpawnPosiSeed);
	}
}
