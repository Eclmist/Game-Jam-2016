using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class LineAI : MonoBehaviour
{
    public float lookAheadDistance;
    public float speed;
    public float damp = 2;

    private Rigidbody rb;
    
    private Vector3 forwardVec;
    private bool wasHittingWall;
    private float vectorScale = 1;

    private int state = 1;

    private float timer = 0;

    // Use this for initialization
    void Start()
    {
        forwardVec = transform.forward;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update ()
    {
        bool isHittingWall = Physics.Raycast(new Ray(transform.position, forwardVec * state), lookAheadDistance, LayerMask.GetMask("ArenaBounds"));
        
        timer += Time.deltaTime;

        rb.velocity = forwardVec*speed*vectorScale*state;

        transform.LookAt(transform.position + rb.velocity);
        
        if (isHittingWall)
        {
            vectorScale -= Mathf.Lerp(1,0, timer);

            //vectorScale = Mathf.SmoothStep(0, 0, timer);

            if (timer >= 2)
            {
                timer = 0;
                vectorScale = 1;
                state = (state == -1) ? 1 : -1;
            }
        }

        return;

        if (isHittingWall & !wasHittingWall)
        {
            wasHittingWall = true;
        }
        wasHittingWall = isHittingWall;
    }
}
