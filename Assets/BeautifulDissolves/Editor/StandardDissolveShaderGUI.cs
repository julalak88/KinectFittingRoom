using System;
using UnityEngine;

namespace UnityEditor
{
	internal class StandardDissolveShaderGUI : ShaderGUI
	{
		private enum WorkflowMode
		{
			Specular,
			Metallic,
			Dielectric
		}

		public enum BlendMode
		{
			Opaque,
			Cutout,
			Fade,   // Old school alpha-blending mode, fresnel does not affect amount of transparency
			Transparent // Physically plausible transparency mode, implemented as alpha pre-multiply
		}

		public enum SmoothnessMapChannel
		{
			SpecularMetallicAlpha,
			AlbedoAlpha,
		}

		private static class Styles
		{
			public static GUIContent uvSetLabel = new GUIContent("UV Set");

			public static GUIContent albedoText = new GUIContent("Albedo", "Albedo (RGB) and Transparency (A)");
			public static GUIContent alphaCutoffText = new GUIContent("Alpha Cutoff", "Threshold for alpha cutoff");
			public static GUIContent specularMapText = new GUIContent("Specular", "Specular (RGB) and Smoothness (A)");
			public static GUIContent metallicMapText = new GUIContent("Metallic", "Metallic (R) and Smoothness (A)");
			public static GUIContent smoothnessText = new GUIContent("Smoothness", "Smoothness value");
			public static GUIContent smoothnessScaleText = new GUIContent("Smoothness", "Smoothness scale factor");
			public static GUIContent smoothnessMapChannelText = new GUIContent("Source", "Smoothness texture and channel");
			public static GUIContent highlightsText = new GUIContent("Specular Highlights", "Specular Highlights");
			public static GUIContent reflectionsText = new GUIContent("Reflections", "Glossy Reflections");
			public static GUIContent normalMapText = new GUIContent("Normal Map", "Normal Map");
			public static GUIContent heightMapText = new GUIContent("Height Map", "Height Map (G)");
			public static GUIContent occlusionText = new GUIContent("Occlusion", "Occlusion (G)");
			public static GUIContent emissionText = new GUIContent("Color", "Emission (RGB)");
			public static GUIContent detailMaskText = new GUIContent("Detail Mask", "Mask for Secondary Maps (A)");
			public static GUIContent detailAlbedoText = new GUIContent("Detail Albedo x2", "Albedo (RGB) multiplied by 2");
			public static GUIContent detailNormalMapText = new GUIContent("Normal Map", "Normal Map");

			public static string primaryMapsText = "Main Maps";
			public static string secondaryMapsText = "Secondary Maps";
			public static string forwardText = "Forward Rendering Options";
			public static string renderingMode = "Rendering Mode";
			public static string advancedText = "Advanced Options";
			public static GUIContent emissiveWarning = new GUIContent("Emissive value is animated but the material has not been configured to support emissive. Please make sure the material itself has some amount of emissive.");
			public static readonly string[] blendNames = Enum.GetNames(typeof(BlendMode));


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

		MaterialProperty blendMode = null;
		MaterialProperty albedoMap = null;
		MaterialProperty albedoColor = null;
		MaterialProperty alphaCutoff = null;
		MaterialProperty specularMap = null;
		MaterialProperty specularColor = null;
		MaterialProperty metallicMap = null;
		MaterialProperty metallic = null;
		MaterialProperty smoothness = null;
		MaterialProperty smoothnessScale = null;
		MaterialProperty smoothnessMapChannel = null;
		MaterialProperty highlights = null;
		MaterialProperty reflections = null;
		MaterialProperty bumpScale = null;
		MaterialProperty bumpMap = null;
		MaterialProperty occlusionStrength = null;
		MaterialProperty occlusionMap = null;
		MaterialProperty heigtMapScale = null;
		MaterialProperty heightMap = null;
		MaterialProperty emissionColorForRendering = null;
		MaterialProperty emissionMap = null;
		MaterialProperty detailMask = null;
		MaterialProperty detailAlbedoMap = null;
		MaterialProperty detailNormalMapScale = null;
		MaterialProperty detailNormalMap = null;
		MaterialProperty uvSetSecondary = null;

		MaterialEditor m_MaterialEditor;
		WorkflowMode m_WorkflowMode = WorkflowMode.Specular;
		ColorPickerHDRConfig m_ColorPickerHDRConfig = new ColorPickerHDRConfig(0f, 99f, 1 / 99f, 3f);

		bool m_FirstTimeApply = true;

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

		Vector2 m_Tiling = new Vector2(1f, 1f);

		public void FindProperties(MaterialProperty[] props)
		{
			blendMode = FindProperty("_Mode", props);
			albedoMap = FindProperty("_MainTex", props);
			albedoColor = FindProperty("_Color", props);
			alphaCutoff = FindProperty("_Cutoff", props);
			specularMap = FindProperty("_SpecGlossMap", props, false);
			specularColor = FindProperty("_SpecColor", props, false);
			metallicMap = FindProperty("_MetallicGlossMap", props, false);
			metallic = FindProperty("_Metallic", props, false);
			if (specularMap != null && specularColor != null)
				m_WorkflowMode = WorkflowMode.Specular;
			else if (metallicMap != null && metallic != null)
				m_WorkflowMode = WorkflowMode.Metallic;
			else
				m_WorkflowMode = WorkflowMode.Dielectric;
			smoothness = FindProperty("_Glossiness", props);
			smoothnessScale = FindProperty("_GlossMapScale", props, false);
			smoothnessMapChannel = FindProperty("_SmoothnessTextureChannel", props, false);
			highlights = FindProperty("_SpecularHighlights", props, false);
			reflections = FindProperty("_GlossyReflections", props, false);
			bumpScale = FindProperty("_BumpScale", props);
			bumpMap = FindProperty("_BumpMap", props);
			heigtMapScale = FindProperty("_Parallax", props);
			heightMap = FindProperty("_ParallaxMap", props);
			occlusionStrength = FindProperty("_OcclusionStrength", props);
			occlusionMap = FindProperty("_OcclusionMap", props);
			emissionColorForRendering = FindProperty("_EmissionColor", props);
			emissionMap = FindProperty("_EmissionMap", props);
			detailMask = FindProperty("_DetailMask", props);
			detailAlbedoMap = FindProperty("_DetailAlbedoMap", props);
			detailNormalMapScale = FindProperty("_DetailNormalMapScale", props);
			detailNormalMap = FindProperty("_DetailNormalMap", props);
			uvSetSecondary = FindProperty("_UVSec", props);

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

		public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
		{
			FindProperties(props); // MaterialProperties can be animated so we do not cache them but fetch them every event to ensure animated values are updated correctly
			m_MaterialEditor = materialEditor;
			Material material = materialEditor.target as Material;

			// Make sure that needed setup (ie keywords/renderqueue) are set up if we're switching some existing
			// material to a standard shader.
			// Do this before any GUI code has been issued to prevent layout issues in subsequent GUILayout statements (case 780071)
			if (m_FirstTimeApply)
			{
				MaterialChanged(material, m_WorkflowMode);
				m_FirstTimeApply = false;
			}

			ShaderPropertiesGUI(material);
		}

		public void ShaderPropertiesGUI(Material material)
		{
			// Use default labelWidth
			EditorGUIUtility.labelWidth = 0f;

			// Detect any changes to the material
			EditorGUI.BeginChangeCheck();
			{
				BlendModePopup();

				// Primary properties
				GUILayout.Label(Styles.primaryMapsText, EditorStyles.boldLabel);
				DoAlbedoArea(material);
				DoSpecularMetallicArea();
				m_MaterialEditor.TexturePropertySingleLine(Styles.normalMapText, bumpMap, bumpMap.textureValue != null ? bumpScale : null);
				m_MaterialEditor.TexturePropertySingleLine(Styles.heightMapText, heightMap, heightMap.textureValue != null ? heigtMapScale : null);
				m_MaterialEditor.TexturePropertySingleLine(Styles.occlusionText, occlusionMap, occlusionMap.textureValue != null ? occlusionStrength : null);
				m_MaterialEditor.TexturePropertySingleLine(Styles.detailMaskText, detailMask);
				DoEmissionArea(material);
				EditorGUI.BeginChangeCheck();
				m_MaterialEditor.TextureScaleOffsetProperty(albedoMap);
				if (EditorGUI.EndChangeCheck())
					emissionMap.textureScaleAndOffset = albedoMap.textureScaleAndOffset; // Apply the main texture scale and offset to the emission texture as well, for Enlighten's sake

				EditorGUILayout.Space();

				// Secondary properties
				GUILayout.Label(Styles.secondaryMapsText, EditorStyles.boldLabel);
				m_MaterialEditor.TexturePropertySingleLine(Styles.detailAlbedoText, detailAlbedoMap);
				m_MaterialEditor.TexturePropertySingleLine(Styles.detailNormalMapText, detailNormalMap, detailNormalMapScale);
				m_MaterialEditor.TextureScaleOffsetProperty(detailAlbedoMap);
				m_MaterialEditor.ShaderProperty(uvSetSecondary, Styles.uvSetLabel.text);

				// Third properties
				GUILayout.Label(Styles.forwardText, EditorStyles.boldLabel);
				if (highlights != null)
					m_MaterialEditor.ShaderProperty(highlights, Styles.highlightsText);
				if (reflections != null)
					m_MaterialEditor.ShaderProperty(reflections, Styles.reflectionsText);

			}
			if (EditorGUI.EndChangeCheck())
			{
				foreach (var obj in blendMode.targets)
					MaterialChanged((Material)obj, m_WorkflowMode);
			}

			EditorGUILayout.Space();

			// NB renderqueue editor is not shown on purpose: we want to override it based on blend mode
			GUILayout.Label(Styles.advancedText, EditorStyles.boldLabel);
			m_MaterialEditor.EnableInstancingField();
			m_MaterialEditor.DoubleSidedGIField();

			EditorGUI.BeginChangeCheck ();
			{
				// Dissolve properties
				EditorGUILayout.Space();
				GUILayout.Label(Styles.dissolveSettings, EditorStyles.boldLabel);
				DoDissolveArea(material);
			}

			if (EditorGUI.EndChangeCheck())
			{
				foreach (var obj in blendMode.targets)
					MaterialChanged((Material)obj, m_WorkflowMode);
			}
		}

		internal void DetermineWorkflow(MaterialProperty[] props)
		{
			if (FindProperty("_SpecGlossMap", props, false) != null && FindProperty("_SpecColor", props, false) != null)
				m_WorkflowMode = WorkflowMode.Specular;
			else if (FindProperty("_MetallicGlossMap", props, false) != null && FindProperty("_Metallic", props, false) != null)
				m_WorkflowMode = WorkflowMode.Metallic;
			else
				m_WorkflowMode = WorkflowMode.Dielectric;
		}

		public override void AssignNewShaderToMaterial(Material material, Shader oldShader, Shader newShader)
		{
			// _Emission property is lost after assigning Standard shader to the material
			// thus transfer it before assigning the new shader
			if (material.HasProperty("_Emission"))
			{
				material.SetColor("_EmissionColor", material.GetColor("_Emission"));
			}

			base.AssignNewShaderToMaterial(material, oldShader, newShader);

			if (oldShader == null || !oldShader.name.Contains("Legacy Shaders/"))
			{
				SetupMaterialWithBlendMode(material, (BlendMode)material.GetFloat("_Mode"));
				return;
			}

			BlendMode blendMode = BlendMode.Opaque;
			if (oldShader.name.Contains("/Transparent/Cutout/"))
			{
				blendMode = BlendMode.Cutout;
			}
			else if (oldShader.name.Contains("/Transparent/"))
			{
				// NOTE: legacy shaders did not provide physically based transparency
				// therefore Fade mode
				blendMode = BlendMode.Fade;
			}
			material.SetFloat("_Mode", (float)blendMode);

			DetermineWorkflow(MaterialEditor.GetMaterialProperties(new Material[] { material }));
			MaterialChanged(material, m_WorkflowMode);
		}

		void BlendModePopup()
		{
			EditorGUI.showMixedValue = blendMode.hasMixedValue;
			var mode = (BlendMode)blendMode.floatValue;

			EditorGUI.BeginChangeCheck();
			mode = (BlendMode)EditorGUILayout.Popup(Styles.renderingMode, (int)mode, Styles.blendNames);
			if (EditorGUI.EndChangeCheck())
			{
				m_MaterialEditor.RegisterPropertyChangeUndo("Rendering Mode");
				blendMode.floatValue = (float)mode;
			}

			EditorGUI.showMixedValue = false;
		}

		void DoAlbedoArea(Material material)
		{
			m_MaterialEditor.TexturePropertySingleLine(Styles.albedoText, albedoMap, albedoColor);
			if (((BlendMode)material.GetFloat("_Mode") == BlendMode.Cutout))
			{
				m_MaterialEditor.ShaderProperty(alphaCutoff, Styles.alphaCutoffText.text, MaterialEditor.kMiniTextureFieldLabelIndentLevel + 1);
			}
		}

		void DoEmissionArea(Material material)
		{
			// Emission for GI?
			if (m_MaterialEditor.EmissionEnabledProperty())
			{
				bool hadEmissionTexture = emissionMap.textureValue != null;

				// Texture and HDR color controls
				m_MaterialEditor.TexturePropertyWithHDRColor(Styles.emissionText, emissionMap, emissionColorForRendering, m_ColorPickerHDRConfig, false);

				// If texture was assigned and color was black set color to white
				float brightness = emissionColorForRendering.colorValue.maxColorComponent;
				if (emissionMap.textureValue != null && !hadEmissionTexture && brightness <= 0f)
					emissionColorForRendering.colorValue = Color.white;

				// change the GI flag and fix it up with emissive as black if necessary
				m_MaterialEditor.LightmapEmissionFlagsProperty(MaterialEditor.kMiniTextureFieldLabelIndentLevel, true);
			}
		}

		void DoSpecularMetallicArea()
		{
			bool hasGlossMap = false;
			if (m_WorkflowMode == WorkflowMode.Specular)
			{
				hasGlossMap = specularMap.textureValue != null;
				m_MaterialEditor.TexturePropertySingleLine(Styles.specularMapText, specularMap, hasGlossMap ? null : specularColor);
			}
			else if (m_WorkflowMode == WorkflowMode.Metallic)
			{
				hasGlossMap = metallicMap.textureValue != null;
				m_MaterialEditor.TexturePropertySingleLine(Styles.metallicMapText, metallicMap, hasGlossMap ? null : metallic);
			}

			bool showSmoothnessScale = hasGlossMap;
			if (smoothnessMapChannel != null)
			{
				int smoothnessChannel = (int)smoothnessMapChannel.floatValue;
				if (smoothnessChannel == (int)SmoothnessMapChannel.AlbedoAlpha)
					showSmoothnessScale = true;
			}

			int indentation = 2; // align with labels of texture properties
			m_MaterialEditor.ShaderProperty(showSmoothnessScale ? smoothnessScale : smoothness, showSmoothnessScale ? Styles.smoothnessScaleText : Styles.smoothnessText, indentation);

			++indentation;
			if (smoothnessMapChannel != null)
				m_MaterialEditor.ShaderProperty(smoothnessMapChannel, Styles.smoothnessMapChannelText, indentation);
		}

		// Dissolve settings area
		void DoDissolveArea(Material material)
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

		public static void SetupMaterialWithBlendMode(Material material, BlendMode blendMode)
		{
			switch (blendMode)
			{
			case BlendMode.Opaque:
				material.SetOverrideTag("RenderType", "");
				material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
				material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
				material.SetInt("_ZWrite", 1);
				material.DisableKeyword("_ALPHATEST_ON");
				material.DisableKeyword("_ALPHABLEND_ON");
				material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
				material.renderQueue = -1;
				break;
			case BlendMode.Cutout:
				material.SetOverrideTag("RenderType", "TransparentCutout");
				material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
				material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
				material.SetInt("_ZWrite", 1);
				material.EnableKeyword("_ALPHATEST_ON");
				material.DisableKeyword("_ALPHABLEND_ON");
				material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
				material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.AlphaTest;
				break;
			case BlendMode.Fade:
				material.SetOverrideTag("RenderType", "Transparent");
				material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
				material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
				material.SetInt("_ZWrite", 0);
				material.DisableKeyword("_ALPHATEST_ON");
				material.EnableKeyword("_ALPHABLEND_ON");
				material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
				material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
				break;
			case BlendMode.Transparent:
				material.SetOverrideTag("RenderType", "Transparent");
				material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
				material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
				material.SetInt("_ZWrite", 0);
				material.DisableKeyword("_ALPHATEST_ON");
				material.DisableKeyword("_ALPHABLEND_ON");
				material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
				material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
				break;
			}
		}

		static SmoothnessMapChannel GetSmoothnessMapChannel(Material material)
		{
			int ch = (int)material.GetFloat("_SmoothnessTextureChannel");
			if (ch == (int)SmoothnessMapChannel.AlbedoAlpha)
				return SmoothnessMapChannel.AlbedoAlpha;
			else
				return SmoothnessMapChannel.SpecularMetallicAlpha;
		}

		static void SetMaterialKeywords(Material material, WorkflowMode workflowMode)
		{
			// Dissolve keywords
			SetKeyword (material, "_SUBMAP", material.GetTexture ("_SubTex"));

			// Note: keywords must be based on Material value not on MaterialProperty due to multi-edit & material animation
			// (MaterialProperty value might come from renderer material property block)
			SetKeyword(material, "_NORMALMAP", material.GetTexture("_BumpMap") || material.GetTexture("_DetailNormalMap"));
			if (workflowMode == WorkflowMode.Specular)
				SetKeyword(material, "_SPECGLOSSMAP", material.GetTexture("_SpecGlossMap"));
			else if (workflowMode == WorkflowMode.Metallic)
				SetKeyword(material, "_METALLICGLOSSMAP", material.GetTexture("_MetallicGlossMap"));
			SetKeyword(material, "_PARALLAXMAP", material.GetTexture("_ParallaxMap"));
			SetKeyword(material, "_DETAIL_MULX2", material.GetTexture("_DetailAlbedoMap") || material.GetTexture("_DetailNormalMap"));

			// A material's GI flag internally keeps track of whether emission is enabled at all, it's enabled but has no effect
			// or is enabled and may be modified at runtime. This state depends on the values of the current flag and emissive color.
			// The fixup routine makes sure that the material is in the correct state if/when changes are made to the mode or color.
			MaterialEditor.FixupEmissiveFlag(material);
			bool shouldEmissionBeEnabled = (material.globalIlluminationFlags & MaterialGlobalIlluminationFlags.EmissiveIsBlack) == 0;
			SetKeyword(material, "_EMISSION", shouldEmissionBeEnabled);

			if (material.HasProperty("_SmoothnessTextureChannel"))
			{
				SetKeyword(material, "_SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A", GetSmoothnessMapChannel(material) == SmoothnessMapChannel.AlbedoAlpha);
			}
		}

		static void MaterialChanged(Material material, WorkflowMode workflowMode)
		{
			SetupMaterialWithBlendMode(material, (BlendMode)material.GetFloat("_Mode"));

			SetMaterialKeywords(material, workflowMode);
		}

		static void SetKeyword(Material m, string keyword, bool state)
		{
			if (state)
				m.EnableKeyword(keyword);
			else
				m.DisableKeyword(keyword);
		}
	}
} // namespace UnityEditor
