// Shader "Custom/NightFog" 
Shader "Custom/NightFog" { Properties{ _Color("Main Color", Color) = (1,1,1,0.5) _MainTex("Texture", 2D) = "white" { } _AlphaTex("Texture", 2D) = "white" { } } SubShader{

	Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }
	LOD 200
	Blend SrcAlpha OneMinusSrcAlpha
	ZTest Less

	Pass{

	CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

	float4 _Color;
sampler2D _MainTex;
sampler2D _AlphaTex;

struct v2f {
	float4  pos : SV_POSITION;
	float2  uv : TEXCOORD0;
};

float4 _MainTex_ST;

v2f vert(appdata_base v)
{
	v2f o;
	o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
	o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
	return o;
}

half4 frag(v2f i) : COLOR
{
	half4 texcol = tex2D(_MainTex, i.uv);
	texcol.a = tex2D(_AlphaTex, i.uv).a;
	return texcol * _Color;
}
ENDCG

}
}
Fallback "VertexLit"
}