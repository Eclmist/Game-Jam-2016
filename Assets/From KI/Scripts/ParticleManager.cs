using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using GenLib.TestingAndProfiling;
using UnityEngine;
using UnityEngine.Assertions;

public class ParticleManager : MonoBehaviour
{
    public static List<ParticleSystem.Particle> particles = new List<ParticleSystem.Particle>();
    public static List<ParticleSystem.Particle> particleQueue = new List<ParticleSystem.Particle>();
    public static List<Vector3> attPt = new List<Vector3>();

    private ParticleSystem ps;
    
    public float partCount,attPtC;

    public float xBound = 100, zBound = 100, xBoundL = 0, zBoundL = 0;

    public float perSecondDamping;

    bool _threadRunning;
    Thread _thread;
    public float attrForce;
    //AutoResetEvent are = new AutoResetEvent(false);

    void Start()
    {
        // Begin our heavy work on a new thread.
        _thread = new Thread(ThreadedWork);
        _thread.Start();

        ps = GetComponent<ParticleSystem>();

    }

    void Update()
    {
        ps.SetParticles(particles.ToArray(), particles.Count);
        partCount = particles.Count;
        attPtC = attPt.Count;
        //are.Set();
    }

    void ThreadedWork()
    {
        _threadRunning = true;
        bool workDone = false;

        Stopwatch time = new Stopwatch();
        time.Start();

        float lastTime = time.ElapsedMilliseconds, deltaTime;

        print("started");

        // This pattern lets us interrupt the work at a safe point if neeeded.
        try
        {

            while (_threadRunning && !workDone)
            {
                //are.WaitOne();
                //Calculate delta Time
                deltaTime = time.ElapsedMilliseconds - lastTime;
                lastTime = time.ElapsedMilliseconds;

                deltaTime /= 1000;
                List<ParticleSystem.Particle> particleList;
                List<Vector3> lAttPt;

                try
                {
                    particleList = new List<ParticleSystem.Particle>(particles);
                    lAttPt = new List<Vector3>(attPt);
                    particleList.AddRange(particleQueue);
                }
                catch (Exception)
                {
                    continue;
                }
                
                particleQueue.Clear();

                for (int i = 0; i < particleList.Count; i++)
                {
                    var p = particleList[i];
//print(neonParticle.velocity.magnitude - deltaTime*neonParticle.decelerationPerSecond);

                    //if (particleList[i] == null || particleList[i].Equals(null)) continue;

                    float newMag = p.velocity.magnitude - deltaTime*perSecondDamping;
                    p.remainingLifetime -= deltaTime;
                    if (p.remainingLifetime<= 0 || newMag <= 0)
                    {
                        particleList.RemoveAt(i--);
                        continue;
                    }
                    p.velocity = p.velocity.normalized*newMag;

                    if (p.position.z < 0 || p.position.z > zBound)
                    {
                        Vector3 v = p.velocity;
                        v.z *= -1;
                        p.velocity = v;
                    }
                    if (p.position.x < 0 || p.position.x > xBound)
                    {
                        Vector3 v = p.velocity;
                        v.x *= -1;
                        p.velocity = v;
                    }
                    foreach (var ap in lAttPt)
                    {
                        Vector3 v = ap - p.position;
                        p.velocity += v.normalized * Mathf.Clamp(Mathf.Pow(0.9f , v.magnitude), 0.1f,2) * 600 * attrForce * deltaTime;
                    }

                    //p.velocity = Vector3.ClampMagnitude(p.velocity, 3);

                    Vector3 newPos = new Vector3(Mathf.Clamp(p.position.x, xBoundL, xBound), 0, Mathf.Clamp(p.position.z, zBoundL, zBound));
                    p.position = newPos;
                    
                    p.position += p.velocity*deltaTime;

                    Color c = p.startColor;
                    c.a = Mathf.Clamp01(p.remainingLifetime * newMag / 40);
                    p.startColor = c;

                    particleList[i] = p;
                }

                particles = new List<ParticleSystem.Particle>(particleList);
            }
        }
        catch (Exception e)
        {
            print(e);
        }

        print("ended");

        _threadRunning = false;
    }

    void OnDisable()
    {


        // If the thread is still running, we should shut it down,
        // otherwise it can prevent the game from exiting correctly.
        if (_threadRunning)
        {
            // This forces the while loop in the ThreadedWork function to abort.
            _threadRunning = false;
            //are.Set();

            // This waits until the thread exits,
            // ensuring any cleanup we do after this is safe. 
            _thread.Abort();
        }

        // Thread is guaranteed no longer running. Do other cleanup tasks.
    }

    void OnDrawGizmos()
    {
        foreach (var vector3 in attPt)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(vector3, 2);
        }
    }
}
