using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using RetroLookPro.Enums;

[Serializable]
[PostProcess(typeof(RLPRO_SRP_NTSC_Renderer), PostProcessEvent.BeforeStack, "Retro Look Pro/NTSC", false)]
public sealed class RLPro_NTSC : PostProcessEffectSettings
{
	[Range(1,40),Tooltip("Brightness.")]
	public FloatParameter brightness = new FloatParameter { value = 39.1f};
	[Range(0.01f, 2f), Tooltip("Blur size.")]
	public FloatParameter blur = new FloatParameter { value = 0.83f };
	[Range(0, 10), Tooltip("Floating lines speed")]
	public FloatParameter lineSpeed = new FloatParameter { value =  0.01f};
}

public sealed class RLPRO_SRP_NTSC_Renderer : PostProcessEffectRenderer<RLPro_NTSC>
{
	private float T;
	public override void Render(PostProcessRenderContext context)
	{
		var sheet = context.propertySheets.Get(Shader.Find("RetroLookPro/NTSC_RLPro"));
		T += Time.deltaTime;
		sheet.properties.SetFloat("T", T);
		sheet.properties.SetFloat("Bsize", 41- settings.brightness.value);
		sheet.properties.SetFloat("val1", settings.lineSpeed.value);
		sheet.properties.SetFloat("val2", settings.blur.value);
		context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
	}
}
