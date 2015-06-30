// Copyright (C) 2015, Felix Kate; All rights reserved
//
// <Class content>
// Simple gradient with a small sinus wave that changes colors
//


Shader "Custom/Background" {
	Properties {
		_Color ("Color", Color) = (0.5,0.5,0.5,1)
	}
	SubShader {
		Tags { "RenderType"="Opaque" "Queue"="Background" "PreviewType"="Plane" }
		
		Lighting Off
		
		Blend SrcAlpha OneMinusSrcAlpha
		
		Pass{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			
			#include "UnityCG.cginc"
			
			fixed3 _Color;
			
			struct v2f{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};
			
			v2f vert(appdata_full v){
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.texcoord;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target {
				fixed4 c;
				
				fixed wave = sin(i.uv.x * 2.5 + _Time.y) * 0.1 + i.uv.y;
				wave = smoothstep(0.1, 0.5, wave) * (1 - smoothstep(0.5, 0.9, wave));
				
				c.rgb = clamp(lerp(_Color.rgb, pow(_Color.rgb, 2), 1 - i.uv.y), 0.25, 1);
				c.rgb = pow(c.rgb, 1 - wave * 0.25) * 0.75;

				c.a = 1;
				
				return c;
			}
			ENDCG
		}
		
	} 

}