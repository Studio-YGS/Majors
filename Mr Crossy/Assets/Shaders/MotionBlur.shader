// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "MotionBlur"
{
	Properties
	{
		_MainTex("Texture", 2D) = "black" {}
		_BlurMagnitude("BlurMagnitude", Range(0,0.01)) = 1 
		_ScreenXMagnitude("ScreenXMagnitude", Range(0,0.05)) = 1 
		_ScreenYMagnitude("ScreenYMagnitude", Range(0,0.05)) = 1
	}
		Subshader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vertex_shader
			#pragma fragment pixel_shader
			#pragma target 2.0

			sampler2D _MainTex;
			float _BlurMagnitude;
			float _ScreenXMagnitude;
			float _ScreenYMagnitude;

			float4 vertex_shader(float4 vertex:POSITION) :SV_POSITION
			{
				return UnityObjectToClipPos(vertex);
			}

			float4 pixel_shader(float4 vertex:SV_POSITION) : COLOR
			{
				vector <float,2> uv = vertex.xy / _ScreenParams.xy;
				uv.x += cos(uv.y * 2.0 + _Time.g) * _ScreenXMagnitude;
				uv.y += sin(uv.x * 2.0 + _Time.g) * _ScreenYMagnitude;
				float offset = sin(_Time.g * 0.5) * _BlurMagnitude;
				float4 a = tex2D(_MainTex,uv);
				float4 b = tex2D(_MainTex,uv - float2(sin(offset),0.0));
				float4 c = tex2D(_MainTex,uv + float2(sin(offset),0.0));
				float4 d = tex2D(_MainTex,uv - float2(0.0,sin(offset)));
				float4 e = tex2D(_MainTex,uv + float2(0.0,sin(offset)));
				return (a + b + c + d + e) / 5.0;
			}
			ENDCG
		}
	}
}