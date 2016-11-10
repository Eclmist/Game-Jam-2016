using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent (typeof(Camera))]
[AddComponentMenu("Image Effects/Shader Porn/World Warp")]
public class WorldWarp : MonoBehaviour
{
    [Header("Warp Properties")]
    public float warpAmount;
    public float power;

    [Space(10F)]
    public Shader worldWarpShader;
    private Material worldWarpMat;

    void Start()
    {
        worldWarpMat = new Material(worldWarpShader);
    }

    void OnRenderImage(RenderTexture source, RenderTexture dest)
    {

        worldWarpMat.SetFloat("_WarpAmount", warpAmount);
        worldWarpMat.SetFloat("_Power", power);
        worldWarpMat.SetFloat("_AspectRatio", Screen.width / Screen.height);

        Graphics.Blit(source, dest, worldWarpMat);
    }

}
