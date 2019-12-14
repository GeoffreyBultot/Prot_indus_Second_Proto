// MIT license: https://github.com/Fewes/CloudNoiseGen
Shader "WeatherMaker/CloudNoiseShader"
{
	Properties
	{
		_Frame("Frame", Float) = 0.0
		_EndFrame("End Frame", Float) = 1.0
	}

		SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "WeatherMakerCloudNoiseShaderInclude.cginc"

			uniform float _Frame;
			uniform float _EndFrame;
			uniform int _Type;

			float4 _PerlinParams;
			float4 _WorleyParams;

			#define _PerlinOctaves 		floor(_PerlinParams.x)
			#define _PerlinPeriod 		floor(_PerlinParams.y)
			#define _PerlinBrightness 	_PerlinParams.z
			#define _PerlinContrast 	_PerlinParams.w

			#define _WorleyOctaves 		floor(_WorleyParams.x)
			#define _WorleyPeriod 		floor(_WorleyParams.y)
			#define _WorleyBrightness 	_WorleyParams.z
			#define _WorleyContrast 	_WorleyParams.w

			struct v2f
			{
				float4 vertex 	: SV_POSITION;
				float3 texcoord	: TEXCOORD0;
			};

			v2f vert(appdata_full v)
			{
				v2f o;
				UNITY_INITIALIZE_OUTPUT(v2f, o);

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord.xy = v.texcoord.xy;
				o.texcoord.z = _Frame;

				return o;
			}

			float fbm_perlin(float3 st, int octaves, int rep)
			{
				// Initial values
				float value = 0;
				float amplitude = 0.5;
				float frequency = 0;

				for (int i = 0; i < octaves; i++)
				{
					value += amplitude * pnoise(st, rep);
					// st 			*= 2;
					amplitude *= 0.5;
					rep *= 2;
				}

				return value * 0.5 + 0.5; // [-1, 1] -> [0, 1]
			}

			float fbm_worley(float3 st, int octaves, int rep)
			{
				// Initial values
				float value = 0;
				float amplitude = 0.5;
				float frequency = 0;

				for (int i = 0; i < octaves; i++)
				{
					value += amplitude * (1 - worley(st, 1, false, rep).x);
					// st 			*= 2;
					amplitude *= 0.5;
					rep *= 2;
				}

				return value;
			}

			float remap(float value, float original_min, float original_max, float new_min, float new_max)
			{
				return new_min + (((value - original_min) / (original_max - original_min)) * (new_max - new_min));
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// 3D coordinates in normalized [0 1] space
				float3 st = i.texcoord;

				fixed4 color = 0;

				int octaves;
				int basePeriod;

				// Perlin
				float perlin = fbm_perlin(st, _PerlinOctaves, _PerlinPeriod);
				perlin = (perlin - 0.5) * _PerlinContrast + 0.5;
				perlin += _PerlinBrightness - 1;

				// Worley
				float worley = fbm_worley(st, _WorleyOctaves, _WorleyPeriod);
				worley = (worley - 0.5) * _WorleyContrast + 0.5;
				worley += _WorleyBrightness - 1;

				UNITY_BRANCH
				if (_Type == 0)
				{
					color.rgb = worley - perlin * (1 - worley);
				}
				else if (_Type == 1)
				{
					color.rgb = perlin;
				}
				else
				{
					color.rgb = worley;
				}
				color.a = 1;
				return color;
			}
			ENDCG
		}
	}
}