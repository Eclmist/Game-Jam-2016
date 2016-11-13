using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    SkipTimer shootingInterval = new SkipTimer(0.2f);
    AudioSource auds;
    private AudioClip shootSound;
    private AudioClip explosionSound;
    private AudioClip collectSound;

    static AudioManager Instance;

    void Awake()
    {
        Instance = this;
    }

	// Use this for initialization
	void Start ()
	{
	    auds = GetComponent<AudioSource>();
	    shootSound = Resources.Load<AudioClip>("Shoot");
        explosionSound = Resources.Load<AudioClip>("Explosion");
        collectSound = Resources.Load<AudioClip>("Collect");

    }

    // Update is called once per frame
    void Update () {

	    //if (shootingInterval.Check(Time.deltaTime))
	    //{
     //       PlayShoot();
	    //}
        
	}

    public static void PlayShoot()
    {
        Instance.auds.PlayOneShot(Instance.shootSound, 0.05F);
    }

    public static void PlayCollect()
    {
        Instance.auds.PlayOneShot(Instance.collectSound, 0.5F);
    }
    public static void PlayExplosion(float vol)
    {
        Instance.auds.PlayOneShot(Instance.explosionSound, vol);
    }

}
