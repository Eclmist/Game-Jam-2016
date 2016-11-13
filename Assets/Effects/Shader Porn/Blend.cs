using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blend : MonoBehaviour
{

    public Shader blendPass;
    private Material mat;
    private RenderToTex rendersource;

    void Start()
    {
        mat = new Material(blendPass);
        rendersource = GetComponentInChildren<RenderToTex>();
    }

    void OnRenderImage(RenderTexture source, RenderTexture dest)
    {
        mat.SetTexture("_Other", rendersource.GetTargetTexture());
        Graphics.Blit(source, dest, mat);
    }
}
