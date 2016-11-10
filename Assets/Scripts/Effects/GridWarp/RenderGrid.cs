using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderGrid : MonoBehaviour
{

    public Material gridMaterial;

    private int numRows;
    private int numCols;


    public void Start()
    {
        numRows = Grid.Instance.numRows;
        numCols = Grid.Instance.numCols;
    }

    public void OnPostRender()
    {

        PointMass[,] points = Grid.Instance.GetPoints();
        PointMass[,] fixedPoints = Grid.Instance.GetFixedPoints();

        gridMaterial.SetPass(0);

        GL.LoadIdentity();
        GL.LoadProjectionMatrix(Camera.main.projectionMatrix);
        GL.MultMatrix(Camera.main.worldToCameraMatrix);

        GL.Begin(GL.LINES);


        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numCols; col++)
            {

                if (col > 0)
                {
                    Vector3 pos1 = points[row, col - 1].position;
                    Vector3 pos2 = points[row, col].position;

                    if (row % 5 == 0 || numRows - 1 == row)
                        GL.Color(new Color(0, 0.1F, 0.8F, 0.4F));
                    else
                        GL.Color(new Color(0, 0.1F, 0.8F, 0.2F));

                    GL.Vertex(pos1);
                    GL.Vertex(pos2);

                    //Debug.DrawLine(pos1, pos2, new Color(0, 1, 0.8F, 0.5F));

                    if (row < numRows - 1)
                    {

                        GL.Color(new Color(0, 0.1F, 0.8F, 0.2F));

                        Vector3 pos3 = (pos1 + pos2) / 2;
                        Vector3 pos4 = (points[row + 1, col - 1].position + points[row + 1, col].position) / 2;

                        GL.Vertex(pos3);
                        GL.Vertex(pos4);

                        //Debug.DrawLine(pos3, pos4, new Color(0,1,0.5F, 0.5F));
                    }

                }

                if (row > 0)
                {
                    Vector3 pos1 = points[row - 1, col].position;
                    Vector3 pos2 = points[row, col].position;

                    if (col % 5 == 0 || numCols - 1 == col)
                        GL.Color(new Color(0, 0.1F, 0.8F, 0.4F));
                    else
                        GL.Color(new Color(0, 0.1F, 0.8F, 0.2F));

                    GL.Vertex(pos1);
                    GL.Vertex(pos2);


                    if (col < numCols - 1)
                    {
                        GL.Color(new Color(0, 0.1F, 0.8F, 0.2F));
                        Vector3 pos3 = (pos1 + pos2) / 2;
                        Vector3 pos4 = (points[row - 1, col + 1].position + points[row, col + 1].position) / 2;

                        GL.Vertex(pos3);
                        GL.Vertex(pos4);

                    }
                }
            }
        }

        GL.End();

    }
}
