Shader "Custom/ProjectorCalibrationShader" {
	Properties {
	//la renderTexture en entrée
		_SourceTex ("Source texture (RGB)", 2D) = "white" {}
		//les 3 textures contenant les données envoyées après la conversion en UV en CPU
		_DataTex ("Data texture (RGB)", 2D) = "white" {}
		_xTex ("xTexture (RGB)", 2D) = "white" {}
		_yTex ("yTexture (RGB)", 2D) = "white" {}
		
	}

	SubShader {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
        Lighting Off
        Cull Off
        ZTest Always
        ZWrite Off
        Fog { Mode Off }

		CGPROGRAM

		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0
		#include "UnityCG.cginc"
		//les textures importées
		sampler2D _SourceTex;
		sampler2D _DataTex;
		sampler2D _xTex;
		sampler2D _yTex;
		//le mapping uv du pixel d'entrée
		struct Input {
			float2 uv_SourceTex;
		};

		void surf (Input IN, inout SurfaceOutputStandard o) {

			//Importation des textures et de leurs uv mapping pour le traitement sur chaque pixel.
			fixed4 data = tex2D (_DataTex, IN.uv_SourceTex);
			fixed4 colo = tex2D (_SourceTex, IN.uv_SourceTex);
			//récupère directement le float en décodant la couleur récupérée dans la texture
			float xvalue = DecodeFloatRGBA(tex2D (_xTex, IN.uv_SourceTex));
			float yvalue = DecodeFloatRGBA(tex2D (_yTex, IN.uv_SourceTex));

			//récupère la nouvelle couleur du pixel en allant chercher le pixel dans la texture source aux coordonnées UVX/UVY décodées 
			fixed4 c=tex2D(_SourceTex,fixed2(xvalue,yvalue));
			//la redéfinit en multipliant par data.a qui vaut 1 ou 0 en fonction de si on est dans ou hors du poly
			o.Albedo = c * data.a;
		}
		ENDCG
	}
	//FallBack "Diffuse"
}
