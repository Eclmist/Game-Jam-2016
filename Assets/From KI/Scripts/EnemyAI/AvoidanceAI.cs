using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class AvoidanceAI : MonoBehaviour
{
    private Rigidbody rb;

    public float cooldown = 1;

    private float vectorScale;
    public Vector3 tgtvec, tgtvec1;

    // Use this for initialization
    void Start ()
	{
	    rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    cooldown -= Time.deltaTime;

        transform.position.Scale(new Vector3(1,0,1));

	    Collider[] colliders = Physics.OverlapSphere(transform.position, 5, LayerMask.GetMask("Bullet"));
	    Vector3 tgtvec2;

        GameObject target = FollowAI.GetClosestPlayer(transform.position);
        if (target != null)
            tgtvec2 = FollowAI.GetClosestPlayer(transform.position).transform.position - transform.position;
        else
            tgtvec2 = transform.position;

        tgtvec2.y = 0;
        tgtvec2.Normalize();
	    tgtvec2 *= 3;

        if (colliders.Length > 0 && cooldown < 0)
        {
            cooldown = 1;
            tgtvec1 = transform.position - colliders[0].transform.position;
            tgtvec1.y = 0;
            tgtvec1.Normalize();
            tgtvec1 *= 5;
        }

        tgtvec = Vector3.Lerp(tgtvec2, tgtvec1,cooldown);

	    vectorScale -= Time.deltaTime;

	    if (vectorScale <= -1) vectorScale = -1;

	    rb.velocity = tgtvec;
	}
}
