// Upgrade NOTE: upgraded instancing buffer 'Cloud' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Cloud"
{
	Properties
	{
		_Texture0("Texture 0", 2D) = "white" {}
		_CloudSpeed("Cloud Speed", Float) = 0.5
		_TextureSample2("Texture Sample 2", 2D) = "white" {}
		_Noise2Scale("Noise 2 Scale", Float) = 1
		_NoiseMasterScale("Noise Master Scale", Float) = 1
		_Noise1Scale("Noise 1 Scale", Float) = 0.5
		_CloudCutoff("Cloud Cutoff", Range( 0 , 1)) = 1
		_CloudSoftness("Cloud Softness", Range( 0 , 3)) = 0.03529414
		_midYValue("midYValue", Float) = 0
		_cloudHeight("cloudHeight", Float) = 0
		_TaperPower("Taper Power", Float) = 1
		_SSSPower("SSS Power", Range( 1 , 50)) = 3.209959
		_SSSStrength("SSS Strength", Float) = 1
		_Ground("Ground", Color) = (0,0,0,0)
		_Sky("Sky", Color) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma multi_compile_instancing
		struct Input
		{
			float3 worldPos;
			float3 viewDir;
			float2 uv_texcoord;
		};

		uniform sampler2D _Texture0;
		uniform sampler2D _TextureSample2;

		UNITY_INSTANCING_BUFFER_START(Cloud)
			UNITY_DEFINE_INSTANCED_PROP(float4, _Ground)
#define _Ground_arr Cloud
			UNITY_DEFINE_INSTANCED_PROP(float4, _Sky)
#define _Sky_arr Cloud
			UNITY_DEFINE_INSTANCED_PROP(float4, _TextureSample2_ST)
#define _TextureSample2_ST_arr Cloud
			UNITY_DEFINE_INSTANCED_PROP(float, _midYValue)
#define _midYValue_arr Cloud
			UNITY_DEFINE_INSTANCED_PROP(float, _cloudHeight)
#define _cloudHeight_arr Cloud
			UNITY_DEFINE_INSTANCED_PROP(float, _TaperPower)
#define _TaperPower_arr Cloud
			UNITY_DEFINE_INSTANCED_PROP(float, _CloudSpeed)
#define _CloudSpeed_arr Cloud
			UNITY_DEFINE_INSTANCED_PROP(float, _Noise1Scale)
#define _Noise1Scale_arr Cloud
			UNITY_DEFINE_INSTANCED_PROP(float, _NoiseMasterScale)
#define _NoiseMasterScale_arr Cloud
			UNITY_DEFINE_INSTANCED_PROP(float, _Noise2Scale)
#define _Noise2Scale_arr Cloud
			UNITY_DEFINE_INSTANCED_PROP(float, _SSSPower)
#define _SSSPower_arr Cloud
			UNITY_DEFINE_INSTANCED_PROP(float, _SSSStrength)
#define _SSSStrength_arr Cloud
			UNITY_DEFINE_INSTANCED_PROP(float, _CloudCutoff)
#define _CloudCutoff_arr Cloud
			UNITY_DEFINE_INSTANCED_PROP(float, _CloudSoftness)
#define _CloudSoftness_arr Cloud
		UNITY_INSTANCING_BUFFER_END(Cloud)

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float _midYValue_Instance = UNITY_ACCESS_INSTANCED_PROP(_midYValue_arr, _midYValue);
			float3 ase_worldPos = i.worldPos;
			float _cloudHeight_Instance = UNITY_ACCESS_INSTANCED_PROP(_cloudHeight_arr, _cloudHeight);
			float _TaperPower_Instance = UNITY_ACCESS_INSTANCED_PROP(_TaperPower_arr, _TaperPower);
			float temp_output_38_0 = ( 1.0 - pow( saturate( ( abs( ( _midYValue_Instance - ase_worldPos.y ) ) / ( _cloudHeight_Instance * 0.35 ) ) ) , _TaperPower_Instance ) );
			float2 appendResult7 = (float2(ase_worldPos.x , ase_worldPos.z));
			float _CloudSpeed_Instance = UNITY_ACCESS_INSTANCED_PROP(_CloudSpeed_arr, _CloudSpeed);
			float mulTime5 = _Time.y * _CloudSpeed_Instance;
			float2 appendResult8 = (float2(mulTime5 , mulTime5));
			float _Noise1Scale_Instance = UNITY_ACCESS_INSTANCED_PROP(_Noise1Scale_arr, _Noise1Scale);
			float _NoiseMasterScale_Instance = UNITY_ACCESS_INSTANCED_PROP(_NoiseMasterScale_arr, _NoiseMasterScale);
			float _Noise2Scale_Instance = UNITY_ACCESS_INSTANCED_PROP(_Noise2Scale_arr, _Noise2Scale);
			float temp_output_16_0 = ( tex2D( _Texture0, ( ( appendResult7 + appendResult8 ) * _Noise1Scale_Instance * _NoiseMasterScale_Instance ) ).r * tex2D( _Texture0, ( ( appendResult7 - appendResult8 ) * _Noise2Scale_Instance * _NoiseMasterScale_Instance ) ).r );
			float temp_output_42_0 = saturate( ( temp_output_38_0 - ( temp_output_16_0 * temp_output_38_0 ) ) );
			float4 _Ground_Instance = UNITY_ACCESS_INSTANCED_PROP(_Ground_arr, _Ground);
			float4 _Sky_Instance = UNITY_ACCESS_INSTANCED_PROP(_Sky_arr, _Sky);
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float dotResult50 = dot( i.viewDir , -ase_worldlightDir );
			float _SSSPower_Instance = UNITY_ACCESS_INSTANCED_PROP(_SSSPower_arr, _SSSPower);
			float _SSSStrength_Instance = UNITY_ACCESS_INSTANCED_PROP(_SSSStrength_arr, _SSSStrength);
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			o.Emission = ( ( temp_output_42_0 * _Ground_Instance ) + ( ( 1.0 - temp_output_42_0 ) * _Sky_Instance ) + ( pow( saturate( dotResult50 ) , _SSSPower_Instance ) * _SSSStrength_Instance * ase_lightColor ) ).rgb;
			float4 _TextureSample2_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_TextureSample2_ST_arr, _TextureSample2_ST);
			float2 uv_TextureSample2 = i.uv_texcoord * _TextureSample2_ST_Instance.xy + _TextureSample2_ST_Instance.zw;
			float _CloudCutoff_Instance = UNITY_ACCESS_INSTANCED_PROP(_CloudCutoff_arr, _CloudCutoff);
			float _CloudSoftness_Instance = UNITY_ACCESS_INSTANCED_PROP(_CloudSoftness_arr, _CloudSoftness);
			o.Alpha = pow( saturate( (0.0 + (( temp_output_16_0 * tex2D( _TextureSample2, uv_TextureSample2 ).r * temp_output_38_0 ) - _CloudCutoff_Instance) * (1.0 - 0.0) / (1.0 - _CloudCutoff_Instance)) ) , _CloudSoftness_Instance );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

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
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
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
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
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
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.viewDir = worldViewDir;
				surfIN.worldPos = worldPos;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
277;298;1636;721;-594.0031;-843.5018;1;True;False
Node;AmplifyShaderEditor.RangedFloatNode;6;-1931.314,978.3385;Float;False;InstancedProperty;_CloudSpeed;Cloud Speed;1;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;25;-686.603,1281.236;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;24;-685.33,1080.213;Float;False;InstancedProperty;_midYValue;midYValue;9;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-360.603,1329.236;Float;False;InstancedProperty;_cloudHeight;cloudHeight;10;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;26;-442.603,1174.236;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;5;-1598.112,974.219;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;9;-1466.283,685.7739;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;-178.603,1333.236;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0.35;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;7;-1243.363,81.00072;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.AbsOpNode;27;-293.603,1175.236;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;8;-1370.141,950.0215;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;59;-915.2592,491.678;Float;False;InstancedProperty;_NoiseMasterScale;Noise Master Scale;5;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-884.1902,224.2564;Float;False;InstancedProperty;_Noise2Scale;Noise 2 Scale;4;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;11;-810.3743,902.825;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-850.3718,706.1733;Float;False;InstancedProperty;_Noise1Scale;Noise 1 Scale;6;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;10;-955.3636,81.00072;Float;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;28;-83.60303,1175.236;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-565.2913,705.5813;Float;False;3;3;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-688.7112,155.942;Float;False;3;3;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;34;82.39697,1304.236;Float;False;InstancedProperty;_TaperPower;Taper Power;11;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;32;93.39697,1174.236;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;2;-686.2084,366.0753;Float;True;Property;_Texture0;Texture 0;0;0;Create;True;0;0;False;0;e28dc97a9541e3642a48c0e3886688c5;e28dc97a9541e3642a48c0e3886688c5;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SamplerNode;4;-320.9827,664.3529;Float;True;Property;_TextureSample1;Texture Sample 1;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;3;-294.603,255.6272;Float;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;33;258.397,1173.236;Float;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;144.2139,533.4023;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;38;459.5351,1171.892;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;51;541.634,1815.394;Float;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;49;603.5762,1559.269;Float;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;933.2289,916.3377;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NegateNode;52;813.179,1794.138;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DotProductOpNode;50;965.403,1596.832;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;40;959.8609,1053.307;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;35;9.330694,939.6714;Float;True;Property;_TextureSample2;Texture Sample 2;3;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;18;585.7196,748.7103;Float;False;InstancedProperty;_CloudCutoff;Cloud Cutoff;7;0;Create;True;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;55;1102.173,1773.313;Float;False;InstancedProperty;_SSSPower;SSS Power;12;0;Create;True;0;0;False;0;3.209959;0;1;50;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;493.2648,580.9543;Float;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;42;1191.601,1008.826;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;53;1146.856,1580.905;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;57;1435.329,1668.807;Float;False;InstancedProperty;_SSSStrength;SSS Strength;13;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;17;857.7197,540.7103;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;54;1391.791,1536.162;Float;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;61;1313.003,790.5018;Float;False;InstancedProperty;_Ground;Ground;14;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LightColorNode;58;1426.065,1397.497;Float;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.OneMinusNode;41;1386.734,1039.255;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;62;1182.003,1190.502;Float;False;InstancedProperty;_Sky;Sky;15;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;19;1065.72,540.7103;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;22;1049.72,684.7103;Float;False;InstancedProperty;_CloudSoftness;Cloud Softness;8;0;Create;True;0;0;False;0;0.03529414;0;0;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;1594.202,874.626;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;1671.845,1437.173;Float;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;1583.601,1052.827;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;45;1938.159,804.1552;Float;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;20;1323.287,571.1447;Float;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2220.381,378.3659;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Cloud;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;TransparentCutout;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;2;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;26;0;24;0
WireConnection;26;1;25;2
WireConnection;5;0;6;0
WireConnection;29;0;31;0
WireConnection;7;0;9;1
WireConnection;7;1;9;3
WireConnection;27;0;26;0
WireConnection;8;0;5;0
WireConnection;8;1;5;0
WireConnection;11;0;7;0
WireConnection;11;1;8;0
WireConnection;10;0;7;0
WireConnection;10;1;8;0
WireConnection;28;0;27;0
WireConnection;28;1;29;0
WireConnection;13;0;11;0
WireConnection;13;1;14;0
WireConnection;13;2;59;0
WireConnection;12;0;10;0
WireConnection;12;1;15;0
WireConnection;12;2;59;0
WireConnection;32;0;28;0
WireConnection;4;0;2;0
WireConnection;4;1;13;0
WireConnection;3;0;2;0
WireConnection;3;1;12;0
WireConnection;33;0;32;0
WireConnection;33;1;34;0
WireConnection;16;0;4;1
WireConnection;16;1;3;1
WireConnection;38;0;33;0
WireConnection;39;0;16;0
WireConnection;39;1;38;0
WireConnection;52;0;51;0
WireConnection;50;0;49;0
WireConnection;50;1;52;0
WireConnection;40;0;38;0
WireConnection;40;1;39;0
WireConnection;36;0;16;0
WireConnection;36;1;35;1
WireConnection;36;2;38;0
WireConnection;42;0;40;0
WireConnection;53;0;50;0
WireConnection;17;0;36;0
WireConnection;17;1;18;0
WireConnection;54;0;53;0
WireConnection;54;1;55;0
WireConnection;41;0;42;0
WireConnection;19;0;17;0
WireConnection;43;0;42;0
WireConnection;43;1;61;0
WireConnection;56;0;54;0
WireConnection;56;1;57;0
WireConnection;56;2;58;0
WireConnection;44;0;41;0
WireConnection;44;1;62;0
WireConnection;45;0;43;0
WireConnection;45;1;44;0
WireConnection;45;2;56;0
WireConnection;20;0;19;0
WireConnection;20;1;22;0
WireConnection;0;2;45;0
WireConnection;0;9;20;0
ASEEND*/
//CHKSM=FAF4E4144C5394FAE105FA54EADAD2E46343EE79