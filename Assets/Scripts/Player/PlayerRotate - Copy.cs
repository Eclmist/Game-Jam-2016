#region Using Directives

using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using GenLib.TestingAndProfiling;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

#endregion


public class PlayerRotate : MonoBehaviour
{
    //TODO: Target recognition
    private Vector3 target;

	[Header("Curves")]
    public AnimationCurve PitchLerpCurve;
    public float pitchScale = 3;
    public AnimationCurve RollLerpCurve;
    public float rollScale = 4;
    public AnimationCurve YawLerpCurve;
    public float yawScale = 2;
    
    [Header("Current Status (Do Not Change in Inspector)")]
    public float pitch, yaw, roll;

    private PlayerController pc;
    private Vector3 lastKnownTarget;

	private enum ShooterType
	{
		RaycastShooter,
		UnguidedProjectileShooter
	}

    #region Fighter Control functions

    private void PitchLerp(float targetAngle)
    {
        Pitch(PitchLerpCurve.Evaluate(Mathf.Abs(targetAngle/180))*pitchScale*(targetAngle < 0 ? -1 : 1)*Time.deltaTime);
    }

    private void RollLerp(float targetAngle)
    {
        Roll(RollLerpCurve.Evaluate(Mathf.Abs(targetAngle/180))*rollScale*(targetAngle < 0 ? -1 : 1)*Time.deltaTime);
    }

    private void YawLerp(float targetAngle)
    {
        if (targetAngle == 0f) return;
        Yaw(YawLerpCurve.Evaluate(Mathf.Abs(targetAngle/180))*yawScale*(targetAngle < 0 ? -1 : 1)*Time.deltaTime);
    }

    private void Pitch(float degAngle)
    {
        transform.Rotate(Vector3.right, degAngle);
    }

    private void Roll(float degAngle)
    {
        transform.Rotate(Vector3.forward, degAngle);
    }

    private void Yaw(float degAngle)
    {
        transform.Rotate(Vector3.up, degAngle);
    }

    #endregion

    #region Heading Calculation functions

    private float YawDeltaAngle(Vector3 target)
    {
        Vector3 targetVector = target - transform.position;
        Vector3 normalProjection = Vector3.Dot(targetVector, transform.up)/transform.up.magnitude*
                                   transform.up.normalized;
        Vector3 yawProjection = targetVector - normalProjection;
        float yawDeltaAngle = Vector3.Angle(transform.forward, yawProjection);
        Vector3 cross = Vector3.Cross(transform.forward, yawProjection);
        if (Vector3.Dot(cross.normalized, transform.up) <= 0) yawDeltaAngle *= -1;

        return yawDeltaAngle;
    }

    private float PitchDeltaAngle(Vector3 target)
    {
        Vector3 targetVector = target - transform.position;
        Vector3 normalProjection = Vector3.Dot(targetVector, transform.right)/transform.right.magnitude*
                                   transform.right.normalized;
        Vector3 pitchProjection = targetVector - normalProjection;
        float pitchDeltaAngle = Vector3.Angle(transform.forward, pitchProjection);
        Vector3 cross = Vector3.Cross(transform.forward, pitchProjection);
        if (Vector3.Dot(cross.normalized, transform.right) <= 0) pitchDeltaAngle *= -1;

        return pitchDeltaAngle;
    }

    private float RollDeltaAngle(Vector3 target)
    {
        Vector3 targetVector = target - transform.position;
        Vector3 normalProjection = Vector3.Dot(targetVector, transform.forward)/transform.forward.magnitude*
                                   transform.forward.normalized;
        Vector3 planeProjection = targetVector - normalProjection;
        float rollDeltaAngle = Vector3.Angle(transform.up, planeProjection);
        Vector3 cross = Vector3.Cross(transform.up, planeProjection);
        if (Vector3.Dot(cross.normalized, transform.forward) <= 0) rollDeltaAngle *= -1;
        return rollDeltaAngle;
    }

    #endregion

    void Start()
    {
        pc = GetComponent<PlayerController>();
        lastKnownTarget = transform.forward;
    }

    protected void Update()
    {
        float horizontal = pc.horizontal;
        float vertical = pc.vertical;

        if (Mathf.Abs(horizontal) < 0.001F && Mathf.Abs(vertical) < 0.001F)
        {
            target = lastKnownTarget;
        }
        else
        {
            lastKnownTarget = transform.position + new Vector3(horizontal, 0, vertical).normalized;
            target = lastKnownTarget;
        }


		pitch = PitchDeltaAngle(target);
        yaw = YawDeltaAngle(target);
        roll = RollDeltaAngle(target);

        TrackToTarget();

        //Correct roll
        float rollDelta = RollDeltaAngle(Vector3.up * 20);
        if (rollDelta > 90) Roll(rollScale * Time.deltaTime * Mathf.Sign(rollDelta));
        else RollLerp(rollDelta);
    }



    public void TrackToTarget()
    {
        YawLerp(Mathf.Abs(yaw) < 1 ? 0 : yaw);
        RollLerp(Mathf.Abs(roll) < 1 ? 0 : roll);
        PitchLerp(Mathf.Abs(pitch) < 1 ? 0 : pitch);
    }

    //public void TrackToTarget()
    //{
    //    YawLerp(yaw);
    //    RollLerp(roll);
    //    PitchLerp(pitch);
    //}
    protected void OnDrawGizmos()
    {

        Gizmos.color = Color.magenta;
        //foreach (var o in targets)
        {
            Gizmos.DrawSphere(target, 0.2f);
        }

        return;
    }



}
