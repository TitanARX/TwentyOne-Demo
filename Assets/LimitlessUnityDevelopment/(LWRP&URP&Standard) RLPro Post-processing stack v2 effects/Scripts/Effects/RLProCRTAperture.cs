using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(RLProCRTAperture_Renderer), PostProcessEvent.BeforeStack, "Retro Look Pro/CRT Aperture", false)]
public sealed class RLProCRTAperture : PostProcessEffectSettings
{
    [Range(0, 5), Tooltip(".")]
    public FloatParameter GlowHalation = new FloatParameter { value = 4.27f };
    [Range(0, 2), Tooltip(".")]
    public FloatParameter GlowDifusion = new FloatParameter { value = 0.83f };
    [Range(0, 2), Tooltip(".")]
    public FloatParameter MaskColors = new FloatParameter { value = 0.57f };
    [Range(0, 1), Tooltip(".")]
    public FloatParameter MaskStrength = new FloatParameter { value = 0.318f };
    [Range(0, 5), Tooltip(".")]
    public FloatParameter GammaInput = new FloatParameter { value = 1.12f };
    [Range(0, 5), Tooltip(".")]
    public FloatParameter GammaOutput = new FloatParameter { value = 0.89f };
    [Range(0, 2.5f), Tooltip(".")]
    public FloatParameter Brightness = new FloatParameter { value = 0.85f };
}

public sealed class RLProCRTAperture_Renderer : PostProcessEffectRenderer<RLProCRTAperture>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("RetroLookPro/CRTAperture_RLPRO"));

        sheet.properties.SetFloat("GLOW_HALATION", settings.GlowHalation.value);
        sheet.properties.SetFloat("GLOW_DIFFUSION", settings.GlowDifusion.value);
        sheet.properties.SetFloat("MASK_COLORS", settings.MaskColors.value);
        sheet.properties.SetFloat("MASK_STRENGTH", settings.MaskStrength.value);
        sheet.properties.SetFloat("GAMMA_INPUT", settings.GammaInput.value);
        sheet.properties.SetFloat("GAMMA_OUTPUT", settings.GammaOutput.value);
        sheet.properties.SetFloat("BRIGHTNESS", settings.Brightness.value);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}
