Shader "Beautiful Dissolves/Mobile/Dissolve (Specular Setup)" {
Properties {
	_Shininess ("Shininess", Range (0.03, 1)) = 0.078125
    _MainTex ("Base (RGB)", 2D) = "white" {}
    [NoScaleOffset] _BumpMap ("Normal Map", 2D) = "bump" {}

  	// Dissolve Settings
	// General
	_DissolveMap ("Dissolve Map", 2D) = "white" {}
	_TilingX("X", Float) = 1.0
	_TilingY("Y", Float) = 1.0
	_DirectionMap ("Direction Map", 2D) = "white" {}
	_DissolveMask ("Dissolve Mask", 2D) = "white" {}
	_DissolveAmount ("Dissolve Amount", Range(0, 1.0)) = 0.5
	_DissolveDelay ("Dissolve Delay", Range(0, 1.0)) = 0.2
	_DissolveRampUp ("Dissolve Ramp Up", Range(1.0, 10.0)) = 5.0
	_SubTex ("Substitute Texture", 2D) = "black" {}

	// Edge color
	_OuterEdgeColor ("Outer Edge Color", Color) = (1,0,0,1)
	_InnerEdgeColor ("Inner Edge Color", Color) = (1,1,0,1)
	_EdgeThickness ("Edge Thickness", Range(0.0, 1.0)) = 0.1
	[Toggle(_EDGECOLORRAMP_USE)] _UseEdgeColorRamp("Use Edge Color Ramp", Int) = 0
	_EdgeColorRamp ("Edge Color Ramp", 2D) = "white" {}

	// Glow
	[Toggle(_DISSOLVEGLOW_ON)] _DissolveGlow ("Dissolve Glow", Int) = 1
	_DissolveGlowColor ("Glow Color", Color) = (1,0.5,0,1)
	_DissolveGlowIntensity ("Glow Intensity", Range(0.0, 1.0)) = 0.5
	[Toggle(_DISSOLVEGLOWFOLLOW_ON)] _DissolveGlowFollow ("Follow-Through", Int) = 0
}
SubShader {
	Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
    LOD 250
    ZWrite Off
	Blend SrcAlpha OneMinusSrcAlpha
		
CGPROGRAM
#pragma target 3.0
#pragma surface surf MobileBlinnPhong exclude_path:prepass nolightmap noforwardadd halfasview interpolateview keepalpha

// --------- DISSOLVE SETTINGS ---------
#pragma multi_compile __ _SUBMAP
#pragma multi_compile __ _EDGECOLORRAMP_USE
#pragma multi_compile __ _DISSOLVEGLOW_ON
#pragma multi_compile __ _DISSOLVEGLOWFOLLOW_ON

// -------------------------------------

#include "../Standard/DissolveCoreFunctions.cginc"

inline fixed4 LightingMobileBlinnPhong (SurfaceOutput s, fixed3 lightDir, fixed3 halfDir, fixed atten)
{
    fixed diff = max (0, dot (s.Normal, lightDir));
    fixed nh = max (0, dot (s.Normal, halfDir));
    fixed spec = pow (nh, s.Specular*128);

    fixed4 c = fixed4(0,0,0,0);
    c.rgb = (s.Albedo * _LightColor0.rgb * diff + _LightColor0.rgb * spec) * atten;
    c.a = s.Alpha;

    return c;
}

sampler2D _MainTex;
sampler2D _BumpMap;
half _Shininess;

struct Input {
    float2 uv_MainTex;
};

void surf (Input IN, inout SurfaceOutput o) {
    fixed4 c = tex2D(_MainTex, IN.uv_MainTex);

	half dissolveMask = DissolveMask(IN.uv_MainTex);

	// an amount <= 0 will be dissolved or replaced with the substitute texture
	half dissolve = DissolveMapFactor(IN.uv_MainTex) * dissolveMask;

	// the x uv coordinate to use in the edge color
	half dissolveUV = DissolveUV(dissolve);

	half dissolveRamp = min(1, _DissolveAmount * _DissolveRampUp);

	half3 dissolveEdgeColor = DissolveEdgeColor(dissolveUV);

	half3 dissolveEmission = DissolveEmission(IN.uv_MainTex);

#if defined(_SUBMAP)
	dissolveEdgeColor = lerp(dissolveEdgeColor, tex2D(_SubTex, IN.uv_MainTex).rgb, dissolve <= 0);
#else
	c.a *= (dissolve > 0);
#endif

	c.rgb = lerp(c.rgb, dissolveEdgeColor, dissolveRamp * dissolveMask * (1.0 - dissolveUV));

    o.Albedo = c.rgb;
    o.Alpha = c.a;
	o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));
	o.Emission = dissolveEmission;
    o.Specular = _Shininess;
}
ENDCG
}

Fallback "Mobile/Transparent/VertexLit"
CustomEditor "MobileDissolveShaderGUI"
}
