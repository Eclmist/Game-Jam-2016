using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods {

	
    public static Vector3 GetCatmullRomPosition(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        //The coefficients of the cubic polynomial (except the 0.5f * which I added later for performance)
        Vector3 a = 2f * p1;
        Vector3 b = p2 - p0;
        Vector3 c = 2f * p0 - 5f * p1 + 4f * p2 - p3;
        Vector3 d = -p0 + 3f * p1 - 3f * p2 + p3;

        //The cubic polynomial: a + b * t + c * t^2 + d * t^3
        Vector3 pos = 0.5f * (a + (b * t) + (c * t * t) + (d * t * t * t));

        return pos;
    }

    public static GameObject MakeLine(Vector3 startPoint, Vector3 endPoint, float width, Material material)
    {
        Vector3 dirVector = startPoint - endPoint;
        Vector3 sideVector = Quaternion.Euler(0, 90, 0) * dirVector;
        //Define vectors
        Vector3 sv1 = startPoint + width * sideVector;
        Vector3 sv2 = startPoint + width * -sideVector;
        Vector3 ev1 = endPoint + width * sideVector;
        Vector3 ev2 = endPoint + width * -sideVector;

        //Get Vertices 
        int vC = 2;
        Vector3[] vert = new Vector3[4];

        vert[0] = sv1;
        vert[2] = sv2;
        vert[1] = ev1;
        vert[3] = ev2;

        //Make Triangles
        int lS = vC - 1; //Line Segments
        int[] tris = new int[lS * 3 * 2]; //Line Segments * 3 points per triangles * 2 triangles per line segment

        for (int i = 0; i < lS; i++)
        {
            int oV1 = i;            //Outer Value 1
            int oV2 = i + 1;        //Outer Value 2
            int iV1 = vC + i;       //Inner Value 1
            int iV2 = vC + i + 1;   //Inner Value 2

            int tI = i * 6;         //Triangle Index Value

            tris[tI] = oV1;
            tris[tI + 1] = oV2;
            tris[tI + 2] = iV1;

            tris[tI + 3] = iV2;
            tris[tI + 4] = iV1;
            tris[tI + 5] = oV2;
        }

        //Make Normals
        Vector3[] normals = new Vector3[vert.Length];

        for (int i = 0; i < normals.Length; i++)
        {
            var vector3 = normals[i];
            vector3 = Vector3.up;
        }

        //Set UVs
        Vector2[] uvs = new Vector2[vert.Length];

        for (int i = 0; i < vC; i++)
        {
            uvs[i] = new Vector2(i, 1);
            uvs[i + vC] = new Vector2(i, 0);
        }

        //Creating mesh object
        GameObject lineGO = new GameObject("ArcLine");
        lineGO.AddComponent<MeshRenderer>().material = material;
        var mf = lineGO.AddComponent<MeshFilter>();
        var mesh = new Mesh();
        mf.mesh = mesh;
        mesh.vertices = vert;
        mesh.triangles = tris;
        mesh.normals = normals;
        mesh.uv = uvs;
        return lineGO;
    }

}
