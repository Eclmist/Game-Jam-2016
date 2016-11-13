using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Range(0, 10)]
    public float speed;

    [Tooltip("Number of times bullet should pierce. (Default: 0)")]
    public int pierceCount = 0;

    public GameObject trail;

    // Use this for initialization
	void Start ()
    {
	}
	
	void FixedUpdate ()
    {
        transform.position += transform.forward * speed;
    }

    void OnTriggerEnter(Collider other)
    {

        IDamagable damagableEnemy = other.GetComponent<IDamagable>();

        // is enemy
        if (damagableEnemy != null)
        {
            damagableEnemy.TakeDamage();

            // Kill bullet
            if (pierceCount <= 0)
            {
                DestroyThisBullet();
            }
            else
            {
                pierceCount--;
            }
        }
        // hits wall
        else
        {
            DestroyThisBullet();
        }
        
    }

    void DestroyThisBullet()
    {
        if (trail != null)
        {
            trail.transform.SetParent(null);
        }

        AudioManager.PlayExplosion(0.1F);
        Helper.SpawnSmallExplosion(0.3F,transform.position, Color.yellow);
        Destroy(gameObject);

    }
}
