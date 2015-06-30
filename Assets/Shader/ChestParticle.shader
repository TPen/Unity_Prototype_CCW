// Copyright (C) 2015, Felix Kate; All rights reserved
//
// <Class content>
// For the particles around the chest objects
//


Shader "Custom/ChestParticle" {
	Properties {
		_MainTex ("Texture", 2D) = "white" { }
	}
	SubShader {
		Tags { "RenderType"="Transparent" "Quene"="Transparent + 10" }
		
		Lighting Off
		
		Alphatest Equal 0
		AlphaToMask True
        ColorMask RGB
		
		Pass{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			
			float4 _MainTex_ST;

	        struct vertInput{
	        	float4 vertex : POSITION;
	         	float4 color : COLOR;
	         	float2 texcoord : TEXCOORD0;
	        };

			struct v2f{
				float4 pos : SV_POSITION;
				fixed4 color : COLOR;
				float2 uv : TEXCOORD0;
			};
			
			v2f vert(vertInput v){
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.color = v.color;
				o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target {
				return tex2D(_MainTex, i.uv) * i.color;
			}
			ENDCG
		}
		
	} 

}
