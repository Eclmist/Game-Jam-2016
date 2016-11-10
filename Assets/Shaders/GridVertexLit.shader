Shader "Shader Porn/GridVertexLit"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
		
		
	SubShader
	{ 
		Pass
		{

			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off 
			Cull Off Fog { Mode Off }
			BindChannels { Bind "vertex", vertex Bind "color", color }
	
		} 
	
	}
}
