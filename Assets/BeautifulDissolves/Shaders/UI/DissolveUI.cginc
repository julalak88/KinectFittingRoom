#ifndef DISSOLVE_UI_INCLUDED
#define DISSOLVE_UI_INCLUDED

#include "UnityCG.cginc"
#include "UnityUI.cginc"
#include "../Standard/DissolveCoreFunctions.cginc"

struct appdata_t
{
    float4 vertex   : POSITION;
    float4 color    : COLOR;
    float2 texcoord : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct v2f
{
    float4 vertex   : SV_POSITION;
    fixed4 color    : COLOR;
    float2 texcoord  : TEXCOORD0;
    float4 worldPosition : TEXCOORD1;
    UNITY_VERTEX_OUTPUT_STEREO
};

fixed4 _Color;
fixed4 _TextureSampleAdd;
float4 _ClipRect;

v2f vert(appdata_t v)
{
    v2f OUT;
    UNITY_SETUP_INSTANCE_ID(v);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
    OUT.worldPosition = v.vertex;
    OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

    OUT.texcoord = v.texcoord;

    OUT.color = v.color * _Color;
    return OUT;
}

sampler2D _MainTex;

fixed4 frag(v2f IN) : SV_Target
{
    half4 color = tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd;

	fixed dissolveMask = DissolveMask(IN.texcoord);

	// an amount < 0 will be dissolved or replaced with the substitute texture
	fixed dissolve = DissolveMapFactor(IN.texcoord) * dissolveMask;

	// the x uv coordinate to use in the edge color
	fixed dissolveUV = DissolveUV(dissolve);

	fixed dissolveRamp = min(1, _DissolveAmount * _DissolveRampUp);

	fixed4 dissolveEdgeColor = fixed4(DissolveEdgeColor(dissolveUV), 1);

	fixed dissolveClip = (dissolve >= 0);

    // dissolve glow emission
	fixed4 dissolveEmission = fixed4(DissolveEmission(IN.texcoord), 0);

//#if _SUBMAP
	fixed4 substituteTexture = tex2D(_SubTex, IN.texcoord) + _TextureSampleAdd;
	dissolveEdgeColor = lerp(dissolveEdgeColor, substituteTexture, dissolve <= 0);
	dissolveClip = 1;
//#endif

	// alpha is either maintex or substitute and on condition that it is dissolving
    fixed alpha = lerp(color.a, dissolveEdgeColor.a, dissolve < 0);

    alpha *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);

    #ifdef UNITY_UI_ALPHACLIP
    clip (alpha - 0.001);
    #endif

    color.a = alpha;
    dissolveEdgeColor.a = alpha;
    dissolveEmission *= alpha;

    return (dissolveEmission + lerp(color, dissolveEdgeColor, dissolveRamp * dissolveMask * (1.0 - dissolveUV)) * dissolveClip * IN.color);
}

#endif // DISSOLVE_UI_INCLUDED
