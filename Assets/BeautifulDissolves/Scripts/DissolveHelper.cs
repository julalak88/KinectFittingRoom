using System;
using UnityEngine;
using System.Collections;

namespace BeautifulDissolves {
	public static class DissolveHelper {

		public static int dissolveMapID = Shader.PropertyToID("_DissolveMap");
		public static int tilingXID = Shader.PropertyToID ("_TilingX");
		public static int tilingYID = Shader.PropertyToID("_TilingY");
		public static int directionMapID = Shader.PropertyToID ("_DirectionMap");
		public static int dissolveMaskID = Shader.PropertyToID ("_DissolveMask");
		public static int dissolveAmountID = Shader.PropertyToID ("_DissolveAmount");
		public static int dissolveDelayID = Shader.PropertyToID("_DissolveDelay");
		public static int dissolveRampUpID = Shader.PropertyToID ("_DissolveRampUp");
		public static int subTexID = Shader.PropertyToID("_SubTex");
		public static int outerEdgeColorID = Shader.PropertyToID ("_OuterEdgeColor");
		public static int innerEdgeColorID = Shader.PropertyToID("_InnerEdgeColor");
		public static int edgeThicknessID = Shader.PropertyToID ("_EdgeThickness");
		public static int edgeColorRampUseID = Shader.PropertyToID ("_UseEdgeColorRamp");
		public static int edgeColorRampID = Shader.PropertyToID ("_EdgeColorRamp");
		public static int dissolveGlowID = Shader.PropertyToID("_DissolveGlow");
		public static int dissolveGlowColorID = Shader.PropertyToID("_DissolveGlowColor");
		public static int dissolveGlowIntensityID = Shader.PropertyToID ("_DissolveGlowIntensity");
		public static int dissolveGlowFollowID = Shader.PropertyToID("_DissolveGlowFollow");

		public static int emissionColorID = Shader.PropertyToID("_EmissionColor");

		public static string subTexKeyword = "_SUBMAP";
		public static string edgeColorRampUseKeyword = "_EDGECOLORRAMP_USE";
		public static string dissolveGlowKeyword = "_DISSOLVEGLOW_ON";
		public static string dissolveGlowFollowKeyword = "_DISSOLVEGLOWFOLLOW_ON";

		public static void SetDissolveMap(Material mat, Texture2D texture)
		{
			SetTexture (dissolveMapID, mat, texture);
		}

		public static Texture GetDissolveMap(Material mat)
		{
			return GetTexture (dissolveMapID, mat);
		}

		public static void SetDissolveMapTiling(Material mat, Vector2 tiling)
		{
			SetFloat (tilingXID, mat, tiling.x);
			SetFloat (tilingYID, mat, tiling.y);
		}

		public static Vector2 GetDissolveMapTiling(Material mat)
		{
			return new Vector2 (GetFloat (tilingXID, mat), GetFloat (tilingYID, mat));
		}

		public static void SetDirectionMap(Material mat, Texture2D texture)
		{
			SetTexture (directionMapID, mat, texture);
		}

		public static Texture GetDirectionMap(Material mat)
		{
			return GetTexture (directionMapID, mat);
		}

		public static void SetDissolveMask(Material mat, Texture2D texture)
		{
			SetTexture (dissolveMaskID, mat, texture);
		}

		public static Texture GetDissolveMask(Material mat)
		{
			return GetTexture (dissolveMaskID, mat);
		}
			
		public static void SetDissolveAmount(Material mat, float value)
		{
			SetFloat (dissolveAmountID, mat, value);
		}

		public static float GetDissolveAmount(Material mat)
		{
			return GetFloat (dissolveAmountID, mat);
		}

		public static void SetDissolveDelay(Material mat, float value)
		{
			SetFloat (dissolveDelayID, mat, value);
		}

		public static float GetDissolveDelay(Material mat)
		{
			return GetFloat (dissolveDelayID, mat);
		}

		public static void SetDissolveRampUp(Material mat, float value)
		{
			SetFloat (dissolveRampUpID, mat, value);
		}

		public static float GetDissolveRampUp(Material mat)
		{
			return GetFloat (dissolveRampUpID, mat);
		}

		public static void SetSubTex(Material mat, Texture2D texture)
		{
			SetTexture (subTexID, mat, texture);

			EnableKeyword (subTexKeyword, mat, texture != null);
		}

		public static Texture GetSubTex(Material mat)
		{
			return GetTexture (subTexID, mat);
		}

		public static void SetOuterEdgeColor(Material mat, Color color)
		{
			SetColor (outerEdgeColorID, mat, color);
		}

		public static Color GetOuterEdgeColor(Material mat)
		{
			return GetColor (outerEdgeColorID, mat);
		}

		public static void SetInnerEdgeColor(Material mat, Color color)
		{
			SetColor (innerEdgeColorID, mat, color);
		}

		public static Color GetInnerEdgeColor(Material mat)
		{
			return GetColor (innerEdgeColorID, mat);
		}

		public static void SetEdgeThickness(Material mat, float value)
		{
			SetFloat (edgeThicknessID, mat, value);
		}

		public static float GetEdgeThickness(Material mat)
		{
			return GetFloat (edgeThicknessID, mat);
		}

		public static void SetEdgeColorRampUse(Material mat, bool value)
		{
			SetBool (edgeColorRampUseID, mat, value);

			EnableKeyword (edgeColorRampUseKeyword, mat, value);
		}

		public static bool GetEdgeColorRampUse(Material mat)
		{
			return GetBool (edgeColorRampUseID, mat);
		}

		public static void SetEdgeColorRamp(Material mat, Texture2D texture)
		{
			SetTexture (edgeColorRampID, mat, texture);
		}

		public static Texture GetEdgeColorRamp(Material mat)
		{
			return GetTexture (edgeColorRampID, mat);
		}

		public static void SetDissolveGlow(Material mat, bool value)
		{
			SetBool (dissolveGlowID, mat, value);

			EnableKeyword (dissolveGlowKeyword, mat, value);
		}

		public static bool GetDissolveGlow(Material mat)
		{
			return GetBool (dissolveGlowID, mat);
		}

		public static void SetDissolveGlowColor(Material mat, Color color)
		{
			SetColor (dissolveGlowColorID, mat, color);
		}

		public static Color GetDissolveGlowColor(Material mat)
		{
			return GetColor (dissolveGlowColorID, mat);
		}

		public static void SetDissolveGlowIntensity(Material mat, float value)
		{
			SetFloat (dissolveGlowIntensityID, mat, value);
		}

		public static float GetDissolveGlowIntensity(Material mat)
		{
			return GetFloat (dissolveGlowIntensityID, mat);
		}

		public static void SetDissolveGlowFollow(Material mat, bool value)
		{
			SetBool (dissolveGlowFollowID, mat, value);

			EnableKeyword (dissolveGlowFollowKeyword, mat, value);
		}

		public static bool GetDissolveGlowFollow(Material mat)
		{
			return GetBool (dissolveGlowFollowID, mat);
		}

		private static void SetFloat(int propertyID, Material mat, float value)
		{
			if (mat.HasProperty (propertyID)) {
				mat.SetFloat (propertyID, value);
			}
		}

		private static float GetFloat(int propertyID, Material mat)
		{
			return mat.HasProperty (propertyID) ? mat.GetFloat (propertyID) : 0f;
		}

		private static void SetTexture(int propertyID, Material mat, Texture2D texture)
		{
			if (mat.HasProperty (propertyID)) {
				mat.SetTexture (propertyID, texture);
			}
		}

		private static Texture GetTexture(int propertyID, Material mat)
		{
			return mat.HasProperty (propertyID) ? mat.GetTexture (propertyID) : null;
		}

		private static void SetColor(int propertyID, Material mat, Color color)
		{
			if (mat.HasProperty (propertyID)) {
				mat.SetColor (propertyID, color);
			}
		}

		private static Color GetColor(int propertyID, Material mat)
		{
			return mat.HasProperty (propertyID) ? mat.GetColor (propertyID) : Color.white;
		}

		private static void SetBool(int propertyID, Material mat, bool value)
		{
			if (mat.HasProperty (propertyID)) {
				mat.SetInt (propertyID, value ? 1 : 0);
			}
		}

		private static bool GetBool(int propertyID, Material mat)
		{
			return mat.HasProperty (propertyID) ? mat.GetInt (propertyID) == 1 : false;
		}

		private static void EnableKeyword(string keyword, Material mat, bool condition)
		{
			if (condition) {
				mat.EnableKeyword (keyword);
			} else {
				mat.DisableKeyword (keyword);
			}
		}

		public static void LinearDissolve(Material mat, float from, float to, float time, Action callback)
		{
			if (mat.HasProperty(dissolveAmountID)) {
				float elapsedTime = 0f;
				
				while (elapsedTime < time) {
					mat.SetFloat(dissolveAmountID, Mathf.Lerp(from, to, elapsedTime/time));
					elapsedTime += Time.deltaTime;
				}
				mat.SetFloat (dissolveAmountID, to);

				callback ();
			}
		}

		public static void LinearDissolve(Material[] mats, float from, float to, float time, Action callback)
		{
			float elapsedTime = 0f;
			
			while (elapsedTime < time) {
				for (int i = 0; i < mats.Length; i++) {
					if (mats[i].HasProperty(dissolveAmountID)) {
						mats[i].SetFloat(dissolveAmountID, Mathf.Lerp(from, to, elapsedTime/time));
					}
				}
				
				elapsedTime += Time.deltaTime;
			}
			
			for (int i = 0; i < mats.Length; i++) {
				if (mats[i].HasProperty(dissolveAmountID)) {
					mats[i].SetFloat(dissolveAmountID, to);
				}
			}

			callback ();
		}

		public static IEnumerator CurveDissolve(Material[] mats, AnimationCurve dissolveCurve, float time, float curveStartPercentage, float speed, Action callback)
		{
			float elapsedTime = curveStartPercentage;

			while (elapsedTime <= 1f && elapsedTime >= 0f) {
				for (int i = 0; i < mats.Length; i++) {
					SetDissolveAmount (mats[i], Mathf.Clamp01 (dissolveCurve.Evaluate (elapsedTime)));
				}
				elapsedTime += Time.deltaTime/time * speed;
				yield return null;
			}

			for (int i = 0; i < mats.Length; i++) {
				SetDissolveAmount (mats[i], Mathf.Clamp01 (dissolveCurve.Evaluate (elapsedTime)));
			}
				
			callback ();
		}

		public static bool IsInLayerMask(this GameObject obj, LayerMask mask) {
			return ((mask.value & (1 << obj.layer)) > 0);
		}
	}
}