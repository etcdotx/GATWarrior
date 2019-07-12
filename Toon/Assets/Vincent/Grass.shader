// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Grass"
{
	Properties
	{
		_PlayerPosition("_PlayerPosition", Vector) = (0,0,0,0)
		_EffectBottomOffset("EffectBottomOffset", Float) = -1
		_EffectTopOffset("EffectTopOffset", Float) = 2
		_EffectRadius("Effect Radius", Range( 0 , 10)) = 1.238712
		_EffectClampMax("Effect Clamp Max", Range( 0 , 10)) = 2.571835
		_OffsetGradientStrength("OffsetGradientStrength", Range( 0 , 1)) = 0.4454732
		_OffsetFixedRoots("OffsetFixedRoots", Range( 0 , 1)) = 0.8529844
		_OffsetMultiplier("OffsetMultiplier", Range( 0 , 10)) = 10
		_World_Position_Variation_Speed("World_Position_Variation_Speed", Range( 0 , 5)) = 0.716695
		_Frequency_Wind("Frequency_Wind", Range( 0 , 1)) = 0.716695
		_XZ_Wave("XZ_Wave", Range( 0 , 2)) = 0.716695
		_GravityGradientStrength("GravityGradientStrength", Range( 0 , 1)) = 0.716695
		_Y_Wave("Y_Wave", Range( 0 , 2.94)) = 0.716695
		_Root("Root", Color) = (0.7264151,0.6686254,0.0788092,1)
		_GravityFixedRoots("GravityFixedRoots", Range( 0 , 1)) = 1
		_Grass("Grass", Color) = (0.7412298,1,0.240566,1)
		_GravityMultiplier("GravityMultiplier", Range( 0 , 100)) = 1
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_Gravity("Gravity", Vector) = (0,-1,0,0)
		_TextureSample2("Texture Sample 2", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Transparent+0" }
		Cull Off
		AlphaToMask On
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "UnityShaderVariables.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float3 worldPos;
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

		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform float _OffsetGradientStrength;
		uniform float _OffsetFixedRoots;
		uniform float _EffectRadius;
		uniform float3 _PlayerPosition;
		uniform float _EffectTopOffset;
		uniform float _EffectBottomOffset;
		uniform float _EffectClampMax;
		uniform float _OffsetMultiplier;
		uniform float3 _Gravity;
		uniform float _GravityMultiplier;
		uniform sampler2D _TextureSample1;
		uniform float4 _TextureSample1_ST;
		uniform float _GravityGradientStrength;
		uniform float _GravityFixedRoots;
		uniform float _World_Position_Variation_Speed;
		uniform float _Frequency_Wind;
		uniform float _XZ_Wave;
		uniform float _Y_Wave;
		uniform sampler2D _TextureSample2;
		uniform float4 _TextureSample2_ST;
		uniform float4 _Grass;
		uniform float4 _Root;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float2 uv_TextureSample0 = v.texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float lerpResult42 = lerp( (tex2Dlod( _TextureSample0, float4( uv_TextureSample0, 0, 0.0) )).r , 1.0 , ( 1.0 - _OffsetGradientStrength ));
			float blendOpSrc45 = v.texcoord.xy.y;
			float blendOpDest45 = lerpResult42;
			float lerpResult46 = lerp( ( saturate( ( blendOpSrc45 * blendOpDest45 ) )) , 1.0 , ( 1.0 - _OffsetFixedRoots ));
			float XZOffset49 = lerpResult46;
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float3 break2 = _PlayerPosition;
			float WorldY16 = ase_worldPos.y;
			float PlayerY6 = break2.y;
			float PlayerTopY14 = ( PlayerY6 + _EffectTopOffset );
			float PlayerBottomY11 = ( PlayerY6 + _EffectBottomOffset );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float vertexY5 = ase_vertex3Pos.y;
			float RecalculatedY24 = (( WorldY16 > PlayerTopY14 ) ? PlayerTopY14 :  (( WorldY16 < PlayerBottomY11 ) ? PlayerBottomY11 :  vertexY5 ) );
			float3 appendResult25 = (float3(break2.x , RecalculatedY24 , break2.z));
			float clampResult30 = clamp( ( _EffectRadius - distance( ase_worldPos , appendResult25 ) ) , 0.0 , _EffectClampMax );
			float3 break3 = _PlayerPosition;
			float3 appendResult35 = (float3(break3.x , ase_vertex3Pos.y , break3.z));
			float3 normalizeResult36 = normalize( ( ase_vertex3Pos - appendResult35 ) );
			float XZMultiplier52 = _OffsetMultiplier;
			float3 normalizeResult67 = normalize( -_Gravity );
			float3 YMultiplier70 = ( normalizeResult67 * _GravityMultiplier );
			float2 uv_TextureSample1 = v.texcoord * _TextureSample1_ST.xy + _TextureSample1_ST.zw;
			float lerpResult55 = lerp( tex2Dlod( _TextureSample1, float4( uv_TextureSample1, 0, 0.0) ).r , 1.0 , ( 1.0 - _GravityGradientStrength ));
			float blendOpSrc60 = v.texcoord.xy.y;
			float blendOpDest60 = lerpResult55;
			float lerpResult61 = lerp( ( saturate( ( blendOpSrc60 * blendOpDest60 ) )) , 1.0 , ( 1.0 - _GravityFixedRoots ));
			float YOffset64 = lerpResult61;
			float temp_output_122_0 = ( _Time.y / 2.0 );
			float temp_output_110_0 = sin( ( ( ase_worldPos.x + temp_output_122_0 + ase_worldPos.z ) * _World_Position_Variation_Speed ) );
			float lerpResult111 = lerp( cos( ( ( v.texcoord.xy.y + temp_output_122_0 ) / _Frequency_Wind ) ) , 0.0 , ( 1.0 - v.texcoord.xy.y ));
			float temp_output_116_0 = ( ( temp_output_110_0 * lerpResult111 ) * _XZ_Wave );
			float clampResult109 = clamp( ( 1.0 - v.texcoord.xy.y ) , 0.0 , 1.0 );
			float lerpResult113 = lerp( temp_output_110_0 , 0.0 , clampResult109);
			float3 appendResult118 = (float3(temp_output_116_0 , ( _Y_Wave * lerpResult113 ) , temp_output_116_0));
			v.vertex.xyz += ( ( ( XZOffset49 * ( clampResult30 * normalizeResult36 ) * XZMultiplier52 ) + ( YMultiplier70 * YOffset64 * -clampResult30 ) ) + ( v.color.r * appendResult118 * v.color.g ) );
		}

		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			float2 uv_TextureSample2 = i.uv_texcoord * _TextureSample2_ST.xy + _TextureSample2_ST.zw;
			float4 tex2DNode80 = tex2D( _TextureSample2, uv_TextureSample2 );
			float4 lerpResult79 = lerp( _Grass , _Root , ( 1.0 - i.uv_texcoord.y ));
			float4 blendOpSrc81 = tex2DNode80;
			float4 blendOpDest81 = lerpResult79;
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			float4 temp_output_125_0 = ( ( saturate( ( blendOpSrc81 * blendOpDest81 ) )) * float4( ase_lightColor.rgb , 0.0 ) * ase_lightColor.a );
			c.rgb = temp_output_125_0.rgb;
			c.a = tex2DNode80.a;
			return c;
		}

		inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
		{
			s.GIData = data;
		}

		void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
		{
			o.SurfInput = i;
			float2 uv_TextureSample2 = i.uv_texcoord * _TextureSample2_ST.xy + _TextureSample2_ST.zw;
			float4 tex2DNode80 = tex2D( _TextureSample2, uv_TextureSample2 );
			float4 lerpResult79 = lerp( _Grass , _Root , ( 1.0 - i.uv_texcoord.y ));
			float4 blendOpSrc81 = tex2DNode80;
			float4 blendOpDest81 = lerpResult79;
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			float4 temp_output_125_0 = ( ( saturate( ( blendOpSrc81 * blendOpDest81 ) )) * float4( ase_lightColor.rgb , 0.0 ) * ase_lightColor.a );
			o.Albedo = temp_output_125_0.rgb;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardCustomLighting keepalpha fullforwardshadows vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			AlphaToMask Off
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
				vertexDataFunc( v, customInputData );
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
				surfIN.worldPos = worldPos;
				SurfaceOutputCustomLightingCustom o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputCustomLightingCustom, o )
				surf( surfIN, o );
				UnityGI gi;
				UNITY_INITIALIZE_OUTPUT( UnityGI, gi );
				o.Alpha = LightingStandardCustomLighting( o, worldViewDir, gi ).a;
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
Version=16800
2197;1;1627;1011;-4415.204;755.0403;1;True;False
Node;AmplifyShaderEditor.Vector3Node;1;-1469.164,43.15833;Float;False;Property;_PlayerPosition;_PlayerPosition;0;0;Create;True;0;0;False;0;0,0,0;17.06,11.02,42.79;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.BreakToComponentsNode;2;-1197.63,-166.4449;Float;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.RegisterLocalVarNode;6;-890.2406,-75.6111;Float;True;PlayerY;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-1053.251,-1313.023;Float;False;Property;_EffectBottomOffset;EffectBottomOffset;1;0;Create;True;0;0;False;0;-1;-1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;7;-1009.628,-1423.706;Float;False;6;PlayerY;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;8;-1009.251,-1196.023;Float;False;6;PlayerY;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;15;-1028.914,-909.6462;Float;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;13;-1017.251,-1060.023;Float;False;Property;_EffectTopOffset;EffectTopOffset;2;0;Create;True;0;0;False;0;2;5.93;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;9;-686.2515,-1400.023;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;87;-1099.375,-364.3076;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;5;-746.6741,-441.2715;Float;False;vertexY;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;12;-655.2515,-1150.023;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;16;-708.2515,-869.0226;Float;False;WorldY;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;11;-511.2515,-1383.023;Float;False;PlayerBottomY;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;20;-166.2515,-1245.023;Float;False;16;WorldY;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;18;-300.0016,-1074.736;Float;False;11;PlayerBottomY;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;19;-271.3324,-985.4969;Float;False;5;vertexY;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;14;-501.2515,-1151.023;Float;False;PlayerTopY;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;22;148.7485,-1311.023;Float;False;16;WorldY;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;23;145.7485,-1215.023;Float;False;14;PlayerTopY;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCCompareLower;17;-60.25146,-1074.023;Float;False;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;96;1554.748,2003.147;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCCompareGreater;21;376.7485,-1229.023;Float;False;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;100;1460.197,2303.219;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;24;649.7485,-1235.023;Float;False;RecalculatedY;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;122;1904.012,2019.616;Float;False;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;26;-890.898,-171.4049;Float;False;24;RecalculatedY;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;56;1139.593,992.2494;Float;False;Property;_GravityGradientStrength;GravityGradientStrength;11;0;Create;True;0;0;False;0;0.716695;0.7044734;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;98;2145.195,2061.219;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;102;2047.886,2239.872;Float;False;Property;_Frequency_Wind;Frequency_Wind;9;0;Create;True;0;0;False;0;0.716695;0.221;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;99;1878.196,1744.219;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;53;892.8138,646.6033;Float;True;Property;_TextureSample1;Texture Sample 1;18;0;Create;True;0;0;False;0;b4aa283e8f7c6fc429be4756c2085ada;b4aa283e8f7c6fc429be4756c2085ada;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;44;885.3979,-205.4975;Float;False;Property;_OffsetGradientStrength;OffsetGradientStrength;5;0;Create;True;0;0;False;0;0.4454732;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;40;676.6045,-554.429;Float;True;Property;_TextureSample0;Texture Sample 0;17;0;Create;True;0;0;False;0;b4aa283e8f7c6fc429be4756c2085ada;b4aa283e8f7c6fc429be4756c2085ada;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;25;-584.0041,-196.5256;Float;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OneMinusNode;43;1176.407,-142.4458;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;54;1350.773,648.7106;Float;True;True;False;False;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;101;2360.195,2052.219;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;94;-853.7131,-313.8862;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ComponentMaskNode;41;1077.407,-547.4458;Float;True;True;False;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;65;2792.47,424.6874;Float;False;Property;_Gravity;Gravity;19;0;Create;True;0;0;False;0;0,-1,0;2,-10,2;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TexCoordVertexDataNode;58;2029.58,617.8439;Float;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PosVertexDataNode;95;-885.0035,561.7179;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;106;2417.633,1885.959;Float;False;Property;_World_Position_Variation_Speed;World_Position_Variation_Speed;8;0;Create;True;0;0;False;0;0.716695;2.23;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;3;-920.8539,807.2224;Float;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.OneMinusNode;57;1536.675,924.2594;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;97;2304.195,1771.219;Float;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;107;2015.164,1415.727;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexCoordVertexDataNode;38;1557.924,-551.1383;Float;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DistanceOpNode;27;-363.0246,-300.5256;Float;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-800.5022,-526.6866;Float;False;Property;_EffectRadius;Effect Radius;3;0;Create;True;0;0;False;0;1.238712;9.83;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;48;2095.18,-189.1968;Float;False;Property;_OffsetFixedRoots;OffsetFixedRoots;6;0;Create;True;0;0;False;0;0.8529844;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.CosOpNode;104;2617.633,2072.959;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;39;1886.711,-559.314;Float;True;True;True;True;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;62;2105.295,1014.075;Float;False;Property;_GravityFixedRoots;GravityFixedRoots;14;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;42;1302.407,-365.4458;Float;True;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;59;2254.924,641.7663;Float;True;True;False;False;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;108;2337.781,1441.244;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;35;-469.9902,682.3221;Float;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;105;2767.633,1753.959;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;55;1893.176,798.7795;Float;True;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NegateNode;66;3009.748,499.2611;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OneMinusNode;103;2269.633,2369.959;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;28;54.97544,-381.5256;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;60;2499.924,829.7664;Float;False;Multiply;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;45;2229.653,-468.9683;Float;True;Multiply;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;109;2696.749,1469.717;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-171.9403,-71.56115;Float;False;Property;_EffectClampMax;Effect Clamp Max;4;0;Create;True;0;0;False;0;2.571835;1.07;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;34;-228.5214,567.3846;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;69;3210.7,599.7075;Float;False;Property;_GravityMultiplier;GravityMultiplier;16;0;Create;True;0;0;False;0;1;100;0;100;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;111;2921.805,2146.83;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;47;2442.617,-191.6468;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;67;3188.747,502.261;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SinOpNode;110;3097.18,1765.108;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;63;2583.296,1003.075;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;113;3376.807,1581.33;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;112;3367.705,2067.53;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;117;3401.312,1871.891;Float;False;Property;_XZ_Wave;XZ_Wave;10;0;Create;True;0;0;False;0;0.716695;0.17;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;115;2962.324,1324.165;Float;False;Property;_Y_Wave;Y_Wave;12;0;Create;True;0;0;False;0;0.716695;0.18;0;2.94;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;68;3536.746,470.2613;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;51;2556.265,111.7545;Float;False;Property;_OffsetMultiplier;OffsetMultiplier;7;0;Create;True;0;0;False;0;10;4.43;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;46;2680.75,-310.9257;Float;True;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;61;2865.156,931.6466;Float;True;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;30;296.4976,-163.6868;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;77;4153.687,-797.3945;Float;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NormalizeNode;36;79.23156,613.9285;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;52;2908.265,111.7545;Float;False;XZMultiplier;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;75;4204.767,-1011.261;Float;False;Property;_Root;Root;13;0;Create;True;0;0;False;0;0.7264151,0.6686254,0.0788092,1;0.5033678,0.5377358,0.1902367,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;451.7776,-5.766571;Float;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;49;2904.986,-219.3868;Float;False;XZOffset;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;64;3162.454,952.4229;Float;False;YOffset;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;116;3751.312,1786.891;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;74;4222.702,-1223.13;Float;False;Property;_Grass;Grass;15;0;Create;True;0;0;False;0;0.7412298,1,0.240566,1;0.7196665,1,0.6556604,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;114;3752.575,1352.089;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NegateNode;72;1185.704,190.7821;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;78;4538.146,-794.0013;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;70;3719.588,524.461;Float;False;YMultiplier;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;71;3910.519,807.2202;Float;True;3;3;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;79;4715.421,-1017.029;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;120;4112.312,1346.891;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;118;4066.312,1601.891;Float;True;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;80;4755.896,-1303.571;Float;True;Property;_TextureSample2;Texture Sample 2;20;0;Create;True;0;0;False;0;e716f6d0bcc4d264abea8433ed77a02c;e178dd88042076b4380eb53b34d7bad7;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;50;3166.375,-34.1988;Float;True;3;3;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;119;5222.75,215.3454;Float;False;3;3;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LightColorNode;126;4748.384,-623.2139;Float;True;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.BlendOpsNode;81;4999.151,-915.4543;Float;True;Multiply;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;73;5168.647,-13.90405;Float;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;121;6051.772,377.6093;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;125;5341.461,-382.7759;Float;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;6064.292,-390.7924;Float;False;True;2;Float;ASEMaterialInspector;0;0;CustomLighting;Grass;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Translucent;0.5;True;True;0;False;Opaque;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;True;Relative;0;;-1;-1;-1;-1;0;True;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;2;0;1;0
WireConnection;6;0;2;1
WireConnection;9;0;7;0
WireConnection;9;1;10;0
WireConnection;5;0;87;2
WireConnection;12;0;8;0
WireConnection;12;1;13;0
WireConnection;16;0;15;2
WireConnection;11;0;9;0
WireConnection;14;0;12;0
WireConnection;17;0;20;0
WireConnection;17;1;18;0
WireConnection;17;2;18;0
WireConnection;17;3;19;0
WireConnection;21;0;22;0
WireConnection;21;1;23;0
WireConnection;21;2;23;0
WireConnection;21;3;17;0
WireConnection;24;0;21;0
WireConnection;122;0;96;2
WireConnection;98;0;100;2
WireConnection;98;1;122;0
WireConnection;25;0;2;0
WireConnection;25;1;26;0
WireConnection;25;2;2;2
WireConnection;43;0;44;0
WireConnection;54;0;53;1
WireConnection;101;0;98;0
WireConnection;101;1;102;0
WireConnection;41;0;40;0
WireConnection;3;0;1;0
WireConnection;57;0;56;0
WireConnection;97;0;99;1
WireConnection;97;1;122;0
WireConnection;97;2;99;3
WireConnection;27;0;94;0
WireConnection;27;1;25;0
WireConnection;104;0;101;0
WireConnection;39;0;38;2
WireConnection;42;0;41;0
WireConnection;42;2;43;0
WireConnection;59;0;58;2
WireConnection;108;0;107;2
WireConnection;35;0;3;0
WireConnection;35;1;95;2
WireConnection;35;2;3;2
WireConnection;105;0;97;0
WireConnection;105;1;106;0
WireConnection;55;0;54;0
WireConnection;55;2;57;0
WireConnection;66;0;65;0
WireConnection;103;0;100;2
WireConnection;28;0;29;0
WireConnection;28;1;27;0
WireConnection;60;0;59;0
WireConnection;60;1;55;0
WireConnection;45;0;39;0
WireConnection;45;1;42;0
WireConnection;109;0;108;0
WireConnection;34;0;95;0
WireConnection;34;1;35;0
WireConnection;111;0;104;0
WireConnection;111;2;103;0
WireConnection;47;0;48;0
WireConnection;67;0;66;0
WireConnection;110;0;105;0
WireConnection;63;0;62;0
WireConnection;113;0;110;0
WireConnection;113;2;109;0
WireConnection;112;0;110;0
WireConnection;112;1;111;0
WireConnection;68;0;67;0
WireConnection;68;1;69;0
WireConnection;46;0;45;0
WireConnection;46;2;47;0
WireConnection;61;0;60;0
WireConnection;61;2;63;0
WireConnection;30;0;28;0
WireConnection;30;2;31;0
WireConnection;36;0;34;0
WireConnection;52;0;51;0
WireConnection;37;0;30;0
WireConnection;37;1;36;0
WireConnection;49;0;46;0
WireConnection;64;0;61;0
WireConnection;116;0;112;0
WireConnection;116;1;117;0
WireConnection;114;0;115;0
WireConnection;114;1;113;0
WireConnection;72;0;30;0
WireConnection;78;0;77;2
WireConnection;70;0;68;0
WireConnection;71;0;70;0
WireConnection;71;1;64;0
WireConnection;71;2;72;0
WireConnection;79;0;74;0
WireConnection;79;1;75;0
WireConnection;79;2;78;0
WireConnection;118;0;116;0
WireConnection;118;1;114;0
WireConnection;118;2;116;0
WireConnection;50;0;49;0
WireConnection;50;1;37;0
WireConnection;50;2;52;0
WireConnection;119;0;120;1
WireConnection;119;1;118;0
WireConnection;119;2;120;2
WireConnection;81;0;80;0
WireConnection;81;1;79;0
WireConnection;73;0;50;0
WireConnection;73;1;71;0
WireConnection;121;0;73;0
WireConnection;121;1;119;0
WireConnection;125;0;81;0
WireConnection;125;1;126;1
WireConnection;125;2;126;2
WireConnection;0;0;125;0
WireConnection;0;9;80;4
WireConnection;0;13;125;0
WireConnection;0;11;121;0
ASEEND*/
//CHKSM=6DCC046205B6F00E3A2428BC931CE0793D48D9CC