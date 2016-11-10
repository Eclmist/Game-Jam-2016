using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Range(0, 1)]
    [Tooltip("Wow a tooltip")]
    public float speed;

    private Vector3 targetPos;

    public GameObject bullet;


    // Use this for initialization
    void Start ()
	{
	    targetPos = transform.position;
	}
	
	void FixedUpdate ()
	{
	    targetPos.x += Input.GetAxis("Horizontal");
        targetPos.z += Input.GetAxis("Vertical");
	    transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * speed);

	    if (Input.GetKey(KeyCode.Space))
	    {
	        Instantiate(bullet, transform.position, Quaternion.identity);
	    }
	}
}
