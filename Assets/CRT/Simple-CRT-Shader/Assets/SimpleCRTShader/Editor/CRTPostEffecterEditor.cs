using UnityEditor;
using UnityEngine;
using static CRTPostEffecter;
using static UnityEditor.EditorGUILayout;

[CustomEditor(typeof(CRTPostEffecter))]
public class CRTPostEffecterEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        CRTPostEffecter effect = target as CRTPostEffecter;
        effect.material = (Material)ObjectField("Effect Material",
            effect.material, typeof(Material), false);

        using (new HorizontalScope(GUI.skin.box))
        {
            effect.whiteNoiseFrequency = IntField(
                "White Noise Freaquency (x/1000)", effect.whiteNoiseFrequency);
            effect.whiteNoiseLength = FloatField("White Noise Time Left (sec)",
                effect.whiteNoiseLength);
        }

        using (new VerticalScope(GUI.skin.box))
        {
            effect.screenJumpFrequency = IntField(
                "Screen Jump Freaquency (x/1000)", effect.screenJumpFrequency);
            effect.screenJumpLength = FloatField("Screen Jump Length",
                effect.screenJumpLength);
            using (new HorizontalScope())
            {
                effect.screenJumpMinLevel =
                    FloatField("min", effect.screenJumpMinLevel);
                effect.screenJumpMaxLevel =
                    FloatField("max", effect.screenJumpMaxLevel);
            }
        }

        using (new HorizontalScope(GUI.skin.box))
        {
            effect.isScanline = Toggle("Scanline On / Off", effect.isScanline);
        }

        using (new HorizontalScope(GUI.skin.box))
        {
            effect.isMonochrome =
                Toggle("Monochrome On / Off", effect.isMonochrome);
        }

        using (new HorizontalScope(GUI.skin.box))
        {
            effect.flickeringStrength = FloatField("Flickering Strength",
                effect.flickeringStrength);
            effect.flickeringCycle =
                FloatField("Flickering Cycle", effect.flickeringCycle);
        }

        using (new VerticalScope(GUI.skin.box))
        {
            effect.isSlippage = Toggle("Slippage On / Off", effect.isSlippage);
            effect.isSlippageNoise =
                Toggle("Slippage Noise", effect.isSlippageNoise);
            effect.slippageStrength = FloatField("Slippage Strength",
                effect.slippageStrength);
            effect.slippageInterval = FloatField("Slippage Interval",
                effect.slippageInterval);
            effect.slippageScrollSpeed = FloatField("Slippage Scroll Speed",
                effect.slippageScrollSpeed);
            effect.slippageSize =
                FloatField("Slippage Size", effect.slippageSize);
        }

        using (new VerticalScope(GUI.skin.box))
        {
            effect.isChromaticAberration = Toggle(
                "Chromatic Aberration On / Off", effect.isChromaticAberration);
            effect.chromaticAberrationStrength = FloatField(
                "Chromatic Aberration Strength",
                effect.chromaticAberrationStrength);
        }

        using (new VerticalScope(GUI.skin.box))
        {
            effect.isMultipleGhost = Toggle("Multiple Ghost On / Off",
                effect.isMultipleGhost);
            effect.multipleGhostStrength = FloatField("Multiple Ghost Strength",
                effect.multipleGhostStrength);
        }

        using (new VerticalScope(GUI.skin.box))
        {
            effect.isLetterBox =
                Toggle("Letter Box On / Off", effect.isLetterBox);
            effect.letterBoxType =
                (LeterBoxType)EnumPopup("Letter Box Type",
                    effect.letterBoxType);
        }

        using (new VerticalScope(GUI.skin.box))
        {
            effect.isDecalTex = Toggle("Decal Tex On / Off", effect.isDecalTex);
            effect.decalTex = (Texture2D)ObjectField("Decal Tex",
                effect.decalTex, typeof(Texture2D), false);
            effect.decalTexPos =
                Vector2Field("Decal Tex Position", effect.decalTexPos);
            effect.decalTexScale =
                Vector2Field("Decal Tex Scale", effect.decalTexScale);
        }

        using (new VerticalScope(GUI.skin.box))
        {
            effect.isLowResolution =
                Toggle("Low Resolution", effect.isLowResolution);
            //effect.resolutions = EditorGUILayout.Vector2IntField("Resolutions", effect.resolutions);
        }

        using (new VerticalScope(GUI.skin.box))
        {
            effect.isFilmDirt = Toggle("Film Dirt", effect.isFilmDirt);
            effect.filmDirtTex = (Texture2D)ObjectField("Film Dirt Tex",
                effect.filmDirtTex, typeof(Texture2D), false);
        }

        EditorUtility.SetDirty(target);
    }
}
