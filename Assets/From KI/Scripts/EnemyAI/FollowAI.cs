using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FollowAI : MonoBehaviour
{
    private Rigidbody rb;

	// Use this for initialization
	void Start ()
	{
	    rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
	{
        Vector3 tgtvec2;

        GameObject target = FollowAI.GetClosestPlayer(transform.position);
        if (target != null)
            tgtvec2 = FollowAI.GetClosestPlayer(transform.position).transform.position - transform.position;
        else
            tgtvec2 = transform.position;

        rb.velocity = (tgtvec2).normalized * 3;
	    transform.LookAt(transform.position + rb.velocity);
	}

    //TODO:: Replace with real function
    private static GameObject player;
    public static GameObject GetClosestPlayer(Vector3 v) {
        return GameManager.GetClosetPlayer(v);
    }
}
