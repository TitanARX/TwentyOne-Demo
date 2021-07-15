using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using RetroLookPro.Enums;

[Serializable]
public sealed class BlendModeParameter : ParameterOverride<BlendingMode> { };

[Serializable]
[PostProcess(typeof(RLPRO_SRP_VHSEffect_Renderer), PostProcessEvent.BeforeStack, "Retro Look Pro/VHS Effect", false)]
public sealed class RLProVHSEffect : PostProcessEffectSettings
{
	[Range(0,50f),Tooltip("Color Offset.")]
	public FloatParameter colorOffset = new FloatParameter { };
	[Range(-3f,3f),Tooltip("Color Offset Angle.")]
	public FloatParameter colorOffsetAngle = new FloatParameter { };
	[Space]
	[Range(0f, 100f), Tooltip("Vertical twitch frequency.")]
	public FloatParameter verticalOffsetFrequency = new FloatParameter { value = 1f };
	[Range(0f, 1f), Tooltip("Amount of vertical twitch. ")]
	public FloatParameter verticalOffset = new FloatParameter { value = 0.1f };
	[Range(3500f, 1f), Tooltip("Amount of horizontal distortion.")]
	public FloatParameter offsetDistortion = new FloatParameter { value = 3499f };
	[Space]
	[Tooltip("Noise texture.")]
	public TextureParameter noiseTexture = new TextureParameter { };
	public BlendModeParameter blendMode = new BlendModeParameter { };
	public Vector2Parameter tile = new Vector2Parameter { value = new Vector2(1, 1) };
	[Space]
	[Range(0f, 1f), Tooltip("Intensity of noise texture.")]
	public FloatParameter _textureIntensity = new FloatParameter { value = 1f };
	[Space]
	public BoolParameter smoothCut = new BoolParameter { value = false };
	[Range(0, 30f), Tooltip(" blur iterations.")]
	public IntParameter iterations = new IntParameter { value = 5 };
	[Range(0, 0.5f), Tooltip("smooth Size.")]
	public FloatParameter smoothSize = new FloatParameter { value = 0.1f };
	[Range(0, 0.5f), Tooltip("deviation.")]
	public FloatParameter deviation = new FloatParameter { value = 0.1f };
	[Space]
	[Range(-1f, 1f), Tooltip("Cut off.")]
	public FloatParameter _textureCutOff = new FloatParameter { value = 1f };
	[Space]
	[Range(0.5f, 0.01f), Tooltip("black bars")]
	public FloatParameter stripes = new FloatParameter { value = 0.5f };
	[Space]
	public BoolParameter unscaledTime = new BoolParameter { value = false };

}

public sealed class RLPRO_SRP_VHSEffect_Renderer : PostProcessEffectRenderer<RLProVHSEffect>
{
	private float T;
	public override void Render(PostProcessRenderContext context)
	{
		var sheet = context.propertySheets.Get(Shader.Find("RetroLookPro/VHS_RetroLook"));
		if (!settings.unscaledTime.value)
			T += Time.deltaTime;
		else
			T += Time.unscaledDeltaTime;
		sheet.properties.SetFloat("Time", T);
		if (UnityEngine.Random.Range(0, 100 - settings.verticalOffsetFrequency) <= 5)
		{
			if (settings.verticalOffset == 0.0f)
			{
				sheet.properties.SetFloat("_OffsetPosY", settings.verticalOffset);
			}
			if (settings.verticalOffset > 0.0f)
			{
				sheet.properties.SetFloat("_OffsetPosY", settings.verticalOffset - UnityEngine.Random.Range(0f, settings.verticalOffset));
			}
			else if (settings.verticalOffset < 0.0f)
			{
				sheet.properties.SetFloat("_OffsetPosY", settings.verticalOffset + UnityEngine.Random.Range(0f, -settings.verticalOffset));
			}
		}

		sheet.properties.SetFloat("iterations", settings.iterations.value);
		sheet.properties.SetFloat("smoothSize", settings.smoothSize.value);
		sheet.properties.SetFloat("_StandardDeviation", settings.deviation.value);

		sheet.properties.SetFloat("tileX", settings.tile.value.x);
		sheet.properties.SetFloat("smooth", settings.smoothCut.value ? 1 : 0);
		sheet.properties.SetFloat("tileY", settings.tile.value.y);
		sheet.properties.SetFloat("_OffsetDistortion", settings.offsetDistortion);
		sheet.properties.SetFloat("_Stripes", settings.stripes);

		sheet.properties.SetVector("_OffsetColorAngle", new Vector2(Mathf.Sin(settings.colorOffsetAngle),
				Mathf.Cos(settings.colorOffsetAngle)));
		sheet.properties.SetFloat("_OffsetColor", settings.colorOffset * 0.001f);

		sheet.properties.SetFloat("_OffsetNoiseX", UnityEngine.Random.Range(-0.4f, 0.4f));
		if (settings.noiseTexture.value != null)
			sheet.properties.SetTexture("_SecondaryTex", settings.noiseTexture);

		float offsetNoise = sheet.properties.GetFloat("_OffsetNoiseY");
		sheet.properties.SetFloat("_OffsetNoiseY", offsetNoise + UnityEngine.Random.Range(-0.03f, 0.03f));
		sheet.properties.SetFloat("_Intensity", settings._textureIntensity);
		sheet.properties.SetFloat("_TexCut", settings._textureCutOff);

		context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, (int)settings.blendMode.value);
	}
}