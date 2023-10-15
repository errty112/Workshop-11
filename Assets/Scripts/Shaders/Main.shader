Shader "CubeInvaders/Main"
{
	Properties
	{
		_Color ("Color", Color) = (1, 1, 1, 1)
		_PointLightColor("Point Light Color", Color) = (1, 1, 1)
		_PointLightPosition("Point Light Position", Vector) = (0.0, 0.0, 0.0)
		_Ka("Ka", Float) = 1.0
		_Kd("Kd", Float) = 1.0
		_Ks("Ks", Float) = 1.0
		_fAtt("fAtt", Float) = 1.0
		_specN("specN", Float) = 1.0
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "Shockwave.cginc"

			uniform float4 _Color;
			uniform float3 _PointLightColor;
			uniform float3 _PointLightPosition;
			uniform float _Ka;
			uniform float _Kd;
			uniform float _Ks;
			uniform float _fAtt;
			uniform float _specN;

			struct vertIn
			{
				float4 vertex : POSITION;
				float4 normal : NORMAL;
			};

			struct vertOut
			{
				float4 vertex : SV_POSITION;
				float4 worldVertex : TEXCOORD0;
				float3 worldNormal : TEXCOORD1;
			};

			// Implementation of the vertex shader
			vertOut vert(vertIn v)
			{
				vertOut o;

				// Calculate the world-space vertex position as before, but also
				// apply the shockwave displacement to the position. We won't 
				// worry about the normal at this stage.
				float4 worldVertex = ComputeShockwaveDisplacement(mul(unity_ObjectToWorld, v.vertex));
				float3 worldNormal = normalize(mul(transpose((float3x3)unity_WorldToObject), v.normal.xyz));

				o.vertex = mul(UNITY_MATRIX_VP, worldVertex);

				o.worldVertex = worldVertex;
				o.worldNormal = worldNormal;

				return o;
			}
			
			// Implementation of the fragment (pixel) shader
			fixed4 frag(vertOut v) : SV_Target
			{
				float3 interpNormal = normalize(v.worldNormal);
				float4 baseColor = _Color;

				// Calculate ambient RGB intensities
				float Ka = _Ka;
				float3 amb = baseColor.rgb * UNITY_LIGHTMODEL_AMBIENT.rgb * Ka;

				// Calculate diffuse RBG reflections
				float fAtt = _fAtt;
				float Kd = _Kd;
				float3 L = normalize(_PointLightPosition - v.worldVertex.xyz);
				float LdotN = dot(L, interpNormal);
				float3 dif = fAtt * _PointLightColor.rgb * Kd * baseColor.rgb * saturate(LdotN);

				// Calculate specular reflections
				float Ks = _Ks;
				float specN = _specN; // Values>>1 give tighter highlights
				float3 V = normalize(_WorldSpaceCameraPos - v.worldVertex.xyz);

				// Using Blinn-Phong approximation
				specN = _specN;
				float3 H = normalize(V + L);
				float3 spe = fAtt * _PointLightColor.rgb * Ks * pow(saturate(dot(interpNormal, H)), specN);

				// Combine illumination model components
				float4 returnColor = float4(0.0f, 0.0f, 0.0f, 0.0f);
				returnColor.rgb = amb.rgb + dif.rgb + spe.rgb;
				returnColor.a = baseColor.a;

				return returnColor;
			}
			ENDCG
		}
	}
}
