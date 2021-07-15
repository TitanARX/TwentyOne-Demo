using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering.PostProcessing;

namespace UnityEditor.Rendering.PostProcessing
{
    [PostProcessEditor(typeof(RLProVHSEffect))]
    internal sealed class RLPro_VHS_Editor : PostProcessEffectEditor<RLProVHSEffect>
    {

        SerializedParameterOverride colorOffset;
        SerializedParameterOverride colorOffsetAngle;
        SerializedParameterOverride verticalOffsetFrequency;
        SerializedParameterOverride verticalOffset;

        SerializedParameterOverride offsetDistortion;
        SerializedParameterOverride noiseTexture;
        SerializedParameterOverride blendMode;
        SerializedParameterOverride tile;
        SerializedParameterOverride _textureIntensity;
		SerializedParameterOverride smoothCut;
		SerializedParameterOverride iterations;
		SerializedParameterOverride smoothSize;
		SerializedParameterOverride deviation;
		SerializedParameterOverride _textureCutOff;
		SerializedParameterOverride stripes;
		SerializedParameterOverride unscaledTime;

		//bool laal;

		public override void OnEnable()
		{
			colorOffset = FindParameterOverride(x => x.colorOffset);
			colorOffsetAngle = FindParameterOverride(x => x.colorOffsetAngle);
			verticalOffsetFrequency = FindParameterOverride(x => x.verticalOffsetFrequency);
			verticalOffset = FindParameterOverride(x => x.verticalOffset);
			offsetDistortion = FindParameterOverride(x => x.offsetDistortion);
			noiseTexture = FindParameterOverride(x => x.noiseTexture);
			blendMode = FindParameterOverride(x => x.blendMode);
			tile = FindParameterOverride(x => x.tile);

			_textureIntensity = FindParameterOverride(x => x._textureIntensity);
			smoothCut = FindParameterOverride(x => x.smoothCut);
			iterations = FindParameterOverride(x => x.iterations);
			smoothSize = FindParameterOverride(x => x.smoothSize);
			deviation = FindParameterOverride(x => x.deviation);
			_textureCutOff = FindParameterOverride(x => x._textureCutOff);
			stripes = FindParameterOverride(x => x.stripes);
			unscaledTime = FindParameterOverride(x => x.unscaledTime);
		}

		public override void OnInspectorGUI()
        {
			//EditorGUILayout.LabelField("Color Shift");
            PropertyField(colorOffset);
            PropertyField(colorOffsetAngle);

			//EditorGUILayout.LabelField("Vertical Twitch");
			PropertyField(verticalOffsetFrequency);
			PropertyField(verticalOffset);
			//EditorGUILayout.LabelField("Image Distortion");
			PropertyField(offsetDistortion);
			//EditorGUILayout.LabelField("Noise Texture Properties");
			PropertyField(noiseTexture);
			PropertyField(blendMode);
			PropertyField(tile);
			PropertyField(_textureIntensity);
			PropertyField(smoothCut);

			if (smoothCut.value.boolValue == true)
            {
                PropertyField(iterations);
                PropertyField(smoothSize);
                PropertyField(deviation);
            }
			PropertyField(_textureCutOff);

			//EditorGUILayout.LabelField("Black Bars");
			PropertyField(stripes);
			PropertyField(unscaledTime);

		}
    }
}
