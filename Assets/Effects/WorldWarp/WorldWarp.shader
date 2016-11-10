Shader "Hidden/WorldWarp"
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
				float4 fragCoord : TEXCOORD1;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				o.fragCoord = ComputeScreenPos(o.vertex);
				return o;
			}
			
			sampler2D _MainTex;
			float _WarpAmount;
			float _Power;
			float _AspectRatio;

			fixed4 frag (v2f i) : SV_Target
			{
				// uv + pow (dist,2) * -dir
				float2 dir = (i.fragCoord - float2(0.5,0.5)) * float2(_AspectRatio, 1);
				float dist = length(dir);

				fixed4 col = tex2D(_MainTex, i.uv + pow(dist, _Power) * _WarpAmount * dir);

				return col;

			}
			ENDCG
		}
	}
}
