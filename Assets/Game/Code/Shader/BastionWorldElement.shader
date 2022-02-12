Shader "Toon/Lit Distance Appear" 
{
    Properties{
        _Color("Color Primary", Color) = (0.5,0.5,0.5,1)
        _MainTex("Main Texture", 2D) = "white" {}
        
        _Glossiness("Smoothness", Range(0.0, 1.0)) = 0.5
        _GlossMapScale("Smoothness Scale", Range(0.0, 1.0)) = 1.0
        [Enum(Metallic Alpha,0,Albedo Alpha,1)] _SmoothnessTextureChannel("Smoothness texture channel", Float) = 0

        [Gamma] _Metallic("Metallic", Range(0.0, 1.0)) = 0.0
        _MetallicGlossMap("Metallic", 2D) = "white" {}

        [ToggleOff] _SpecularHighlights("Specular Highlights", Float) = 1.0
        [ToggleOff] _GlossyReflections("Glossy Reflections", Float) = 1.0

        _BumpScale("Scale", Float) = 1.0
        [Normal] _BumpMap("Normal Map", 2D) = "bump" {}

        _Parallax("Height Scale", Range(0.005, 0.08)) = 0.02
        _ParallaxMap("Height Map", 2D) = "black" {}

        _OcclusionStrength("Strength", Range(0.0, 1.0)) = 1.0
        _OcclusionMap("Occlusion", 2D) = "white" {}


        // Specific shader stuff
        _Speed("MoveSpeed", Range(1,50)) = 10 // speed of the swaying
        _Min("Min Transparency", Range(0,1)) = 0.5 // speed of the swaying
        _Max("Max Transparency", Range(0,1)) = 1 // speed of the swaying
        [Toggle(DOWN_ON)] _DOWN("Move Down?", Int) = 0
    }

    SubShader{
        Tags {"Queue" = "Geometry" "LightMode" = "ForwardAdd" }
        LOD 200

        Pass
        {
            ZWrite On
            ColorMask 0
        }

        CGPROGRAM
        #pragma surface surf Standard vertex:vert alpha // addshadow applies shadow after vertex animation
        #pragma multi_compile_instancing
        #pragma shader_feature DOWN_ON
        #pragma target 3.0

        sampler2D _MainTex;

        float _Glossiness;
        float _GlossMapScale;
        float _SmoothnessTextureChannel;

        float _Metallic;
        sampler2D _MetallicGlossMap;

        float _SpecularHighlights;
        float _GlossyReflections;

        float _BumpScale;
        sampler2D _BumpMap;

        float _Parallax;
        sampler2D _ParallaxMap;

        float _OcclusionStrength;
        sampler2D _OcclusionMap;

        float _Speed;
        float4 _Color;
        float _Min;
        float _Max;
        struct Input {
            float4 color : COLOR;
            float2 uv_MainTex : TEXCOORD0;
            float2 uv_MetallicGlossMap : TEXCOORD1;
            float2 uv_BumpMap : TEXCOORD2;
            float2 uv_ParallaxMap : TEXCOORD3;
            float2 uv_OcclusionMap : TEXCOORD4;
            float3 viewDir;
        };

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_DEFINE_INSTANCED_PROP(float, _Moved)
        UNITY_INSTANCING_BUFFER_END(Props)

        void vert(inout appdata_full v)//
        {
            #if DOWN_ON
            v.color.a += _Speed - UNITY_ACCESS_INSTANCED_PROP(Props, _Moved * _Speed);
            #else
            v.color.a -= _Speed - UNITY_ACCESS_INSTANCED_PROP(Props, _Moved * _Speed);
            #endif
            v.color.a = clamp(v.color.a, _Min, _Max);
        }

        void surf(Input IN, inout SurfaceOutputStandard o) {
            float2 texOffset = ParallaxOffset(tex2D(_BumpMap, IN.uv_BumpMap).r, _BumpScale, IN.viewDir);
            o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap)) * _BumpScale;

            half4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;

            o.Albedo = c.rgb * IN.color.rgb;
            o.Alpha = c.a * IN.color.a;

            o.Metallic = tex2D(_MetallicGlossMap, IN.uv_MetallicGlossMap).a * _Metallic;
            o.Smoothness = tex2D(_ParallaxMap, IN.uv_ParallaxMap).rgb * _Parallax;
            o.Occlusion = tex2D(_OcclusionMap, IN.uv_OcclusionMap).rgb * _OcclusionStrength;
            //o.Alpha = IN.color.a;
        }
        ENDCG
    }
    Fallback "Diffuse"
}