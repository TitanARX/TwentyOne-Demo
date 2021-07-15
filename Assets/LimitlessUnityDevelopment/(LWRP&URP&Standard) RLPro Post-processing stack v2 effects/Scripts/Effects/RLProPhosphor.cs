using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using RetroLookPro.Enums;

[Serializable]
[PostProcess(typeof(RLPRO_SRP_Phosphor_Renderer), PostProcessEvent.BeforeStack, "Retro Look Pro/Phosphor", false)]
public sealed class RLProPhosphor : PostProcessEffectSettings
{
	[Range(0f, 1f), Tooltip("fade.")]
	public FloatParameter fade = new FloatParameter { value = 1f };
	[Range(0f, 20f), Tooltip("width.")]
	public FloatParameter width = new FloatParameter { value = 0.4f };
	[Range(0f, 1f), Tooltip("amount.")]
	public FloatParameter amount = new FloatParameter { value = 0.5f };
}

public sealed class RLPRO_SRP_Phosphor_Renderer : PostProcessEffectRenderer<RLProPhosphor>
{
	private RenderTexture texTape = null;
	bool stop;

	float T;
	public override void Render(PostProcessRenderContext context)
	{
		var sheet = context.propertySheets.Get(Shader.Find("RetroLookPro/Phosphor_RLPro"));
		//HDUtils.DrawFullScreen(cmd, sheet.properties, texTape, shaderPassId: 1);
		if (texTape == null)
		{
			texTape = new RenderTexture(Screen.width, Screen.height, 1);
		}
		context.command.BlitFullscreenTriangle(context.source, texTape, sheet, 1);
		sheet.properties.SetTexture("_Tex", texTape);
		T = Time.time;
		sheet.properties.SetFloat("T", T);
		sheet.properties.SetFloat("speed", settings.width.value);
		sheet.properties.SetFloat("amount", settings.amount.value + 1);
		sheet.properties.SetFloat("fade", settings.fade.value);

		texTape.Release();
		context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
	}
}
