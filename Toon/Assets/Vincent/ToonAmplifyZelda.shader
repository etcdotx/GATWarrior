// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "New Amplify Shader"
{
	Properties
	{
		_NormalMap("Normal Map", 2D) = "white" {}
		[Toggle]_UseNormalMap("Use Normal Map", Float) = 1
		_Color0("Color 0", Color) = (0,0.04285717,1,0)
		_SpecularSize("SpecularSize", Float) = 0.5
		_SpecularStrength("Specular Strength", Float) = 0.5
		[Toggle]_UseSpecularDabs("Use Specular Dabs", Float) = 1
		_AlbedoMap("Albedo Map", 2D) = "white" {}
		_Tint("Tint", Color) = (1,1,1,0)
		[Toggle]_ToggleSwitch0("Toggle Switch0", Float) = 0
		_FresnelPower("Fresnel Power", Float) = 4
		_MetalicSmoothnessMap("Metalic/Smoothness Map", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "UnityCG.cginc"
		#include "UnityStandardUtils.cginc"
		#include "UnityShaderVariables.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float3 worldPos;
			float3 viewDir;
			float3 worldNormal;
			INTERNAL_DATA
			float2 uv_texcoord;
		};

		struct SurfaceOutputCustomLightingCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			Input SurfInput;
			UnityGIInput GIData;
		};

		uniform float _ToggleSwitch0;
		uniform float _FresnelPower;
		uniform float _UseNormalMap;
		uniform sampler2D _NormalMap;
		uniform float4 _NormalMap_ST;
		uniform float4 _Color0;
		uniform float _UseSpecularDabs;
		uniform float _SpecularSize;
		uniform float _SpecularStrength;
		uniform sampler2D _MetalicSmoothnessMap;
		uniform float4 _MetalicSmoothnessMap_ST;
		uniform float4 _Tint;
		uniform sampler2D _AlbedoMap;
		uniform float4 _AlbedoMap_ST;

		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			#ifdef UNITY_PASS_FORWARDBASE
			float ase_lightAtten = data.atten;
			if( _LightColor0.a == 0)
			ase_lightAtten = 0;
			#else
			float3 ase_lightAttenRGB = gi.light.color / ( ( _LightColor0.rgb ) + 0.000001 );
			float ase_lightAtten = max( max( ase_lightAttenRGB.r, ase_lightAttenRGB.g ), ase_lightAttenRGB.b );
			#endif
			#if defined(HANDLE_SHADOWS_BLENDING_IN_GI)
			half bakedAtten = UnitySampleBakedOcclusion(data.lightmapUV.xy, data.worldPos);
			float zDist = dot(_WorldSpaceCameraPos - data.worldPos, UNITY_MATRIX_V[2].xyz);
			float fadeDist = UnityComputeShadowFadeDistance(data.worldPos, zDist);
			ase_lightAtten = UnityMixRealtimeAndBakedShadows(data.atten, bakedAtten, UnityComputeShadowFade(fadeDist));
			#endif
			float3 ase_worldPos = i.worldPos;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float3 normalizeResult15 = normalize( ase_worldlightDir );
			float dotResult25 = dot( ( float3( -1,0,0 ) * normalizeResult15 ) , i.viewDir );
			float temp_output_29_0 = ( ( 1.0 - saturate( ( dotResult25 + 0.2 ) ) ) + 0.2 );
			float3 ase_worldNormal = i.worldNormal;
			float3 ase_normWorldNormal = normalize( ase_worldNormal );
			float fresnelNdotV30 = dot( ase_normWorldNormal, i.viewDir );
			float fresnelNode30 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV30, _FresnelPower ) );
			float2 uv_NormalMap = i.uv_texcoord * _NormalMap_ST.xy + _NormalMap_ST.zw;
			float3 temp_output_4_0 = BlendNormals( lerp(tex2D( _NormalMap, uv_NormalMap ),_Color0,_UseNormalMap).rgb , ase_worldNormal );
			float dotResult18 = dot( normalizeResult15 , temp_output_4_0 );
			float temp_output_19_0 = step( 0.7 , ase_lightAtten );
			float smoothstepResult21 = smoothstep( 0.29 , 0.3 , ( dotResult18 * temp_output_19_0 ));
			float3 normalizeResult16 = normalize( ( normalizeResult15 + i.viewDir ) );
			float dotResult17 = dot( normalizeResult16 , temp_output_4_0 );
			float temp_output_38_0 = ( temp_output_19_0 * dotResult17 );
			float smoothstepResult54 = smoothstep( 0.01 , 0.02 , ( temp_output_38_0 + (-1.0 + (_SpecularSize - 0.0) * (-0.9 - -1.0) / (1.0 - 0.0)) ));
			float4 temp_cast_9 = (0.4).xxxx;
			float2 uv_MetalicSmoothnessMap = i.uv_texcoord * _MetalicSmoothnessMap_ST.xy + _MetalicSmoothnessMap_ST.zw;
			float2 uv_AlbedoMap = i.uv_texcoord * _AlbedoMap_ST.xy + _AlbedoMap_ST.zw;
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			float4 temp_output_69_0 = ( saturate( ( step( ( temp_output_29_0 + 0.2 ) , fresnelNode30 ) * smoothstepResult21 ) ) + ( 1.5 * ( ( ( step( temp_output_29_0 , fresnelNode30 ) + (0.2 + (smoothstepResult21 - 0.0) * (0.9 - 0.2) / (1.0 - 0.0)) ) + ( smoothstepResult21 * ( lerp(( smoothstepResult54 * _SpecularStrength ),( _SpecularStrength * step( ( 1.0 - _SpecularSize ) , (0.0 + (temp_output_38_0 - 0.9) * (1.0 - 0.0) / (1.0 - 0.9)) ) ),_UseSpecularDabs) * step( temp_cast_9 , tex2D( _MetalicSmoothnessMap, uv_MetalicSmoothnessMap ) ) ) ) ) * ( ( _Tint * tex2D( _AlbedoMap, uv_AlbedoMap ) ) * float4( ase_lightColor.rgb , 0.0 ) * ase_lightColor.a ) ) ) );
			float4 temp_cast_11 = (0.4).xxxx;
			float4 temp_output_72_0 = lerp(temp_output_69_0,( temp_output_69_0 * unity_AmbientSky ),_ToggleSwitch0);
			c.rgb = temp_output_72_0.rgb;
			c.a = 1;
			return c;
		}

		inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
		{
			s.GIData = data;
		}

		void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
		{
			o.SurfInput = i;
			float3 ase_worldPos = i.worldPos;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float3 normalizeResult15 = normalize( ase_worldlightDir );
			float dotResult25 = dot( ( float3( -1,0,0 ) * normalizeResult15 ) , i.viewDir );
			float temp_output_29_0 = ( ( 1.0 - saturate( ( dotResult25 + 0.2 ) ) ) + 0.2 );
			float3 ase_worldNormal = i.worldNormal;
			float3 ase_normWorldNormal = normalize( ase_worldNormal );
			float fresnelNdotV30 = dot( ase_normWorldNormal, i.viewDir );
			float fresnelNode30 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV30, _FresnelPower ) );
			float2 uv_NormalMap = i.uv_texcoord * _NormalMap_ST.xy + _NormalMap_ST.zw;
			float3 temp_output_4_0 = BlendNormals( lerp(tex2D( _NormalMap, uv_NormalMap ),_Color0,_UseNormalMap).rgb , ase_worldNormal );
			float dotResult18 = dot( normalizeResult15 , temp_output_4_0 );
			float temp_output_19_0 = step( 0.7 , 1 );
			float smoothstepResult21 = smoothstep( 0.29 , 0.3 , ( dotResult18 * temp_output_19_0 ));
			float3 normalizeResult16 = normalize( ( normalizeResult15 + i.viewDir ) );
			float dotResult17 = dot( normalizeResult16 , temp_output_4_0 );
			float temp_output_38_0 = ( temp_output_19_0 * dotResult17 );
			float smoothstepResult54 = smoothstep( 0.01 , 0.02 , ( temp_output_38_0 + (-1.0 + (_SpecularSize - 0.0) * (-0.9 - -1.0) / (1.0 - 0.0)) ));
			float4 temp_cast_2 = (0.4).xxxx;
			float2 uv_MetalicSmoothnessMap = i.uv_texcoord * _MetalicSmoothnessMap_ST.xy + _MetalicSmoothnessMap_ST.zw;
			float2 uv_AlbedoMap = i.uv_texcoord * _AlbedoMap_ST.xy + _AlbedoMap_ST.zw;
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			float4 temp_output_69_0 = ( saturate( ( step( ( temp_output_29_0 + 0.2 ) , fresnelNode30 ) * smoothstepResult21 ) ) + ( 1.5 * ( ( ( step( temp_output_29_0 , fresnelNode30 ) + (0.2 + (smoothstepResult21 - 0.0) * (0.9 - 0.2) / (1.0 - 0.0)) ) + ( smoothstepResult21 * ( lerp(( smoothstepResult54 * _SpecularStrength ),( _SpecularStrength * step( ( 1.0 - _SpecularSize ) , (0.0 + (temp_output_38_0 - 0.9) * (1.0 - 0.0) / (1.0 - 0.9)) ) ),_UseSpecularDabs) * step( temp_cast_2 , tex2D( _MetalicSmoothnessMap, uv_MetalicSmoothnessMap ) ) ) ) ) * ( ( _Tint * tex2D( _AlbedoMap, uv_AlbedoMap ) ) * float4( ase_lightColor.rgb , 0.0 ) * ase_lightColor.a ) ) ) );
			float4 temp_cast_4 = (0.4).xxxx;
			float4 temp_output_72_0 = lerp(temp_output_69_0,( temp_output_69_0 * unity_AmbientSky ),_ToggleSwitch0);
			o.Albedo = temp_output_72_0.rgb;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardCustomLighting keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.viewDir = worldViewDir;
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputCustomLightingCustom o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputCustomLightingCustom, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16800
35;563;1906;1021;-1565.165;388.8325;1.3;True;False
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;12;-1145.886,-264.9896;Float;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.NormalizeNode;15;-808.5646,-239.2621;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;1;-2401.803,142.3398;Float;True;Property;_NormalMap;Normal Map;0;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;3;-2374.668,388.2496;Float;False;Property;_Color0;Color 0;2;0;Create;True;0;0;False;0;0,0.04285717,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;10;-1535.214,-361.6752;Float;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ToggleSwitchNode;2;-2000.668,142.2496;Float;True;Property;_UseNormalMap;Use Normal Map;1;0;Create;True;0;0;False;0;1;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldNormalVector;8;-2009.077,415.7615;Float;True;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;11;-1054.518,381.5569;Float;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NormalizeNode;16;-712.1002,439.4996;Float;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.BlendNormalsNode;4;-1562.303,169.1707;Float;True;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LightAttenuation;13;-1127.892,-81.93512;Float;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-681.3649,-1083.183;Float;True;2;2;0;FLOAT3;-1,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DotProductOpNode;17;-286.9524,356.9596;Float;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;42;239.1313,752.258;Float;False;Property;_SpecularSize;SpecularSize;3;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;19;-112.0389,148.6119;Float;True;2;0;FLOAT;0.7;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;163.3463,444.1759;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;41;403.8243,591.2692;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-1;False;4;FLOAT;-0.9;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;25;-386.4879,-978.7307;Float;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;37;618.7889,322.3492;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;43;240.9559,1104.925;Float;True;5;0;FLOAT;0;False;1;FLOAT;0.9;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;46;756.9838,1053.185;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;55;808.3036,161.9917;Float;False;Constant;_Vector0;Vector 0;6;0;Create;True;0;0;False;0;0.01,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleAddOpNode;26;-147.967,-981.5784;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;57;785.3036,565.9917;Float;False;Constant;_Vector1;Vector 1;6;0;Create;True;0;0;False;0;0.02,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.DotProductOpNode;18;-418.9716,-166.6848;Float;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;50;970.281,924.4204;Float;False;Property;_SpecularStrength;Specular Strength;4;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;27;153.5621,-976.7556;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;76;1525.446,1060.799;Float;True;Property;_MetalicSmoothnessMap;Metalic/Smoothness Map;10;0;Create;True;0;0;False;0;None;None;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SmoothstepOpNode;54;960.3036,350.9917;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;44;931.9304,1089.144;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;16.57256,-89.09355;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;74;89.63074,-1085.792;Float;False;Property;_FresnelPower;Fresnel Power;9;0;Create;True;0;0;False;0;4;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;78;1943.867,882.3125;Float;False;Constant;_Vector3;Vector 3;11;0;Create;True;0;0;False;0;0.4,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;1256.304,558.9917;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;48;1195.188,1068.075;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;28;375.5621,-945.7556;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;75;1860.422,1060.262;Float;True;Property;_TextureSample1;Texture Sample 1;10;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldNormalVector;34;-306.2848,-1300.589;Float;False;True;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.StepOpNode;77;2274.867,1031.313;Float;False;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;51;1574.322,837.239;Float;False;Property;_UseSpecularDabs;Use Specular Dabs;5;0;Create;True;0;0;False;0;1;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;62;1765.564,-427.3987;Float;True;Property;_AlbedoMap;Albedo Map;6;0;Create;True;0;0;False;0;None;None;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.FresnelNode;30;335.8543,-1369.46;Float;True;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;4;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;29;573.1803,-964.6358;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;21;314.0364,-85.73514;Float;True;3;0;FLOAT;0;False;1;FLOAT;0.29;False;2;FLOAT;0.3;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;31;964.5686,-949.0667;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;22;766.0364,-85.73514;Float;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0.2;False;4;FLOAT;0.9;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;79;1892.267,687.6129;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;63;2069.418,-430.4781;Float;True;Property;_TextureSample0;Texture Sample 0;7;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;65;2135.725,-623.2182;Float;False;Property;_Tint;Tint;7;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;32;1017.569,-1292.067;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;64;2472.725,-465.2182;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;52;1297.686,-85.87858;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;59;1965.583,262.6271;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LightColorNode;14;2237.419,-226.7581;Float;True;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.StepOpNode;33;1304.569,-1236.067;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;60;2017.698,-53.1637;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;66;2571.132,-304.6429;Float;True;3;3;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;1987.024,-1129.602;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;68;2889.844,-246.118;Float;False;Constant;_Vector2;Vector 2;8;0;Create;True;0;0;False;0;1.5,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;61;2811.351,-68.07384;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;36;2332.794,-1129.34;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;67;3110.68,-103.6612;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;69;3421.844,-136.118;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FogAndAmbientColorsNode;71;3392.543,173.4216;Float;False;unity_AmbientSky;0;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;70;3655.814,25.45826;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;72;3885.698,-122.4124;Float;True;Property;_ToggleSwitch0;Toggle Switch0;8;0;Create;True;0;0;False;0;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;4358.26,-314.9169;Float;False;True;2;Float;ASEMaterialInspector;0;0;CustomLighting;New Amplify Shader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;15;0;12;0
WireConnection;2;0;1;0
WireConnection;2;1;3;0
WireConnection;11;0;15;0
WireConnection;11;1;10;0
WireConnection;16;0;11;0
WireConnection;4;0;2;0
WireConnection;4;1;8;0
WireConnection;24;1;15;0
WireConnection;17;0;16;0
WireConnection;17;1;4;0
WireConnection;19;1;13;0
WireConnection;38;0;19;0
WireConnection;38;1;17;0
WireConnection;41;0;42;0
WireConnection;25;0;24;0
WireConnection;25;1;10;0
WireConnection;37;0;38;0
WireConnection;37;1;41;0
WireConnection;43;0;38;0
WireConnection;46;0;42;0
WireConnection;26;0;25;0
WireConnection;18;0;15;0
WireConnection;18;1;4;0
WireConnection;27;0;26;0
WireConnection;54;0;37;0
WireConnection;54;1;55;1
WireConnection;54;2;57;1
WireConnection;44;0;46;0
WireConnection;44;1;43;0
WireConnection;20;0;18;0
WireConnection;20;1;19;0
WireConnection;58;0;54;0
WireConnection;58;1;50;0
WireConnection;48;0;50;0
WireConnection;48;1;44;0
WireConnection;28;0;27;0
WireConnection;75;0;76;0
WireConnection;77;0;78;1
WireConnection;77;1;75;0
WireConnection;51;0;58;0
WireConnection;51;1;48;0
WireConnection;30;0;34;0
WireConnection;30;4;10;0
WireConnection;30;3;74;0
WireConnection;29;0;28;0
WireConnection;21;0;20;0
WireConnection;31;0;29;0
WireConnection;31;1;30;0
WireConnection;22;0;21;0
WireConnection;79;0;51;0
WireConnection;79;1;77;0
WireConnection;63;0;62;0
WireConnection;32;0;29;0
WireConnection;64;0;65;0
WireConnection;64;1;63;0
WireConnection;52;0;31;0
WireConnection;52;1;22;0
WireConnection;59;0;21;0
WireConnection;59;1;79;0
WireConnection;33;0;32;0
WireConnection;33;1;30;0
WireConnection;60;0;52;0
WireConnection;60;1;59;0
WireConnection;66;0;64;0
WireConnection;66;1;14;1
WireConnection;66;2;14;2
WireConnection;35;0;33;0
WireConnection;35;1;21;0
WireConnection;61;0;60;0
WireConnection;61;1;66;0
WireConnection;36;0;35;0
WireConnection;67;0;68;1
WireConnection;67;1;61;0
WireConnection;69;0;36;0
WireConnection;69;1;67;0
WireConnection;70;0;69;0
WireConnection;70;1;71;0
WireConnection;72;0;69;0
WireConnection;72;1;70;0
WireConnection;0;0;72;0
WireConnection;0;13;72;0
ASEEND*/
//CHKSM=352DC5D48694777544918863E59A9EC1D941C228