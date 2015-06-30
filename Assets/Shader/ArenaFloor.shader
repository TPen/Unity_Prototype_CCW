// Copyright (C) 2015, Felix Kate; All rights reserved
//
// <Class content>
// Shader for the arena floor; second uv set stretches over the whole floor
//


Shader "Custom/ArenaFloor" {
	Properties {
		_MainTex ("Texture", 2D) = "white" { }
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		
		Lighting Off
		
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
	         	float2 texcoord2 : TEXCOORD1;
	        };

			struct v2f{
				float4 pos : SV_POSITION;
				fixed4 color : COLOR;
				float2 uv1 : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
			};
			
			v2f vert(vertInput v){
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.color = v.color;
				o.uv1 = TRANSFORM_TEX (v.texcoord, _MainTex);
				o.uv2 = TRANSFORM_TEX (v.texcoord2, _MainTex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target {
				fixed4 c = tex2D(_MainTex, i.uv2);

				c.rgb = lerp(i.color.rgb * (1 + i.color.a * 2), c.rgb, c.a);

				c.a = 1;

				return c;
			}
			ENDCG
		}
		
	} 

}
