// Copyright (C) 2015, Felix Kate; All rights reserved
//
// <Class content>
// Small fake shadow plane below the chest objects
//


Shader "Custom/ChestShadow" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
	}
	SubShader {
		Tags { "RenderType"="Transparent" "Quene"="Transparent" }
		
		Lighting Off
		
		Blend DstColor Zero
		
		Pass{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			
			#include "UnityCG.cginc"
			
			fixed4 _Color;
			
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
				fixed cirGrad = distance(i.uv, fixed2(0.5, 0.5));
			
				return lerp(_Color, 1, smoothstep(0.25, 0.5, cirGrad));
			}
			ENDCG
		}
		
	} 

}