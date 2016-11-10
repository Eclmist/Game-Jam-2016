using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointMass
{
    public Vector3 position;
    public Vector3 velocity;
    public float inverseMass;

    private Vector3 acceleration;
    private float damping = 0.98F;

    public PointMass()
    {
        position = Vector3.zero;
        inverseMass = 0;
        acceleration = Vector3.zero;
        velocity = Vector3.zero;
    }

    public PointMass(Vector3 pos, float invMass)
    {
        position = pos;
        inverseMass = invMass;
        acceleration = Vector3.zero;
        velocity = Vector3.zero;
    }

    public void ApplyForce(Vector3 force)
    {
        acceleration += force* inverseMass;
    }

    public void Translate(Vector3 amount)
    {
        position += amount;
    }

    public void IncreaseDamping(float factor)
    {
        damping *= factor;
    }

    public void Update()
    { 
        velocity += acceleration;
        position += velocity;
        acceleration = Vector3.zero;

        if (velocity.magnitude < 0.001F) velocity = Vector3.zero;

        velocity *= damping;
        damping = 0.98F;

        //position.y = Mathf.Clamp(position.y, 0, 2);
    }

}
