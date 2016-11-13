using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleControlledCharacter : MonoBehaviour
{
    public float movementScale;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	    transform.position += (Input.GetAxis("Horizontal") * Vector3.right + Input.GetAxis("Vertical") * Vector3.forward) * Time.deltaTime * movementScale;
	}
}
