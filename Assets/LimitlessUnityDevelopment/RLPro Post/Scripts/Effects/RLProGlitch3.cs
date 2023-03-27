using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(Glitch3Renderer), PostProcessEvent.BeforeStack, "Retro Look Pro/Glitch3", false)]
public sealed class RLProGlitch3 : PostProcessEffectSettings
{
    [Range(0f, 1f), Tooltip("Speed")]
    public FloatParameter speed = new FloatParameter { value = 1f };
    [Range(0f, 5f), Tooltip("block size (higher value = smaller blocks).")]
    public FloatParameter density = new FloatParameter { value = 1f };
    [Range(0f, 5f), Tooltip("glitch offset.(color shift)")]
    public FloatParameter maxDisplace = new FloatParameter { value = 1f };
    [Space]
    public BoolParameter mask = new BoolParameter { value = false };
    public TextureParameter maskTexture = new TextureParameter { value = null };
}

public sealed class Glitch3Renderer : PostProcessEffectRenderer<RLProGlitch3>
{
	private float T;
    public override void Render(PostProcessRenderContext context)
    {
		T += Time.deltaTime;
        var sheet = context.propertySheets.Get(Shader.Find("RetroLookPro/Glitch3"));
        if (settings.mask.value)
        {
            sheet.properties.SetFloat("alphaTex", 1);
            if (settings.maskTexture.value != null)
                sheet.properties.SetTexture("_AlphaMapTex", settings.maskTexture);
        }
        else
        {
            sheet.properties.SetFloat("alphaTex", 0);
        }
        sheet.properties.SetFloat("speed", settings.speed);
        sheet.properties.SetFloat("density", settings.density);
        sheet.properties.SetFloat("maxDisplace", settings.maxDisplace);
		sheet.properties.SetFloat("_Time", T);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}