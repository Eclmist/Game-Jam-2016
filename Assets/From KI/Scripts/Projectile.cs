using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    public float speed;

    public GameObject InstantiateOnDeath;

    public GameObject target;
    private Rigidbody rigidbody;

	// Use this for initialization
	void Start ()
	{
	    rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
	    rigidbody.velocity = (target.transform.position - transform.position).normalized*speed;
        transform.LookAt(transform.position + rigidbody.velocity, transform.up);
	}

    void OnCollisionEnter(Collision collision)
    {
        enabled = false;

        if (InstantiateOnDeath != null)
        {
            Destroy(Instantiate(InstantiateOnDeath, transform.position, transform.rotation), 3);
        }

        rigidbody.velocity = Vector3.zero;

        foreach (var componentsInChild in GetComponentsInChildren<MeshRenderer>())
        {
            componentsInChild.enabled = false;
        }
        foreach (var componentsInChild in GetComponentsInChildren<Light>())
        {
            componentsInChild.enabled = false;
        }
        foreach (var componentsInChild in GetComponentsInChildren<ParticleSystem>())
        {
            componentsInChild.Stop();
        }
        foreach (var componentsInChild in GetComponentsInChildren<Rotator>())
        {
            componentsInChild.enabled = false;
        }
        Destroy(gameObject,2);
    }
}
