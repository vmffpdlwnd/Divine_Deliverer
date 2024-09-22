Shader "Custom/SeeThrough" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _SeeThroughColor ("See-through Color", Color) = (1,1,1,0.5)
        _EffectDistance ("Effect Distance", Float) = 2.0
    }
    SubShader {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        LOD 100

        CGPROGRAM
        #pragma surface surf Lambert alpha:fade

        sampler2D _MainTex;
        fixed4 _Color;
        fixed4 _SeeThroughColor;
        float _EffectDistance;

        struct Input {
            float2 uv_MainTex;
            float3 worldPos;
            float3 viewDir;
        };

        void surf (Input IN, inout SurfaceOutput o) {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            
            float3 viewToCam = _WorldSpaceCameraPos - IN.worldPos;
            float viewDist = length(viewToCam);
            float3 viewDir = viewToCam / viewDist;
            
            float effect = dot(viewDir, IN.viewDir);
            float alpha = saturate((viewDist - _EffectDistance) / _EffectDistance * effect);
            
            o.Alpha = lerp(_SeeThroughColor.a, c.a, alpha);
        }
        ENDCG
    }
    FallBack "Diffuse"
}