using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ParticleShooter : MonoBehaviour
{
    public GameObject target;
    public GameObject bulletTrail;

    public float initialMag = 20, deceleration = 10f;

    private DebugHelper.DebugData data;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    if (data == null)
	    {
	        data = DebugHelper.AddLine(gameObject, "", Color.blue);
	    }

	    float deltaColorHue = 0;
	    if (Input.GetKey(KeyCode.O)) deltaColorHue += Time.deltaTime;
	    if (Input.GetKey(KeyCode.P)) deltaColorHue -= Time.deltaTime;

        float h, s, v;
        Color color = data.color;
        Color.RGBToHSV(color, out h, out s, out v);
        h += deltaColorHue;
        if (h > 1) h -= 1;
        color = Color.HSVToRGB(h, s, v);
        data.color = color;

        data.debugString = String.Format("Hue : {0:0000}", h);

	    if (Input.GetMouseButton(0))
        {
            for (float i = 0.9f; i > Random.value;)
            {
                ParticleSystem.Particle p = Helper.CreateParticle(initialMag, 5, Helper.ClickPoint(), data.color, true);

                ParticleManager.particleQueue.Add(p);
            }
	    }

	    if (Input.GetMouseButtonDown(1))
	    {
            ParticleManager.attPt.Add(Helper.ClickPoint());
	    }

	    if (Input.GetMouseButtonDown(3))
	    {
	        if (target == null)
	        {
	            target = new GameObject();
                target.transform.position = Vector3.right* 100;
	            target.AddComponent<SphereCollider>();
	        }

	        GameObject go = Instantiate(bulletTrail);
	        go.transform.position = Helper.ClickPoint();
	        go.GetComponent<Projectile>().target = target;

	    }

	    if (Input.GetKeyDown(KeyCode.E))
	    {
	        Helper.SpawnExplosion(Helper.ClickPoint());
	    }

	    if (Input.GetKeyDown(KeyCode.Space)) ParticleManager.attPt.Clear();
	}

    
}
