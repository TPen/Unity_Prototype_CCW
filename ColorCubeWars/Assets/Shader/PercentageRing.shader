// Copyright (C) 2015, Felix Kate; All rights reserved
//
// <Class content>
// Ring showing the percent of how many percent of the arena are colored by the different players
//

Shader "Custom/Percentage" {
	Properties {
		_Col0 ("Color", Color) = (1,1,1,0)
		_Col1 ("Color", Color) = (1,1,1,0)
		_Col2 ("Color", Color) = (1,1,1,0)
		_Col3 ("Color", Color) = (1,1,1,0)
		_Outline ("Outline", Float) = 1
	}
	SubShader {
		Tags { "RenderType"="Background" "Queue"="Background" "RenderType"="Opaque" "PreviewType"="Plane" }
		
		Lighting Off
		ZTest Off
		
		Blend SrcAlpha OneMinusSrcAlpha
		
		Pass{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			
			#include "UnityCG.cginc"
			
			fixed4 _Col0;
			fixed4 _Col1;
			fixed4 _Col2;
			fixed4 _Col3;
			fixed _Outline;
			
			//Function to create a slightly blured circle
			inline fixed drawCircle(fixed size, fixed grad){
				return 1 - smoothstep(size / 2 - 0.005, size / 2 + 0.005, grad);
			}
			
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
				//Make an angular and circle gradient
				fixed PI = 3.14159265359;
				fixed angGrad = (atan2(i.uv.x - 0.5, 1 - i.uv.y - 0.5) + PI) / (2 * PI);
				fixed cirGrad = distance(i.uv, fixed2(0.5, 0.5));

				fixed4 c;

				fixed percentage = clamp(_Col0.a + _Col1.a + _Col2.a + _Col3.a, 0, 1);

				//Inner Circle
				fixed3 colCir = 0.2;

				colCir = lerp(colCir, _Col3.rgb, floor(angGrad + percentage));
				percentage -= _Col3.a;
				
				colCir = lerp(colCir, _Col2.rgb, floor(angGrad + percentage));
				percentage -= _Col2.a;
				
				colCir = lerp(colCir, _Col1.rgb, floor(angGrad + percentage));
				percentage -= _Col1.a;
				
				colCir = lerp(colCir, _Col0.rgb, floor(angGrad + percentage));

				c.rgb = lerp(_Outline, colCir, drawCircle(0.95, cirGrad) - drawCircle(0.7, cirGrad));

				c.a = drawCircle(1, cirGrad) - drawCircle(0.65, cirGrad);
				
				return c;
			}
			ENDCG
		}
		
	} 

}