Shader "Beautiful Dissolves/Sprites/Dissolve"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0

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

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
        CGPROGRAM
            #pragma vertex SpriteVert
            #pragma fragment DissolveSpriteFrag
            #pragma target 3.0
            #pragma multi_compile_instancing
            #pragma multi_compile _ PIXELSNAP_ON
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA

            // --------- DISSOLVE SETTINGS ---------
			#pragma multi_compile __ _SUBMAP
			#pragma multi_compile __ _EDGECOLORRAMP_USE
			#pragma multi_compile __ _DISSOLVEGLOW_ON
			#pragma multi_compile __ _DISSOLVEGLOWFOLLOW_ON

            // -------------------------------------

            #include "DissolveSprites.cginc"
        ENDCG
        }
    }

   	CustomEditor "SpriteDissolveShaderGUI"
}
