using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnController : MonoBehaviour
{
	public static SpawnController Instance { get; private set; }

	public List<SpawnWave> Spawner = new List<SpawnWave> ();
	public bool Endless;

	private int WaveCounter = 0;

	protected void Awake ()
	{
		Instance = this;
	}

	protected void Start ()
	{
		Spawner [WaveCounter].StartSpawn = true;
	}

	protected void Update ()
	{
		if (Spawner [WaveCounter].CanMoveToNextWave ()) {
			if (++WaveCounter < Spawner.Count) {
				Spawner [WaveCounter].StartSpawn = true;
			} else if (Endless == true) {
				WaveCounter = Spawner.Count - 1;
				Spawner [WaveCounter].SpawnThisAgain ();
			} else {
				GameManager.instance.WinGame ();
			}
		}
	}

	public void EnemyDied ()
	{
		Spawner [WaveCounter].EnemyDied ();
	}
}
