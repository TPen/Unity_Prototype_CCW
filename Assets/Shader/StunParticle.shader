// Copyright (C) 2015, Felix Kate; All rights reserved
//
// <Class content>
// Generate a ring that becomes thinner with decrease of the alpha
//


Shader "Custom/StunParticle" {
	Properties {

	}
	SubShader {
		Tags { "RenderType"="Transparent" "Quene"="Transparent" }
		
		Lighting Off
		
		Alphatest Equal 0
		AlphaToMask True
        ColorMask RGB
		
		Blend SrcAlpha OneMinusSrcAlpha
		
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
				o.uv = v.texcoord;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target {
				fixed cirGrad = distance(i.uv, fixed2(0.5, 0.5));
				
				fixed4 c = i.color;
				
				c.a *= (1 - floor(cirGrad + 0.5)) - (1 - floor(cirGrad + 0.5 + 0.2 * c.a));
			
				return c;
			}
			ENDCG
		}
		
	} 

}
