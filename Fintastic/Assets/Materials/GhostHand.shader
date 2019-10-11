// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/TestUnlit"
{
	Properties
	{
		_EdgeFeather("Edge Feather", Float) = 0.5
		_TintColor("Tint Color", Color) = (1,1,1,1)
	}

    SubShader
    {
        Tags {"Queue" = "Transparent" "RenderType"="Transparent" }
        LOD 100
		
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

			float _EdgeFeather;
			float4 _TintColor;

			struct v2f {
				// we'll output world space normal as one of regular ("texcoord") interpolators
				float3 worldPos : TEXCOORD0;
				half3 worldNormal : TEXCOORD1;
				float4 pos : SV_POSITION;
			};

			// vertex shader: takes object space normal as input too
			v2f vert(float4 vertex : POSITION, float3 normal : NORMAL)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(vertex);
				o.worldPos = mul(unity_ObjectToWorld, vertex).xyz;
				o.worldNormal = UnityObjectToWorldNormal(normal);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 c = 0;
				half3 worldViewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));
				half dN = clamp(-(dot(worldViewDir, i.worldNormal)-_EdgeFeather), 0., 1.);
				//c.rgb = (i.worldNormal * 0.5 + 0.5);
				c.rgb = _TintColor.rgb;
				c.a = dN;
				return c;
			}
            ENDCG
        }
    }
}
