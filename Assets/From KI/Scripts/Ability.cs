using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public GameObject Bullet;
    SkipTimer interval = new SkipTimer(0.025f);

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (!interval.Check(Time.deltaTime))
	    {
	        return;
	    }
	    Instantiate(Bullet, transform.position, transform.rotation);
	    Instantiate(Bullet, transform.position, Quaternion.LookRotation(transform.forward * -1, Vector3.up));
	}
}
