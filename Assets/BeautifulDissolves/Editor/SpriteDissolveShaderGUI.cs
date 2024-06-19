using System;
using UnityEngine;

namespace UnityEditor
{
	internal class SpriteDissolveShaderGUI : ShaderGUI
	{
		private static class Styles
		{
			public static string emptyTootip = "";
			public static GUIContent spriteText = new GUIContent("Sprite Texture", "");
			public static GUIContent spriteColorText = new GUIContent("Tint", "");
			public static GUIContent pixelSnapText = new GUIContent("Pixel Snap", "");

			// Dissolve GUI labels
			public static GUIContent dissolveMapText = new GUIContent("Dissolve Map", "Dissolve Map (R)");
			public static GUIContent tilingText = new GUIContent("Tiling", "");
			public static GUIContent directionMapText = new GUIContent("Direction Map", "Direction of Dissolve (R)");
			public static GUIContent dissolveMaskText = new GUIContent("Dissolve Mask", "Mask for dissolving (1=dissolve, 0=no dissolve) (A)");
			public static GUIContent dissolveAmountText = new GUIContent("Amount", "Dissolve Amount");
			public static GUIContent dissolveDelayText = new GUIContent("Delay", "The dissolve amount required before beginning to dissolve");
			public static GUIContent dissolveRampUpText = new GUIContent("Ramp Up", "The ramp up speed before dissolve colors fade in");
			public static GUIContent substituteText = new GUIContent("Substitute Texture", "Substitute Texture (RGB)");
			public static GUIContent blendColorsText = new GUIContent("Edge Color Blending", "");
			public static GUIContent outerEdgeColorText = new GUIContent("Outer Edge Color", "");
			public static GUIContent innerEdgeColorText = new GUIContent("Inner Edge Color", "");
			public static GUIContent useEdgeColorRampText = new GUIContent("Use Color Ramp", "");
			public static GUIContent edgeColorRampText = new GUIContent("Color Ramp", "Edge Color Ramp (RGB)");
			public static GUIContent edgeThicknessText = new GUIContent("Edge Thickness", "");
			public static GUIContent dissolveGlowText = new GUIContent("Dissolve Glow", "");
			public static GUIContent dissolveGlowColorText = new GUIContent("Glow Color", "");
			public static GUIContent dissolveGlowIntensityText = new GUIContent("Glow Intensity", "");
			public static GUIContent dissolveGlowFollowText = new GUIContent("Follow-Through", "");
			public static string dissolveSettings = "Dissolve Settings";
		}

		MaterialProperty spriteMap = null;
		MaterialProperty spriteColor = null;
		MaterialProperty pixelSnap = null;

		// Dissolve properties
		MaterialProperty dissolveMap = null;
		MaterialProperty tilingX = null;
		MaterialProperty tilingY = null;
		MaterialProperty directionMap = null;
		MaterialProperty dissolveMask = null;
		MaterialProperty dissolveAmount = null;
		MaterialProperty dissolveDelay = null;
		MaterialProperty dissolveRampUp = null;
		MaterialProperty substituteMap = null;
		MaterialProperty outerEdgeColor = null;
		MaterialProperty innerEdgeColor = null;
		MaterialProperty useEdgeColorRamp = null;
		MaterialProperty edgeColorRamp = null;
		MaterialProperty edgeThickness = null;
		MaterialProperty dissolveGlow = null;
		MaterialProperty dissolveGlowColor = null;
		MaterialProperty dissolveGlowIntensity = null;
		MaterialProperty dissolveGlowFollow = null;

		MaterialEditor m_MaterialEditor;

		bool m_FirstTimeApply = true;
		Vector2 m_Tiling = new Vector2(1f, 1f);

		public void FindProperties (MaterialProperty[] props)
		{
			spriteMap = FindProperty ("_MainTex", props);
			spriteColor = FindProperty ("_Color", props);
			pixelSnap = FindProperty ("PixelSnap", props);

			dissolveMap = FindProperty ("_DissolveMap", props);
			tilingX = FindProperty("_TilingX", props);
			tilingY = FindProperty("_TilingY", props);
			directionMap = FindProperty ("_DirectionMap", props);
			dissolveMask = FindProperty ("_DissolveMask", props);
			dissolveAmount = FindProperty ("_DissolveAmount", props);
			dissolveDelay = FindProperty ("_DissolveDelay", props);
			dissolveRampUp = FindProperty ("_DissolveRampUp", props);
			substituteMap = FindProperty ("_SubTex", props);
			outerEdgeColor = FindProperty ("_OuterEdgeColor", props);
			innerEdgeColor = FindProperty ("_InnerEdgeColor", props);
			useEdgeColorRamp = FindProperty ("_UseEdgeColorRamp", props);
			edgeColorRamp = FindProperty("_EdgeColorRamp", props);
			edgeThickness = FindProperty ("_EdgeThickness", props);
			dissolveGlow = FindProperty ("_DissolveGlow", props);
			dissolveGlowColor = FindProperty ("_DissolveGlowColor", props);
			dissolveGlowIntensity = FindProperty ("_DissolveGlowIntensity", props);
			dissolveGlowFollow = FindProperty("_DissolveGlowFollow", props);
		}

		public override void OnGUI (MaterialEditor materialEditor, MaterialProperty[] props)
		{
			FindProperties (props); // MaterialProperties can be animated so we do not cache them but fetch them every event to ensure animated values are updated correctly
			m_MaterialEditor = materialEditor;
			Material material = materialEditor.target as Material;

			ShaderPropertiesGUI (material);

			// Make sure that needed keywords are set up if we're switching some existing
			// material to a standard shader.
			if (m_FirstTimeApply)
			{
				SetMaterialKeywords (material);
				m_FirstTimeApply = false;
			}
		}

		public void ShaderPropertiesGUI (Material material)
		{
			// Use default labelWidth
			EditorGUIUtility.labelWidth = 0f;

			// Detect any changes to the material
			EditorGUI.BeginChangeCheck();
			{
				// Primary properties
				DoAlbedoArea();
			}

			if (EditorGUI.EndChangeCheck())
			{
				SetMaterialKeywords(material);
			}

			m_MaterialEditor.RenderQueueField();
			m_MaterialEditor.EnableInstancingField();
			m_MaterialEditor.DoubleSidedGIField();

			// Detect any changes to the material
			EditorGUI.BeginChangeCheck();
			{
				// Dissolve settings
				GUILayout.Label(Styles.dissolveSettings, EditorStyles.boldLabel);
				DoDissolveArea(material);
			}

			if (EditorGUI.EndChangeCheck())
			{
				SetMaterialKeywords(material);
			}
		}

		public override void AssignNewShaderToMaterial (Material material, Shader oldShader, Shader newShader)
		{
			base.AssignNewShaderToMaterial(material, oldShader, newShader);

			if (oldShader == null || !oldShader.name.Contains("Legacy Shaders/"))
				return;
			SetMaterialKeywords(material);
		}

		// Dissolve settings area
		public virtual void DoDissolveArea(Material material)
		{
			m_MaterialEditor.TexturePropertySingleLine(Styles.dissolveMapText, dissolveMap);

			EditorGUI.indentLevel += (MaterialEditor.kMiniTextureFieldLabelIndentLevel + 1);
			m_Tiling.x = tilingX.floatValue;
			m_Tiling.y = tilingY.floatValue;
			EditorGUI.BeginChangeCheck();
			m_Tiling = EditorGUILayout.Vector2Field(Styles.tilingText, m_Tiling);
			if (EditorGUI.EndChangeCheck()) {
				tilingX.floatValue = m_Tiling.x;
				tilingY.floatValue = m_Tiling.y;

			}
			EditorGUI.indentLevel -= (MaterialEditor.kMiniTextureFieldLabelIndentLevel + 1);

			m_MaterialEditor.ShaderProperty(dissolveAmount, Styles.dissolveAmountText.text, MaterialEditor.kMiniTextureFieldLabelIndentLevel+1);
			m_MaterialEditor.ShaderProperty(dissolveDelay, Styles.dissolveDelayText.text, MaterialEditor.kMiniTextureFieldLabelIndentLevel+1);
			m_MaterialEditor.ShaderProperty(dissolveRampUp, Styles.dissolveRampUpText.text, MaterialEditor.kMiniTextureFieldLabelIndentLevel+1);

			m_MaterialEditor.TexturePropertySingleLine(Styles.directionMapText, directionMap);
			m_MaterialEditor.TexturePropertySingleLine(Styles.dissolveMaskText, dissolveMask);

			m_MaterialEditor.TexturePropertySingleLine(Styles.substituteText, substituteMap);

			EditorGUILayout.Space();

			m_MaterialEditor.ShaderProperty(useEdgeColorRamp, Styles.useEdgeColorRampText.text);
			if (material.IsKeywordEnabled("_EDGECOLORRAMP_USE")) {
				m_MaterialEditor.TexturePropertySingleLine(Styles.edgeColorRampText, edgeColorRamp);
			} else {
				m_MaterialEditor.ColorProperty(innerEdgeColor, Styles.innerEdgeColorText.text);
				m_MaterialEditor.ColorProperty(outerEdgeColor, Styles.outerEdgeColorText.text);
			}

			m_MaterialEditor.ShaderProperty(edgeThickness, Styles.edgeThicknessText.text);

			EditorGUILayout.Space();

			m_MaterialEditor.ShaderProperty(dissolveGlow, Styles.dissolveGlowText.text);

			if (material.IsKeywordEnabled("_DISSOLVEGLOW_ON")) {
				m_MaterialEditor.ColorProperty(dissolveGlowColor, Styles.dissolveGlowColorText.text);
				m_MaterialEditor.ShaderProperty(dissolveGlowIntensity, Styles.dissolveGlowIntensityText.text);
				m_MaterialEditor.ShaderProperty(dissolveGlowFollow, Styles.dissolveGlowFollowText.text);
			}

			EditorGUILayout.Space();
		}

		public virtual void DoAlbedoArea()
		{
			m_MaterialEditor.TexturePropertySingleLine(Styles.spriteText, spriteMap, spriteColor);
			m_MaterialEditor.ShaderProperty(pixelSnap, Styles.pixelSnapText.text);
		}

		static void SetMaterialKeywords(Material material)
		{
			// Dissolve keywords
			SetKeyword (material, "_SUBMAP", material.GetTexture ("_SubTex"));
		}

		static void SetKeyword(Material m, string keyword, bool state)
		{
			if (state)
				m.EnableKeyword (keyword);
			else
				m.DisableKeyword (keyword);
		}
	}

} // namespace UnityEditor
