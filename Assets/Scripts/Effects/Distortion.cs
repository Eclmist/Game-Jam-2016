using UnityEngine;

namespace Assets.Scripts.Effects
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Image Effects/Shader Porn/Distortion")]
    public class Distortion : MonoBehaviour
    {
        public Shader distortionPass;
        private Material distortionMat;

        public Transform distortionPoint;
        public float sine;

        // Use this for initialization
        void Start()
        {
            distortionMat = new Material(distortionPass);
            distortionMat.SetFloat("_Aspect", (float)Screen.width/ Screen.height);
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            Vector2 screenCoord = Camera.main.WorldToViewportPoint(distortionPoint.position);
            screenCoord.y = 1 - screenCoord.y;
            distortionMat.SetVector("_DistortionPoint", screenCoord);
            sine -= Time.deltaTime;
            if (sine < 0)
                sine = 1;
            distortionMat.SetFloat("_Sine", sine);
            Graphics.Blit(source, destination, distortionMat);
        }

    }
}
