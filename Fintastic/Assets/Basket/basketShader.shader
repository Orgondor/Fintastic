Shader "Custom/NewSurfaceShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
		_NormalMap ("Normalmap", 2D) = "normal" {}
		_AlphaMap ("Alphamap", 2D) = "alpha" {}
		_RoughnessMap ("Roughnessmap", 2D) = "rough" {}
        _Metallic ("Metallic", Range(0,1)) = 0.0
		_cutOff ("CutOff", Range(0,1)) = 0.2
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
		Cull Off
		
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
		sampler2D _NormalMap;
		sampler2D _AlphaMap;
		sampler2D _RoughnessMap;
        half _Metallic;
        fixed4 _Color;
		float _cutOff;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            o.Alpha = tex2D (_AlphaMap, IN.uv_MainTex).r;
			clip(o.Alpha - _cutOff);

            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
			
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = 1 - (tex2D(_RoughnessMap, IN.uv_MainTex).r);
			o.Normal = UnpackNormal (tex2D (_NormalMap, IN.uv_MainTex));
			
        }
        ENDCG
    }
    FallBack "Diffuse"
}
