Shader "Custom/Circle" {
    Properties {
        _Color ("Color", Color) = (1, 1, 1, 1)
        _CirclesX("Circles in X", Float) = 0.5
        _CirclesY("Circles in Y", Float) = 0.5
        _Fade("Fade", Range(0.1,1.0)) = 0.5

    }

    SubShader {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass {
            CGPROGRAM
            #include "UnityCG.cginc"

            #pragma vertex vert_img
            #pragma fragment frag
            #pragma target 3.0

            float4 _Color;
            uniform float _CirclesX;
            uniform float _CirclesY;
            uniform float _Fade;
 
            float4 frag(v2f_img i): COLOR {
                fixed4 transparent = float4(1,1,1,0 );
                float distance = length(i.uv - float2(_CirclesX, _CirclesY));
                float delta = fwidth(distance);
                float alpha = smoothstep(_Fade, _Fade - delta, distance);
                return lerp(transparent, _Color, alpha);
            }

            ENDCG
        }
    }
}
