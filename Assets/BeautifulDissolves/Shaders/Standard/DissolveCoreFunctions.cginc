#ifndef DISSOLVE_CORE_FUNCTIONS_INCLUDED
#define DISSOLVE_CORE_FUNCTIONS_INCLUDED

half		_DissolveGlowIntensity;
half4		_DissolveGlowColor;
half		_DissolveRampUp;
half 		_DissolveDelay;
sampler2D	_DissolveMap;
sampler2D	_DissolveMask;
sampler2D	_DirectionMap;
half		_DissolveAmount;

sampler2D	_SubTex;

half3		_OuterEdgeColor;
half3		_InnerEdgeColor;
half		_EdgeThickness;

sampler2D	_EdgeColorRamp;

float _TilingX;
float _TilingY;

//-------------------------------------------------------------------------------------
// Core dissolve functions

float2 AdjustedUV(float2 uv)
{
	return float2(uv.x * _TilingX, uv.y * _TilingY);
}

half DirectionMapFactor(half dissolve, float2 uv)
{
	return dissolve * tex2D(_DirectionMap, uv).r;
}

half Remap(half value, half low1, half high1, half low2, half high2) {
	return low2 + (value - low1) * (high2 - low2) / (high1 - low1);
}

half DissolveMapFactor(float2 uv)
{
	float2 adjustedUV = AdjustedUV(uv);
	half dissolve = clamp(DirectionMapFactor(tex2D(_DissolveMap, adjustedUV).r, adjustedUV), 0, 1);
	return 1.0 - ((1.0 - dissolve) + ((_DissolveAmount > _DissolveDelay) * Remap(_DissolveAmount, min(0.99, _DissolveDelay), 1, 0, 1)));
}

half DissolveUV(half dissolve) {
	return clamp(Remap(dissolve, -_EdgeThickness, _EdgeThickness, -(1.0 - _DissolveAmount), 1.0 - _DissolveAmount), 0, 1);
}

half3 DissolveGlow(half dissolve) {
	return clamp(Remap(dissolve, -(_DissolveGlowIntensity), (_DissolveGlowIntensity), -1, 1), 0, 1);
}

half3 DissolveEdgeColor(half dissolveUV)
{
#if defined(_EDGECOLORRAMP_USE)
	return tex2D(_EdgeColorRamp, float2(dissolveUV, dissolveUV)).rgb;
#else
	return lerp(_OuterEdgeColor, _InnerEdgeColor, dissolveUV);
#endif
}

half DissolveMask(float2 uv) {
	return clamp(tex2D(_DissolveMask, uv).a, 0, 1);
}

half3 Dissolve(half3 diffColor, float2 uv, half oneMinusReflectivity)
{
	half dissolveMask = DissolveMask(uv);

	// an amount < 0 will be dissolved or replaced with the substitute texture
	half dissolve = DissolveMapFactor(uv) * dissolveMask;

	// the x uv coordinate to use in the edge color
	half dissolveUV = DissolveUV(dissolve);

	half dissolveRamp = min(1, _DissolveAmount * _DissolveRampUp);

	half3 dissolveEdgeColor = DissolveEdgeColor(dissolveUV);
#if defined(_SUBMAP)
	dissolveEdgeColor = lerp(dissolveEdgeColor, tex2D(_SubTex, uv).rgb * oneMinusReflectivity, dissolve <= 0);
#else
	clip(dissolve);
#endif

	return lerp(diffColor, dissolveEdgeColor, dissolveRamp * dissolveMask * (1.0 - dissolveUV));
}

half3 DissolveEmission(float2 uv)
{
#if defined(_DISSOLVEGLOW_ON)

	half dissolveMask = DissolveMask(uv);

	// an amount < 0 will be dissolved or replaced with the substitute texture
	half dissolve = DissolveMapFactor(uv) * dissolveMask;

	// the x uv coordinate to use in the edge color
	half dissolveUV = DissolveUV(dissolve);

	half3 dissolveEdgeColor = DissolveEdgeColor(dissolveUV);

	half dissolveFollow = dissolve > 0;

	// point at which dissolve glow should be at peak
	half dissolvePeak = ((1.0 - _DissolveDelay) * 0.5) + _DissolveDelay;

	// glow curve, highest point at peak
	half dissolveCurve = lerp(lerp(0, 1, Remap(_DissolveAmount, 0, dissolvePeak, 0, 1)), lerp(1, 0, Remap(_DissolveAmount, min(0.99, dissolvePeak), 1, 0, 1)), _DissolveAmount >= dissolvePeak);

	// speed at which glow reaches its highest glow
	half dissolveRamp = min(1, _DissolveAmount * _DissolveRampUp);

	half dissolveIntensity = (1.0 + _DissolveGlowIntensity);

	// ensures that glow color does not overpower edge color
	half3 glowColor =  lerp(dissolveEdgeColor, _DissolveGlowColor.rgb, dissolveUV > 0.99);

#if defined(_DISSOLVEGLOWFOLLOW_ON)
	dissolveFollow = 1.0;
#endif

	return dissolveCurve * glowColor * dissolveRamp * dissolveIntensity * dissolveFollow * (1.0 - DissolveGlow(dissolve)) * dissolveMask;
#endif

	return half3(0,0,0);
}
			
#endif // DISSOLVE_CORE_FUNCTIONS_INCLUDED
