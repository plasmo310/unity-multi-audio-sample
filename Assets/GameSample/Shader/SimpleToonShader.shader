// 塗りつぶし+背面法によるアウトライン
Shader "Unlit/SimpleToonShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "black" {}
        _RampTex ("Ramp", 2D) = "white" {}
        _MainColor ("Main Color", Color) = (0, 0, 0, 1)
        _OutlineWidth("Outline Width", Float) = 0.04
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        UsePass "Standard/ShadowCaster" // 影を落とす

        // 背面
        Pass
        {
            //Tags{ "LightMode" = "UniversalForward"} // URPでマルチパス描画するのに必要
            Cull Front

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            float _OutlineWidth;

            v2f vert (appdata v)
            {
                v2f o;
                v.vertex += float4(v.normal * _OutlineWidth, 0); // 法線方向に膨張
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return fixed4(1, 1, 1, 1); // 白色で固定
            }
            ENDCG
        }

        // 前面
        Pass
        {
            Cull Back

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv_MainTex : TEXCOORD0;
                float2 uv_RampTex : TEXCOORD1;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
                float2 uv_MainTex : TEXCOORD0;
                float2 uv_RampTex : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _RampTex;
            float4 _RampTex_ST;
            fixed4 _MainColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.uv_MainTex = TRANSFORM_TEX(v.uv_MainTex, _MainTex);
                o.uv_RampTex = TRANSFORM_TEX(v.uv_RampTex, _RampTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // RampMapは使用せず、メインカラーをそのまま出力
                fixed4 col = _MainColor;
                return col;
            }
            ENDCG
        }
    }
}