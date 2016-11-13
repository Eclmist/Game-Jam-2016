using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Crystal : MonoBehaviour
{
    public float playerMagnetRadius;
    public float spawnForce;
    public float spawnRotationForce;

    private Rigidbody rb;

    float countdown = 2;

    // Material
    private Renderer ren;
    private Material mat;

	// Use this for initialization
	void Start ()
    {
        ren = GetComponent<Renderer>();
        mat = ren.material;

        rb = GetComponent<Rigidbody>();

        Vector2 randomDir = Random.insideUnitCircle;
        randomDir *= spawnForce;
        rb.AddForce(randomDir.x, 0, randomDir.y);

        Vector3 randomTorque = Random.insideUnitSphere.normalized;

        rb.AddTorque(Random.insideUnitSphere.normalized * spawnRotationForce);
	}
	
	// Update is called once per frame
	void Update ()
    {
        Collider[] players;
        players = Physics.OverlapSphere(transform.position, playerMagnetRadius, LayerMask.GetMask("Player"));

        if (players.Length > 0)
        {
            float shortestDist = float.MaxValue;
            Collider shortestTarget = null;

            foreach (Collider c in players)
            {
                if (Vector3.SqrMagnitude(transform.position - c.transform.position) < shortestDist)
                {
                    shortestDist = Vector3.SqrMagnitude(transform.position - c.transform.position);
                    shortestTarget = c;
                }
            }

            rb.velocity = Mathf.Clamp((playerMagnetRadius - Mathf.Sqrt(shortestDist)) * 3, rb.velocity.magnitude, 99) *
                (shortestTarget.transform.position - transform.position).normalized;
        }


        if (rb.velocity.magnitude <= 0.1F)
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            countdown = 2;
        }

        Color currentColor = mat.GetColor("_TintColor");
        currentColor.a = countdown / 2;
        mat.SetColor("_TintColor", currentColor);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            AudioManager.PlayCollect();
            GameManager.instance.AddScore(1);
            Destroy(gameObject);
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }
}
