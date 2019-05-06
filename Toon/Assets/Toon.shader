Shader "Unlit/Toon"
{
    Properties
    {
        _Color("Color",Color) = (0,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
		_Ramp ("Color Ramp", 2D) = "white" {}
		[HDR]
		_AmbientColor("Ambient Color",Color) = (0.4,0.4,0.4,1)
		[HDR]
		_SpecularColor("Specular Color",Color) = (0.9,0.9,0.9,1)
		_Glossiness("Glossines", Float) = 32
		[HDR]
		_RimColor("Rim Color", Color) = (1,1,1,1)
		_RimAmount("Rim Amount", Range(0, 1)) = 0.716
		_RimThreshold("Rim Threshold", Range(0,1)) = 0.1
    }
    SubShader
    {
        Tags {  
			"LightMode" = "ForwardBase"
			"PassFlags" = "OnlyDirectional"
		}
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog
			#pragma multi_compile_fwdbase

			#include "Lighting.cginc"
            #include "UnityCG.cginc"
			#include "AutoLight.cginc"

            struct appdata
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 pos : SV_POSITION;
				float3 worldNormal : NORMAL;
				SHADOW_COORDS(3)
				float3 viewDir : TEXCOORD2;
            };

            sampler2D _MainTex;
			sampler2D _Ramp;
			float _Glossiness;
			float _RimAmount;
			float _RimThreshold;
			float4 _SpecularColor;
            float4 _MainTex_ST;
			float4 _Color;
			float4 _AmbientColor;
			float4 _RimColor;
			

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.pos);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
                UNITY_TRANSFER_FOG(o,o.pos);
				o.viewDir = WorldSpaceViewDir(v.pos);
				TRANSFER_SHADOW(o);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				
				float3 shadow = SHADOW_ATTENUATION(i);
				float3 normal = normalize(i.worldNormal);
				float NdotL = dot(_WorldSpaceLightPos0,normal);
				float lightIntensity = smoothstep(0,0.4,NdotL) * shadow;
				float2 uv = float2(1 - (smoothstep(0,0.4,NdotL)  * 0.45 + 0.5) *-1, 0.5) ;
				//float4 light = lightIntensity * _LightColor0;

				float3 viewDir = normalize(i.viewDir);

				float3 halfVector = normalize(_WorldSpaceLightPos0 + viewDir);
				float NdotH = dot(normal,halfVector);

				float specularIntensity = pow(NdotH *lightIntensity,_Glossiness*_Glossiness);
				float specularIntensitySmooth = smoothstep(0.005,0.01, specularIntensity); 
				float4 specular = specularIntensitySmooth * _SpecularColor;

				float4 rimDot = 1 - dot(viewDir, normal);
				float rimIntensity = rimDot *pow(NdotL,_RimThreshold);
				rimIntensity = smoothstep(_RimAmount-0.01,_RimAmount+0.01,rimIntensity);
				float4 rim = rimIntensity * _RimColor;	

                fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 ramp = tex2D(_Ramp,uv);
				ramp.rgb *=lightIntensity;	
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);

                return col * _Color * (_AmbientColor + ( ramp * _LightColor0 ) +specular + rim);
            }
            ENDCG
        }
		UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}
