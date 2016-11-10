using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantWarp : MonoBehaviour
{

    public float force;
    public float range;

    public ForceType forceType = ForceType.Explosive;

    private int counter = 0;

    void FixedUpdate()
    {
        counter ++;
        if (counter%2 == 0)
            Grid.Instance.ApplyForce(force, transform.position, range, forceType);
    }
}
