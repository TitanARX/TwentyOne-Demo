using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using RetroLookPro.Enums;

[Serializable]
[PostProcess(typeof(RLPRO_SRP_AnalogTVNoise_Renderer), PostProcessEvent.BeforeStack, "Retro Look Pro/Analog TV Noise", false)]
public sealed class RLProAnalogTVNoise : PostProcessEffectSettings
{
    [Tooltip("Option enables static noise (without movement).")]
    public BoolParameter staticNoise = new BoolParameter { };
    [Tooltip("Horizontal/Vertical Noise lines.")]
    public BoolParameter Horizontal = new BoolParameter { value = true };
    [Range(0f, 1f), Tooltip("Effect Fade.")]
    public FloatParameter Fade = new FloatParameter { value = 1f };
    [Range(0f, 60f), Tooltip("Noise bar width.")]
    public FloatParameter barHeight = new FloatParameter { value = 21f };
        [Range(0f, 60f), Tooltip("Noise tiling.")]
    public Vector2Parameter tile = new Vector2Parameter { value = new Vector2(1,1) };
        [Range(0f, 1f), Tooltip("Noise texture angle.")]
    public FloatParameter textureAngle = new FloatParameter { value = 1f };
    [Range(0f, 100f), Tooltip("Noise bar edges cutoff.")]
    public FloatParameter edgeCutOff = new FloatParameter { value = 0f };
    [Range(-1f, 1f), Tooltip("Noise cutoff.")]
    public FloatParameter CutOff = new FloatParameter { value = 1f };
    [Range(-10f, 100f), Tooltip("Noise bars speed.")]
    public FloatParameter barSpeed = new FloatParameter { value = 1f };
    [Tooltip("Noise texture.")]
    public TextureParameter texture = new TextureParameter { };
}

public sealed class RLPRO_SRP_AnalogTVNoise_Renderer : PostProcessEffectRenderer<RLProAnalogTVNoise>
{
    float TimeX;
    public override void Render(PostProcessRenderContext context)
    {
        TimeX += Time.deltaTime;
        if (TimeX > 100) TimeX = 0;
        
        var sheet = context.propertySheets.Get(Shader.Find("RetroLookPro/AnalogTVNoise"));

        sheet.properties.SetFloat("_TimeX", TimeX);
        sheet.properties.SetFloat("_Fade", settings.Fade);
        if(settings.texture.value != null)
        sheet.properties.SetTexture("_Pattern", settings.texture);
        sheet.properties.SetFloat("barHeight", settings.barHeight);
        sheet.properties.SetFloat("barSpeed", settings.barSpeed);
        sheet.properties.SetFloat("cut", settings.CutOff);
        sheet.properties.SetFloat("edgeCutOff", settings.edgeCutOff);
        sheet.properties.SetFloat("angle", settings.textureAngle);
        sheet.properties.SetFloat("tileX", settings.tile.value.x);
        sheet.properties.SetFloat("tileY", settings.tile.value.y);
        if (!settings.staticNoise.value)
        {
            sheet.properties.SetFloat("_OffsetNoiseX", UnityEngine.Random.Range(0f, 0.6f));
            sheet.properties.SetFloat("_OffsetNoiseY", UnityEngine.Random.Range(0f, 0.6f));
        }
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, settings.Horizontal.value ? 0 : 1);
    }
}
