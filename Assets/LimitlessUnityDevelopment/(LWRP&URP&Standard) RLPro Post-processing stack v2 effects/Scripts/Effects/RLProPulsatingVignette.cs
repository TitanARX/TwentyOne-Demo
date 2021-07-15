using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(RLPRO_SRP_PulsatingVignetteRenderer), PostProcessEvent.BeforeStack, "Retro Look Pro/Pulsating Vignette", false)]
public sealed class RLProPulsatingVignette : PostProcessEffectSettings
{
    [Range(0.001f, 50f), Tooltip("Vignette shake speed.")]
    public FloatParameter speed = new FloatParameter { value = 1f };
        [Range(0.001f, 50f), Tooltip("Vignette amount.")]
    public FloatParameter amount = new FloatParameter { value = 1f };
}

public sealed class RLPRO_SRP_PulsatingVignetteRenderer : PostProcessEffectRenderer<RLProPulsatingVignette>
{
	private float T;
    public override void Render(PostProcessRenderContext context)
    {
		T += Time.deltaTime;
        var sheet = context.propertySheets.Get(Shader.Find("RetroLookPro/PulsatingVignette"));
		sheet.properties.SetFloat("_Time", T);
		sheet.properties.SetFloat("vignetteSpeed", settings.speed);
        sheet.properties.SetFloat("vignetteAmount", settings.amount);

        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}
