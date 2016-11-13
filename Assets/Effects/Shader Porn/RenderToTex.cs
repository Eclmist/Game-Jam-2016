using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderToTex : MonoBehaviour {

    Camera cam;
    RenderTexture target;
    
	// Use this for initialization
	void Start ()
    {
        cam = GetComponent<Camera>();
        target = new RenderTexture(cam.pixelWidth, cam.pixelHeight, 24);

        cam.targetTexture = target;
	}

    public RenderTexture GetTargetTexture()
    {
        return target;
    }

    void OnDestroy()
    {
        cam.targetTexture = null;
    }
}
