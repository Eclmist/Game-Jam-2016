using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultBullet : MonoBehaviour {

    private static float speed = 0.2F;
    private static float force = 2;
    private static float range = 1;


    // Use this for initialization
    void Start () {
        Destroy(gameObject, 2);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.position += transform.forward * speed;

    }
}
