using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomAI : MonoBehaviour
{
    public float flipAngles = 180;

    private float timer;

    private Rigidbody rb;
    private Direction d;

    private Vector3 fromPosition;
    private Quaternion fromQuaternion;
    
    Vector3 dirVec = Vector3.zero;
    Quaternion offsetQuaternion = Quaternion.identity;

    enum Direction
    {
        up,down,left,right,NULL
    }

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        fromPosition = transform.position;
        fromQuaternion = transform.rotation;
    }

    // Update is called once per frame
    void Update ()
    {
        timer += Time.deltaTime * 2;

        bool upBlocked = Physics.Raycast(transform.position, Vector3.forward, 1.5f, LayerMask.GetMask("ArenaBounds"));
        bool dwBlocked = Physics.Raycast(transform.position, -Vector3.forward, 1.5f, LayerMask.GetMask("ArenaBounds"));
        bool lfBlocked = Physics.Raycast(transform.position, -Vector3.right, 1.5f, LayerMask.GetMask("ArenaBounds"));
        bool rtBlocked = Physics.Raycast(transform.position, Vector3.right, 1.5f, LayerMask.GetMask("ArenaBounds"));



        if (timer > 1.6f)
        {
            d = (Direction)Random.Range(0, 4);
            fromQuaternion = transform.rotation;
            fromPosition = transform.position;
            timer = 0;

            switch (d)
            {
                case Direction.up:
                    dirVec = Vector3.forward;
                    offsetQuaternion = Quaternion.Euler(flipAngles, 0, 0);
                    if (upBlocked) timer = 2;
                    break;
                case Direction.down:
                    dirVec = -Vector3.forward;
                    offsetQuaternion = Quaternion.Euler(-flipAngles, 0, 0);
                    if (dwBlocked) timer = 2;
                    break;
                case Direction.left:
                    dirVec = -Vector3.right;
                    offsetQuaternion = Quaternion.Euler(0, 0, flipAngles);
                    if (lfBlocked) timer = 2;
                    break;
                case Direction.right:
                    dirVec = Vector3.right;
                    offsetQuaternion = Quaternion.Euler(0, 0, -flipAngles);
                    if (rtBlocked) timer = 2;
                    break;
                case Direction.NULL:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return;
        }

        transform.rotation = Quaternion.Lerp(fromQuaternion, offsetQuaternion * fromQuaternion, timer);
        transform.position = Vector3.Lerp(fromPosition, fromPosition + dirVec, timer);
        

    }
}
