using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable
{
    [Tooltip("Number of hits enemy can take. (Default = 1)")]
    public int health = 1;

    [Tooltip("Number of points awarded for killing this enemy. (Default = 50)")]
    public int points = 50;

    public GameObject crystal;

    public void TakeDamage()
    {
        health--;

        // Kill enemy
        if (health <= 0)
        {
            // GameManager.updateScore(points)
            Destroy(gameObject);

            for (int i = 0; i < points / 10; i++)
            {
                Instantiate(crystal, transform.position, Quaternion.identity);
            }

        }

        Helper.SpawnExplosion(transform.position);
    }



    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (transform.position.sqrMagnitude >
            GameManager.instance.ArenaWidth * GameManager.instance.ArenaWidth / 4 +
            GameManager.instance.ArenaHeight * GameManager.instance.ArenaHeight / 4)
        {
            Destroy(gameObject);
        }


    }

    void OnDestroy()
    {

        AudioManager.PlayExplosion(0.3F);
        Helper.SpawnExplosion(transform.position);

        SpawnController.Instance.EnemyDied();
        GameManager.instance.IncrementScoreModifier();
        GameManager.instance.IncrementKillCount();
    }

}
