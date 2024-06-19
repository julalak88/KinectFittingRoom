#ifndef DISSOLVE_SPRITES_INCLUDED
#define DISSOLVE_SPRITES_INCLUDED

#include "UnitySprites.cginc"
#include "../Standard/DissolveCoreFunctions.cginc"

fixed4 DissolveSpriteTexture (float2 uv, fixed4 inColor)
{
#if ETC1_EXTERNAL_ALPHA
    fixed4 alpha = tex2D (_AlphaTex, uv);
    inColor.a = lerp (inColor.a, alpha.r, _EnableExternalAlpha);
#endif

    return inColor;
}

fixed4 DissolveSpriteFrag(v2f IN) : SV_Target
{
    fixed4 color = DissolveSpriteTexture (IN.texcoord, tex2D(_MainTex, IN.texcoord));

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

#if _SUBMAP
	fixed4 substituteTexture = DissolveSpriteTexture(IN.texcoord, tex2D(_SubTex, IN.texcoord));
	dissolveEdgeColor = lerp(dissolveEdgeColor, substituteTexture, dissolve <= 0);
	dissolveClip = 1;
#endif

	// alpha is either maintex or substitute and on condition that it is dissolving
    fixed alpha = lerp(color.a, dissolveEdgeColor.a, dissolve <= 0);

    color.rgb *= alpha;
    dissolveEdgeColor.rgb *= alpha;
    dissolveEmission.rgb *= alpha;

    dissolveEdgeColor.a = alpha;

    return (dissolveEmission + lerp(color, dissolveEdgeColor, dissolveRamp * dissolveMask * (1.0 - dissolveUV)) * dissolveClip * IN.color);
}

#endif // DISSOLVE_SPRITES_INCLUDED
