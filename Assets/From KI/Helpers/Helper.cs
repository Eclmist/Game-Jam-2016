using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper : MonoBehaviour
{

    public static GameObject explosion;

    public static ParticleSystem.Particle CreateParticle(float speed, float lifetime, Vector3 initialPosition, Color color, bool colorDeviation = false)
    {
        if (colorDeviation)
        {
            float h, s, v;
            Color.RGBToHSV(color, out h, out s, out v);
            h += Random.Range(-0.03f, 0.03f);
            color = Color.HSVToRGB(h, s, v);
        }

        ParticleSystem.Particle p = new ParticleSystem.Particle();
        p.position = initialPosition;
        p.startSize = 0.5f;
        p.startColor = color;
        p.startLifetime = lifetime;
        p.remainingLifetime = lifetime;
        p.velocity = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized * speed;
        return p;
    }

    public static ParticleSystem.Particle CreateParticle(float speed, float lifetime, Vector3 initialPosition)
    {
        return CreateParticle(speed, lifetime, initialPosition, Random.ColorHSV(0, 1, 1, 1, 1, 1, 1, 1));
    }

    public static Vector3 ClickPoint(float y = 0)
    {
        float mag;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.up*y);
        plane.Raycast(ray, out mag);

        Vector3 point = ray.origin + ray.direction * mag;
        
        return point;
    }

    public static void SpawnExplosion(Vector3 position)
    {
        SpawnExplosion(position, Random.ColorHSV(0, 1, 1, 1, 1, 1, 1, 1));
    }

    public static void SpawnExplosion(Vector3 position, Color c)
    {
        if(explosion == null)
            explosion = Resources.Load<GameObject>("Explosion");

        GameObject ob = Instantiate(explosion, position, Quaternion.identity);
        ob.GetComponent<ParticleSystem>().startColor = c;

        Destroy(ob, 2);
    }
}
