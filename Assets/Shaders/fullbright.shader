// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/UnlitHardLight"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            fixed4 _Color;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 normal : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                // Transform normal to world space
                o.normal = normalize(mul((float3x3)unity_ObjectToWorld, v.normal));
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Fixed directional light direction (normalized)
                float3 lightDir = normalize(float3(0.5, 0.5, -1));
                float NdotL = max(dot(i.normal, lightDir), 0);

                // Hard light: strong contrast
                // You can tweak the power here for harder light
                float intensity = pow(NdotL, 8); // increase exponent for harder light

                return fixed4(_Color.rgb * intensity, 1);
            }
            ENDCG
        }
    }
}

