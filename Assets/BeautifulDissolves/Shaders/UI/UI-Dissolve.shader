Shader "Beautiful Dissolves/UI/Dissolve"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255

        _ColorMask ("Color Mask", Float) = 15

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0

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

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            Name "Default"
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #pragma multi_compile __ UNITY_UI_ALPHACLIP

            // --------- DISSOLVE SETTINGS ---------
			#pragma multi_compile __ _SUBMAP
			#pragma multi_compile __ _EDGECOLORRAMP_USE
			#pragma multi_compile __ _DISSOLVEGLOW_ON
			#pragma multi_compile __ _DISSOLVEGLOWFOLLOW_ON

            // -------------------------------------

            #include "DissolveUI.cginc"
        ENDCG
        }
    }

    CustomEditor "UIDissolveShaderGUI"
}
