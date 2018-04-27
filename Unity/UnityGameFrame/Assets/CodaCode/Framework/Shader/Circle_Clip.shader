
Shader "CodaShader/Circle_Clip"
{
	Properties
	{
		_MainTex ("Main Texture", 2D) = "black" {}
		_AlphaClip ("Alpha Clip Rate", Range(0, 1)) = 1
		_OriginPoint ("Origin Point", Range(0,1)) = 0
		_CutRate ("Cut Rate", Range(0,1)) = 0.5
	}

	SubShader
	{
		LOD 200
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }

		Fog { Mode Off }
		Cull Off
		ZWrite Off
		Blend One OneMinusSrcAlpha
		Lighting Off
		
		Pass
		{

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			fixed _AlphaClip;
			fixed _OriginPoint;
			fixed _CutRate;
			float _TestOffset;

			struct appdata
			{
				float4 vertex : POSITION;
				half4 color : COLOR;
				float4 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : POSITION;
				half4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = v.color;
				o.texcoord = v.texcoord;
				return o;
			}

			fixed4 frag (v2f i) : COLOR
			{
				float middleCoordPos = 0.5;
				float2 originVector = float2(0.0, -1.0);

				fixed4 col;
				col.rgba = tex2D(_MainTex, i.texcoord).rgba;
				col.rgb *= col.a <= _AlphaClip ? col.a : 1;

				float2 inTexCoord = i.texcoord - middleCoordPos;
				float xPos = step(inTexCoord.x, 0);
				float angle = acos(dot(normalize(inTexCoord), originVector)) / (2 * UNITY_PI);
				float angleFromOrigin = xPos == 0 ? 1 - angle : angle;		
				angleFromOrigin = angleFromOrigin < _OriginPoint ? angleFromOrigin + (1 - _OriginPoint) : angleFromOrigin - _OriginPoint;		
				col.rgba *= angleFromOrigin < _CutRate ? 1 : 0;
				return col;
			}
			ENDCG
		}
	}
	
	SubShader
	{
		LOD 100

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		
		Pass
		{
			Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha
			AlphaTest Greater .01
			ColorMaterial AmbientAndDiffuse
			
			SetTexture [_MainTex]
			{
				Combine Texture * Primary
			}
		}
	}
}
