// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Shader Porn/Surface Distortion"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color("Color", Color) = (0,0,0,0)
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" }
		LOD 100

		Blend One One
		ZTest Always

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
				float4 worldPos : TEXCOORD1;

			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _Color;
					
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				return o;
			}
		
			float4 _DistortionPos;

			fixed4 frag (v2f i) : SV_Target
			{
				_DistortionPos = float4(0,0,0,0);
				
				half4 dir = i.worldPos - _DistortionPos;
				float dist = length(dir);

				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);


				col += _Color * col.a * (4 - dist);
				
				return col;
			}
			ENDCG
		}
	}
}
