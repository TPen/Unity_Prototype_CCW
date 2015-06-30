// Copyright (C) 2015, Felix Kate; All rights reserved
//
// <Class content>
// Simple shader that inverts the input from the background
//


Shader "Custom/InvertedText" {
     Properties { 
        _MainTex ("Font Texture", 2D) = "white" {} 
     } 
     
     SubShader { 
        Tags { "Queue"="Transparent" "RenderType"="Transparent" } 
        Lighting Off
        Cull Off
        ZWrite Off
        ZTest Off
        
        Blend OneMinusDstColor OneMinusSrcAlpha
        
        Pass{ 
          	CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			
			#include "UnityCG.cginc"
			
			sampler2D _MainTex;
			
			fixed4 _MainTex_ST;
			
			struct v2f{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};
			
			v2f vert(appdata_base v){
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target {
				fixed4 c;
				c.rgb = 1;
				c.a = tex2D(_MainTex, i.uv).a;
				
				c.rgb *= c.a;
				
				return c;
			}
			ENDCG			
        } 
	} 
}
