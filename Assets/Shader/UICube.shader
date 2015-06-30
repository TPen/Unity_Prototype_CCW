// Copyright (C) 2015, Felix Kate; All rights reserved
//
// <Class content>
// Unitys sprite shader changed so that the optic matches the in game optic (Uses ZTest Off else GUI materials would be behind the background layer?)
//

Shader "Custom/UICube" {
	Properties {
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
	}	
	SubShader {
		Tags { "RenderType"="Background" "Queue"="Background" "IgnoreProjector"="True" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }

		Cull Off
		Lighting Off
		ZTest Off
		Blend One OneMinusSrcAlpha
		
		Pass{
			CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile DUMMY PIXELSNAP_ON
            #include "UnityCG.cginc"

            sampler2D _MainTex;

            struct appdata_t{
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f{
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                half2 texcoord  : TEXCOORD0;
            };

            v2f vert(appdata_t v){
                v2f o;
                o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
                o.texcoord = v.texcoord;
                o.color = v.color;
                #ifdef PIXELSNAP_ON
                o.vertex = UnityPixelSnap (o.vertex);
                #endif

                return o;
            }

            fixed4 frag(v2f i) : SV_Target{
                fixed4 c = tex2D(_MainTex, i.texcoord);
                c.rgb = lerp(pow(i.color.rgb, 2.5), i.color.rgb, c.r) * 1.5;
                c.rgb *= c.a;
                return c;
            }
        ENDCG
       	}
	} 
}
