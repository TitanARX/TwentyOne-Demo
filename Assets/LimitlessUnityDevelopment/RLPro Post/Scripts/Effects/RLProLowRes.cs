using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using RetroLookPro.Enums;

[Serializable]
[PostProcess(typeof(RLPRO_SRP_LowRes_Renderer), PostProcessEvent.BeforeStack, "Retro Look Pro/LowRes", false)]
public sealed class RLProLowRes : PostProcessEffectSettings
{
	[Range(1, 20), Tooltip("Dark areas adjustment.")]
	public IntParameter pixelSize = new IntParameter { value = 1 };
}

public sealed class RLPRO_SRP_LowRes_Renderer : PostProcessEffectRenderer<RLProLowRes>
{
	public override void Render(PostProcessRenderContext context)
	{
		var sheet = context.propertySheets.Get(Shader.Find("RetroLookPro/LowRes_RLPro"));
		Vector2Int res = new Vector2Int(Screen.width / settings.pixelSize, Screen.height / settings.pixelSize);
		RenderTexture scaled = RenderTexture.GetTemporary(res.x,res.y);
		scaled.filterMode = FilterMode.Point;
		context.command.BlitFullscreenTriangle(context.source, scaled, sheet, 1);
		context.command.BlitFullscreenTriangle(scaled, context.destination, sheet, 0);
		RenderTexture.ReleaseTemporary(scaled);
	}
}
