////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Martin Bustos @FronkonGames <fronkongames@gmail.com>. All rights reserved.
//
// THIS FILE CAN NOT BE HOSTED IN PUBLIC REPOSITORIES.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEditor;
using static FronkonGames.Glitches.VHS.Inspector;

namespace FronkonGames.Glitches.VHS.Editor
{
  /// <summary> VHS inspector. </summary>
  [CustomPropertyDrawer(typeof(VHS.Settings))]
  public class VHSFeatureSettingsDrawer : Drawer
  {
    private VHS.Settings settings;

    protected override void ResetValues() => settings?.ResetDefaultValues();

    protected override void InspectorGUI()
    {
      settings ??= GetSettings<VHS.Settings>();

      /////////////////////////////////////////////////
      // Common.
      /////////////////////////////////////////////////
      settings.intensity = Slider("Intensity", "Controls the intensity of the effect [0, 1]. Default 0.", settings.intensity, 0.0f, 1.0f, 1.0f);

      /////////////////////////////////////////////////
      // VHS.
      /////////////////////////////////////////////////
      Separator();

      settings.noise = Slider("Noise", "Noise intensity [0, 1]. Default 0.3.", settings.noise, 0.0f, 1.0f, 0.3f);
      IndentLevel++;
      settings.noiseSpeed = Slider("Speed", "Effect speed [-10.0 - 10.0]. Default 2.", settings.noiseSpeed, -10.0f, 10.0f, 2.0f);
      settings.noiseSize = Slider("Size", "Noise size [0.0 - 1.0]. Default 0.2.", settings.noiseSize, 0.0f, 1.0f, 0.2f);
      settings.noiseBlend = (ColorBlends)EnumPopup("Blend", "Noise color blend operation. Default Additive.", settings.noiseBlend, ColorBlends.Additive);
      settings.noiseColor = ColorField("Color", "Noise color. Default white.", settings.noiseColor, Color.white);
      IndentLevel--;

      settings.pause = Slider("Pause", "", settings.pause, 0.0f, 1.0f, 0.25f);
      IndentLevel++;
      settings.pauseNoise = Slider("Noise", "Noise size [0.0 - 1.0]. Default 0.2.", settings.pauseNoise, 0.0f, 1.0f, 0.1f);
      IndentLevel--;
      settings.pauseBand = Slider("Pause Band", "Pause band width [0, 2]. Default 1.", settings.pauseBand, 0.0f, 2.0f, 1.0f);
      IndentLevel++;
      settings.pauseBlend = (ColorBlends)EnumPopup("Blend", "Pause color blend operation. Default Solid.", settings.pauseBlend, ColorBlends.Solid);
      settings.pauseColor = ColorField("Color", "Pause color. Default white", settings.pauseColor, Color.white);
      IndentLevel--;

      /////////////////////////////////////////////////
      // Color.
      /////////////////////////////////////////////////
      Separator();

      if (Foldout("Color") == true)
      {
        IndentLevel++;

        settings.brightness = Slider("Brightness", "Brightness [-1.0, 1.0]. Default 0.", settings.brightness, -1.0f, 1.0f, 0.0f);
        settings.contrast = Slider("Contrast", "Contrast [0.0, 10.0]. Default 1.", settings.contrast, 0.0f, 10.0f, 1.0f);
        settings.gamma = Slider("Gamma", "Gamma [0.1, 10.0]. Default 1.", settings.gamma, 0.01f, 10.0f, 1.0f);
        settings.hue = Slider("Hue", "The color wheel [0.0, 1.0]. Default 0.", settings.hue, 0.0f, 1.0f, 0.0f);
        settings.saturation = Slider("Saturation", "Intensity of a colors [0.0, 2.0]. Default 1.", settings.saturation, 0.0f, 2.0f, 1.0f);

        IndentLevel--;
      }

      /////////////////////////////////////////////////
      // Advanced.
      /////////////////////////////////////////////////
      Separator();

      if (Foldout("Advanced") == true)
      {
        IndentLevel++;

#if !UNITY_6000_0_OR_NEWER
        settings.filterMode = (FilterMode)EnumPopup("Filter mode", "Filter mode. Default Bilinear.", settings.filterMode, FilterMode.Bilinear);
#endif
        settings.affectSceneView = Toggle("Affect the Scene View?", "Does it affect the Scene View?", settings.affectSceneView);
        settings.whenToInsert = (UnityEngine.Rendering.Universal.RenderPassEvent)EnumPopup("RenderPass event",
          "Render pass injection. Default BeforeRenderingPostProcessing.",
          settings.whenToInsert,
          UnityEngine.Rendering.Universal.RenderPassEvent.BeforeRenderingPostProcessing);
#if !UNITY_6000_0_OR_NEWER
        settings.enableProfiling = Toggle("Enable profiling", "Enable render pass profiling", settings.enableProfiling);
#endif

        IndentLevel--;
      }
    }
  }
}
