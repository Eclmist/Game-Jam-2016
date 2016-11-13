using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CameraController : MonoBehaviour
{

    public Transform target1;
    public Transform target2;

    public float damping;

    private Vector3 offset;
    private float originalY;

    private Vector3 targetPos, midpoint;

    bool singlePlayer = false;

	// Use this for initialization
	void Start ()
	{
        originalY = transform.position.y;

        if (GameManager.instance.Players[1] != null && GameManager.instance.Players[0] != null)
        {
            target1 = GameManager.instance.Players[0].transform;
            target2 = GameManager.instance.Players[1].transform;
        }
        else if (GameManager.instance.Players[0] != null)
        {
            singlePlayer = true;
            target1 = GameManager.instance.Players[0].transform;
        }
        else
        {
            throw new MissingReferenceException();
        }

        if (singlePlayer)
            midpoint = target1.position;
        else
            midpoint = (target1.position + target2.position) / 2;

        offset = transform.position - midpoint;
        offset.x = 0;
        offset.z = 0;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
        float y = originalY;

        if (singlePlayer)
            midpoint = target1.position;
        else
        {
            if (!target1 || !target2)
            {
                try
                {
                    target1 = GameManager.instance.Players[0].transform;
                    target2 = GameManager.instance.Players[1].transform;
                }
                catch { }

            }

            if (target1 != null && target2 != null)
            {
                midpoint = (target1.position + target2.position) / 2;

                y += Vector3.SqrMagnitude(target1.position - target2.position) * 0.006F;

                targetPos = midpoint + offset;
                targetPos.y = y;

                transform.position = Vector3.Lerp(transform.position, targetPos, damping);
            }
            else if (target1 != null)
            {
                midpoint = target1.position;

                targetPos = midpoint + offset;

                transform.position = Vector3.Lerp(transform.position, targetPos, damping);

            }
            else if (target2 != null)
            {
                midpoint = target2.position;

                targetPos = midpoint + offset;

                transform.position = Vector3.Lerp(transform.position, targetPos, damping);

            }
        }


	    //transform.LookAt(Vector3.Lerp(target.position, transform.forward, 0.2F), Vector3.forward);
	}
}
