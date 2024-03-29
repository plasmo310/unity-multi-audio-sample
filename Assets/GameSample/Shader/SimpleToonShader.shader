// 塗りつぶし+背面法によるアウトライン
Shader "Unlit/SimpleToonShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "black" {}
        _RampTex ("Ramp", 2D) = "white" {}
        _OutlineWidth("Outline Width", Float) = 0.04
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        UsePass "Standard/ShadowCaster" // 影を落とす

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
                // RampMapから取り出して乗算
                const half nl = dot(i.normal, _WorldSpaceLightPos0.xyz) * 0.5 + 0.5;
                const fixed3 ramp = tex2D(_RampTex, fixed2(nl, 0.5)).rgb;
                fixed4 col = tex2D(_MainTex, i.uv_MainTex);
                col.rgb *= ramp;
                return col;
            }
            ENDCG
        }
    }
}