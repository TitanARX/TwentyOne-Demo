using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(RLPRO_SRP_CustomTexture_Renderer), PostProcessEvent.BeforeStack, "Retro Look Pro/Custom Texture", false)]
public sealed class RLProCustomTexture : PostProcessEffectSettings
{
	[Tooltip("Your custom texture.")]
	public TextureParameter texture = new TextureParameter { };
	[Range(0f, 1f), Tooltip("Fade parameter.")]
	public FloatParameter fade = new FloatParameter { value = 1f };
}

public sealed class RLPRO_SRP_CustomTexture_Renderer : PostProcessEffectRenderer<RLProCustomTexture>
{
	public override void Render(PostProcessRenderContext context)
	{
		var sheet = context.propertySheets.Get(Shader.Find("RetroLookPro/CustomTexture"));
		sheet.properties.SetFloat("fade", settings.fade.value);
		if (settings.texture.value != null)
			sheet.properties.SetTexture("_CustomTex", settings.texture.value);
		context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
	}
}
