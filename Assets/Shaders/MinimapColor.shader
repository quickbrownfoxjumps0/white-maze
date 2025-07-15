Shader "Hidden/MinimapFlatColor"
{
    Properties
    {
        _Color ("Color", Color) = (0,0,0,1)
    }
    SubShader
    {
        Tags { "Queue"="Overlay" }
        LOD 100

        Pass
        {
            ZWrite On
            ZTest LEqual
            Cull Off
            Color [_Color]
        }
    }
}

