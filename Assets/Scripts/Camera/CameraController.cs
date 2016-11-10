using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform target;
    public float damping;

    private Vector3 offset;

    private Vector3 targetPos;

	// Use this for initialization
	void Start ()
	{
	    offset = transform.position - target.position;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
	    targetPos = target.position + offset;

	    transform.position = Vector3.Lerp(transform.position, targetPos, damping);

	    transform.LookAt(Vector3.Lerp(target.position, transform.forward, 0.2F), Vector3.forward);
	}
}
