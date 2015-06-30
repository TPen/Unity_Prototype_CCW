// Copyright (C) 2015, Felix Kate; All rights reserved
//
// <Class content>
// Shader for the cube object; uses directional lighting as modifier for a pow to change the shadows colors
//


Shader "Custom/Cube" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_BumpMap ("Normal Map", 2D) = "blue" { }
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		
		Pass{
			Tags {"LightMode" = "ForwardBase"}
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			
			#include "UnityCG.cginc"

			fixed3 _Color;
			sampler2D _BumpMap;
			
			fixed4 _BumpMap_ST;

			struct v2f{
				float4 pos : SV_POSITION;
				fixed2 uv : TEXCOORD0;
				fixed3 lightDir : TEXCOORD1;
			};
			
			v2f vert(appdata_tan v){
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX (v.texcoord, _BumpMap);
				
				TANGENT_SPACE_ROTATION;
				o.lightDir = mul(rotation, ObjSpaceLightDir(v.vertex));
				
				return o;
			}

			fixed4 frag(v2f i) : SV_Target {
				i.lightDir = normalize(i.lightDir);
				
				fixed3 norm = UnpackNormal(tex2D(_BumpMap, i.uv));
				
				fixed light = saturate(dot(i.lightDir, norm));
				
				fixed4 c;
				
				c.rgb = lerp(pow(_Color, 2.5), _Color, light) * 1.5;
				
				c.a = 1;

				return c;
			}
			
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
