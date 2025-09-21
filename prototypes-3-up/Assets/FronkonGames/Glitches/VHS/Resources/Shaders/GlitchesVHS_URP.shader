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
Shader "Hidden/Fronkon Games/Glitches/VHS URP"
{
  Properties
  {
    _MainTex("Main Texture", 2D) = "white" {}
  }

  SubShader
  {
    Tags
    {
      "RenderType" = "Opaque"
      "RenderPipeline" = "UniversalPipeline"
    }
    LOD 100
    ZTest Always ZWrite Off Cull Off

    Pass
    {
      Name "Fronkon Games Glitches VHS Pass"

      HLSLPROGRAM
      #pragma vertex GlitchesVert
      #pragma fragment GlitchesFrag
      #pragma fragmentoption ARB_precision_hint_fastest
      #pragma exclude_renderers d3d9 d3d11_9x ps3 flash
      #pragma multi_compile_instancing
      #pragma multi_compile _ STEREO_INSTANCING_ON
      #pragma multi_compile _ UNITY_SINGLE_PASS_STEREO STEREO_MULTIVIEW_ON

      #include "Glitches.hlsl"
      #include "ColorBlend.hlsl"

      float _Noise;
      float _NoiseSpeed;
      float _NoiseSize;
      int _NoiseBlend;
      float3 _NoiseColor;

      float _Pause;
      float _PauseNoise;
      float _PauseBand;
      int _PauseBlend;
      float3 _PauseColor;

      inline float4 Hash42(float2 n)
      {
        float4 p4 = frac(n.xyxy * float4(443.8975,397.2973, 491.1871, 470.7827));
        p4 += dot(p4.wzxy, p4 + 19.19);
          
        return frac(float4(p4.x * p4.y, p4.x * p4.z, p4.y * p4.w, p4.x * p4.w));
      }

      // 3D noise function (www.iquilezles.org).
      inline float Noise3D(float3 x)
      {
        float3 p = floor(x);
        float3 f = frac(x);
        
        f = f * f * (3.0 - 2.0 * f);

        float n = p.x + p.y * 57.0 + 113.0 * p.z;
        float res = lerp(lerp(lerp(Rand(n + 0.0), Rand(n + 1.0), f.x),
                    lerp(Rand(n + 57.0), Rand(n + 58.0), f.x), f.y),
                    lerp(lerp(Rand(n + 113.0), Rand(n + 114.0), f.x),
                    lerp(Rand(n + 170.0), Rand(n + 171.0), f.x), f.y), f.z);

        return res;
      }

      inline float TapeNoise(float2 p)
      {
        float y = p.y;
        float s = _Time.y * _NoiseSpeed;

        float v = (Noise3D(float3(y * 0.01 + s, 1.0, 1.0)) + 0.0) *
                  (Noise3D(float3(y * 0.011 + 1000.0 + s, 1.0, 1.0)) + 0.0) * 
                  (Noise3D(float3(y * 0.51 + 421.0 + s, 1.0, 1.0)) + 0.0);

        v *= Hash42(float2(p.x + _Time.y * 0.01, p.y)).x + _Noise;
        v = pow(v + _Noise, 1.0);

        return v < 0.7 ? 0.0 : v;
      }
      
      half4 GlitchesFrag(GlitchesVaryings input) : SV_Target
      {
        UNITY_SETUP_INSTANCE_ID(input);
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
        const float2 uv = UnityStereoTransformScreenSpaceTex(input.texcoord).xy;

        const half4 color = SAMPLE_MAIN(uv);
        half4 pixel = color;

        float2 uv2 = uv;
        uv2.x += ((Rand(float2(_Time.y, uv.y)) - 0.5) / 64.0) * _PauseNoise;
        uv2.y += ((Rand(_Time.y) - 0.5) / 32.0) * _PauseNoise;

        UNITY_BRANCH
        if (_NoiseSize > 0.0)
        {
          float oneY = _ScreenParams.y / _NoiseSize;

          pixel.rgb = ColorBlend(_NoiseBlend, pixel.rgb, _NoiseColor * TapeNoise(floor(uv2 * _ScreenParams.xy / oneY) * oneY));
        }

        float3 pause = (-0.5 + float3(Rand(float2(uv.y, _Time.y)),
                                      Rand(float2(uv.y, _Time.y + 1.0)),
                                      Rand(float2(uv.y, _Time.y + 2.0)))) * _PauseColor;

        float noise = Rand(float2(floor(uv2.y * 80.0), floor(uv2.x * 50.0)) + float2(_Time.y, 0.0)) * _PauseBand;

        UNITY_BRANCH
        if (noise > (11.5 * _PauseBand) - 30.0 * uv2.y || noise < 1.5 - 5.0 * uv2.y)
          pixel.rgb = lerp(pixel.rgb, pause + SAMPLE_MAIN(uv2).rgb, _Pause);
        else
          pixel.rgb = lerp(pixel.rgb, ColorBlend(_PauseBlend, pixel.rgb, _PauseColor.rgb), _PauseBand);

        pixel.rgb = ColorAdjust(pixel.rgb, _Contrast, _Brightness, _Hue, _Gamma, _Saturation);

        return lerp(color, pixel, _Intensity);
      }

      ENDHLSL
    }
  }
  
  FallBack "Diffuse"
}
