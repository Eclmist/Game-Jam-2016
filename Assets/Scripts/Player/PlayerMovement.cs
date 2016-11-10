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

	    if (Input.GetKeyDown(KeyCode.Space))
	    {
            float mag;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, transform.position.y + 1);
            plane.Raycast(ray, out mag);
            Vector3 point = ray.origin + ray.direction * mag;


            Vector3 lookVector = point - transform.position;
            lookVector.y = 0;

            Instantiate(bullet, transform.position, Quaternion.LookRotation(lookVector, Vector3.up));
	    }
	}
}
