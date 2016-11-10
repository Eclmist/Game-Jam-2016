using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class Grid : MonoBehaviour
{
    [Header("Temp")]
    public Transform player;
    public float force1, range1;
    public float force2, range2;
    public float force3, range3;

    [Header("Grid Size")]
    public float gridWidth;
    public float gridHeight;
    public int numRows;
    public int numCols;

    [Header("Spring Properties")]
    public float stiffness;
    public float damping;


    public static Grid Instance;

    private float rowCellSize;
    private float colCellSize;

    private List<Spring> springs = new List<Spring>();
    private PointMass[,] points;
    private PointMass[,] fixedPoints;

    private Thread thread;
    private bool threadStopped = false;
    private AutoResetEvent resetEvent;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        points = new PointMass[numRows, numCols];

        fixedPoints = new PointMass[numRows, numCols];

        float halfWidth = gridWidth/2;
        float halfHeight = gridHeight/2;

        float yPos = transform.position.y;

        rowCellSize = (gridHeight/(numRows - 1));
        colCellSize = (gridWidth/(numCols - 1));
        for (int rows = 0; rows < numRows; rows ++)
        {
            for (int cols = 0; cols < numCols; cols++)
            {
                float xPos = transform.position.x - halfWidth + colCellSize * cols;
                float zPos = transform.position.z - halfHeight + rowCellSize * rows;

                points[rows, cols] = new PointMass(new Vector3(xPos, yPos, zPos), 1);
                fixedPoints[rows, cols] = new PointMass(new Vector3(xPos, yPos, zPos), 0);
            }
        }

        //Setting up springs
        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numCols; col++)
            {
                if (col == 0 || row == 0 || col == numCols - 1 || row == numRows - 1)
                {

                    springs.Add(new Spring(fixedPoints[row, col], points[row, col], 0.1F, 0.1F));
                }
                else if (col % 10 == 0 && row % 10 == 0)
                {
                    springs.Add(new Spring(fixedPoints[row, col], points[row, col], 0.2f, 0.2f));
                }

                if (col > 0)
                {
                    springs.Add(new Spring(points[row, col - 1], points[row, col], 0.28f, 0.06f));
                }

                if (row > 0)
                {
                    springs.Add(new Spring(points[row - 1, col], points[row, col], 0.28f, 0.06f));
                }
            }
        }


        thread = new Thread(UpdateGrid);
        thread.Start();
        resetEvent = new AutoResetEvent(false);

    }

    void FixedUpdate()
    {
        resetEvent.Set();
    }

    void UpdateGrid()
    {

        while (!threadStopped)
        {

            resetEvent.WaitOne();

            foreach (Spring s in springs)
            {
                s.Update();
            }

            for (int row = 0; row < numRows; row++)
            {
                for (int col = 0; col < numCols; col++)
                {
                    points[row, col].Update();

                }
            }
        }
    }

    public void ApplyDirectedForce(Vector3 force, Vector3 position, float radius)
    {
        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numCols; col++)
            {
                float distance = Vector3.Distance(points[row, col].position, position);
                if (distance < radius)
                {
                    points[row, col].ApplyForce(force / distance);
                }
            }
        }
    }

    public void ApplyImplosiveForce(float force, Vector3 position, float radius)
    {

        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numCols; col++)
            {
                float distance = Vector3.Distance(points[row, col].position, position);
                if (distance < radius)
                {
                    points[row, col].ApplyForce(10*force*(position - points[row, col].position)/(100 + Mathf.Pow(distance, 0.1F)));
                    points[row, col].IncreaseDamping(0.6F);
                }
            }
        }
    }

    public void ApplyExplosiveForce(float force, Vector3 position, float radius)
    {

        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numCols; col++)
            {
                float distance = Vector3.Distance(points[row, col].position, position);
                if (distance < radius)
                {
                    points[row, col].ApplyForce(100*force*(points[row, col].position - position)/(10000 + distance));
                    points[row, col].IncreaseDamping(0.6F);
                }
            }
        }
    }

    public void Update()
    {

        //ApplyDirectedForce(Vector3.down * force1, player.position, range1);

        if (Input.GetMouseButton(1))
        {
            float mag;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, transform.position.y + 1);
            plane.Raycast(ray, out mag);

            Vector3 point = ray.origin + ray.direction * mag;

            ApplyImplosiveForce(force1, point, range1);
        }

        if (Input.GetMouseButton(0))
        {
            ApplyDirectedForce(Vector3.down * force1, player.position, range1);
        }
    }

    public void OnApplicationQuit()
    {
        threadStopped = true;
        thread.Abort();
    }

    public void OnDestroy()
    {
        threadStopped = true;
        thread.Abort();
    }

    public PointMass[,] GetPoints()
    {
        return points;
    }

    public PointMass[,] GetFixedPoints()
    {
        return fixedPoints;
    }

}
