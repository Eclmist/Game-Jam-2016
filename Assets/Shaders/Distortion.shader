Shader "Shader Porn/Distortion"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			half2 _DistortionPoint;
			half _Sine;
			float _Aspect;

			fixed4 frag (v2f i) : SV_Target
			{
				//_DistortionPoint = half2(0,0);
				
				half dist = length(i.uv * float2(_Aspect, 1) - _DistortionPoint  * float2(_Aspect, 1));
				half2 dir = normalize(i.uv - _DistortionPoint);	

				dist *= 100;
				//dist = dist * 21.667;
				
				float distortionMagnitude = 5 * sin(dist*_Sine)*_Sine / dist;

				dir = dir * (distortionMagnitude);
				fixed4 col;

				if (dist < 900)
				{
					col = tex2D(_MainTex, i.uv + dir * 0.05 * (1 - dist / 20));
				}
				else
				{
					col = tex2D(_MainTex, i.uv);
				}


				//col = lerp(0, col, pow(dist, 10));

				return col;
			}
			ENDCG
		}
	}
}
