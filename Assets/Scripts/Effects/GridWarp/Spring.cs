using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring
{

    public PointMass left;
    public PointMass right;
    public float targetLength;
    public float stiffness;
    public float damping;

    public Spring(PointMass l, PointMass r, float s, float d)
    {
        left = l;
        right = r;
        targetLength = (Vector3.Magnitude(l.position - r.position)*0.95f);
        stiffness = s;
        damping = d;
    }

    public void Update()
    {
        Vector3 targetVector = left.position - right.position;

        float length = targetVector.magnitude;

        if (length > targetLength)
        {
            targetVector = (targetVector / length) * (length - targetLength);
            Vector3 dv = right.velocity - left.velocity;
            Vector3 force = stiffness * targetVector - dv * damping;

            left.ApplyForce(-force);
            right.ApplyForce(force);
        }
    }
}
