// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:Standard,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:0,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:True,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:0,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:4013,x:33609,y:32274,varname:node_4013,prsc:2|normal-2280-RGB,emission-6465-OUT,custl-4683-OUT;n:type:ShaderForge.SFN_LightVector,id:7529,x:30942,y:32348,varname:node_7529,prsc:2;n:type:ShaderForge.SFN_NormalVector,id:7249,x:30942,y:32546,prsc:2,pt:True;n:type:ShaderForge.SFN_Dot,id:2637,x:31132,y:32433,varname:node_2637,prsc:2,dt:1|A-7529-OUT,B-7249-OUT;n:type:ShaderForge.SFN_Set,id:1527,x:31289,y:32457,varname:Out,prsc:2|IN-2637-OUT;n:type:ShaderForge.SFN_Get,id:4114,x:31505,y:32699,varname:node_4114,prsc:2|IN-1527-OUT;n:type:ShaderForge.SFN_Multiply,id:8108,x:31740,y:32675,varname:node_8108,prsc:2|A-4114-OUT,B-7029-OUT;n:type:ShaderForge.SFN_ValueProperty,id:2039,x:31505,y:32519,ptovrint:False,ptlb:Tone,ptin:_Tone,varname:node_2039,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:3;n:type:ShaderForge.SFN_Round,id:5720,x:31893,y:32716,varname:node_5720,prsc:2|IN-8108-OUT;n:type:ShaderForge.SFN_Divide,id:1105,x:32036,y:32591,varname:node_1105,prsc:2|A-5720-OUT,B-7029-OUT;n:type:ShaderForge.SFN_Multiply,id:5454,x:32069,y:33011,varname:node_5454,prsc:2|A-3528-OUT,B-8756-RGB,C-4337-OUT,D-2686-OUT;n:type:ShaderForge.SFN_LightColor,id:8756,x:31710,y:32866,varname:node_8756,prsc:2;n:type:ShaderForge.SFN_LightAttenuation,id:4337,x:31710,y:33006,varname:node_4337,prsc:2;n:type:ShaderForge.SFN_Lerp,id:4683,x:32661,y:32804,varname:node_4683,prsc:2|A-1545-OUT,B-5551-OUT,T-5454-OUT;n:type:ShaderForge.SFN_Tex2d,id:6008,x:32022,y:32225,ptovrint:False,ptlb:Albedo,ptin:_Albedo,varname:node_6008,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Color,id:6700,x:32022,y:32406,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_6700,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:3704,x:32237,y:32369,varname:node_3704,prsc:2|A-6008-RGB,B-6700-RGB;n:type:ShaderForge.SFN_Get,id:5551,x:32699,y:32515,varname:node_5551,prsc:2|IN-2583-OUT;n:type:ShaderForge.SFN_Set,id:2583,x:32390,y:32295,varname:BlackShadow,prsc:2|IN-3704-OUT;n:type:ShaderForge.SFN_Slider,id:3440,x:32275,y:32825,ptovrint:False,ptlb:Shadow Darkness,ptin:_ShadowDarkness,varname:node_3440,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-1,cur:0.4081948,max:1;n:type:ShaderForge.SFN_Multiply,id:1545,x:32398,y:32678,varname:node_1545,prsc:2|A-5551-OUT,B-3440-OUT;n:type:ShaderForge.SFN_Multiply,id:6043,x:32757,y:32244,varname:node_6043,prsc:2|A-3704-OUT,B-7059-OUT;n:type:ShaderForge.SFN_AmbientLight,id:1574,x:32379,y:32495,varname:node_1574,prsc:2;n:type:ShaderForge.SFN_Subtract,id:7029,x:31675,y:32475,varname:node_7029,prsc:2|A-2039-OUT,B-9468-OUT;n:type:ShaderForge.SFN_Vector1,id:9468,x:31484,y:32596,varname:node_9468,prsc:2,v1:1;n:type:ShaderForge.SFN_Multiply,id:7059,x:32625,y:32363,varname:node_7059,prsc:2|A-1574-RGB,B-8864-OUT;n:type:ShaderForge.SFN_ValueProperty,id:8864,x:32545,y:32544,ptovrint:False,ptlb:Ambient Multiplier,ptin:_AmbientMultiplier,varname:node_8864,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Tex2d,id:9143,x:32728,y:31894,ptovrint:False,ptlb:EmmisionMap,ptin:_EmmisionMap,varname:node_9143,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:7484,x:33008,y:32006,varname:node_7484,prsc:2|A-446-RGB,B-9143-RGB;n:type:ShaderForge.SFN_Color,id:446,x:32767,y:32111,ptovrint:False,ptlb:EmmisionColor,ptin:_EmmisionColor,varname:node_446,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:6465,x:33092,y:32297,varname:node_6465,prsc:2|A-6043-OUT,B-7484-OUT;n:type:ShaderForge.SFN_ValueProperty,id:2686,x:31710,y:33149,ptovrint:False,ptlb:LightMultiply,ptin:_LightMultiply,varname:node_2686,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2;n:type:ShaderForge.SFN_Multiply,id:3528,x:32181,y:32713,varname:node_3528,prsc:2|A-1105-OUT,B-7941-OUT;n:type:ShaderForge.SFN_ValueProperty,id:7941,x:32020,y:32836,ptovrint:False,ptlb:AllRoundMultiply,ptin:_AllRoundMultiply,varname:node_7941,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Tex2d,id:2280,x:33272,y:32124,ptovrint:False,ptlb:Normal Map,ptin:_NormalMap,varname:node_2280,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:3,isnm:True;proporder:2039-6008-6700-3440-8864-9143-446-2686-7941-2280;pass:END;sub:END;*/

Shader "Shader Forge/ToonLit" {
    Properties {
        _Tone ("Tone", Float ) = 3
        _Albedo ("Albedo", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _ShadowDarkness ("Shadow Darkness", Range(-1, 1)) = 0.4081948
        _AmbientMultiplier ("Ambient Multiplier", Float ) = 0
        _EmmisionMap ("EmmisionMap", 2D) = "white" {}
        _EmmisionColor ("EmmisionColor", Color) = (0.5,0.5,0.5,1)
        _LightMultiply ("LightMultiply", Float ) = 2
        _AllRoundMultiply ("AllRoundMultiply", Float ) = 1
        _NormalMap ("Normal Map", 2D) = "bump" {}
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Cull Off
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float _Tone;
            uniform sampler2D _Albedo; uniform float4 _Albedo_ST;
            uniform float4 _Color;
            uniform float _ShadowDarkness;
            uniform float _AmbientMultiplier;
            uniform sampler2D _EmmisionMap; uniform float4 _EmmisionMap_ST;
            uniform float4 _EmmisionColor;
            uniform float _LightMultiply;
            uniform float _AllRoundMultiply;
            uniform sampler2D _NormalMap; uniform float4 _NormalMap_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
                UNITY_FOG_COORDS(7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _NormalMap_var = UnpackNormal(tex2D(_NormalMap,TRANSFORM_TEX(i.uv0, _NormalMap)));
                float3 normalLocal = _NormalMap_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
////// Emissive:
                float4 _Albedo_var = tex2D(_Albedo,TRANSFORM_TEX(i.uv0, _Albedo));
                float3 node_3704 = (_Albedo_var.rgb*_Color.rgb);
                float4 _EmmisionMap_var = tex2D(_EmmisionMap,TRANSFORM_TEX(i.uv0, _EmmisionMap));
                float3 emissive = ((node_3704*(UNITY_LIGHTMODEL_AMBIENT.rgb*_AmbientMultiplier))*(_EmmisionColor.rgb*_EmmisionMap_var.rgb));
                float3 BlackShadow = node_3704;
                float3 node_5551 = BlackShadow;
                float Out = max(0,dot(lightDirection,normalDirection));
                float node_7029 = (_Tone-1.0);
                float3 finalColor = emissive + lerp((node_5551*_ShadowDarkness),node_5551,(((round((Out*node_7029))/node_7029)*_AllRoundMultiply)*_LightColor0.rgb*attenuation*_LightMultiply));
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            Cull Off
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float _Tone;
            uniform sampler2D _Albedo; uniform float4 _Albedo_ST;
            uniform float4 _Color;
            uniform float _ShadowDarkness;
            uniform float _AmbientMultiplier;
            uniform sampler2D _EmmisionMap; uniform float4 _EmmisionMap_ST;
            uniform float4 _EmmisionColor;
            uniform float _LightMultiply;
            uniform float _AllRoundMultiply;
            uniform sampler2D _NormalMap; uniform float4 _NormalMap_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
                UNITY_FOG_COORDS(7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _NormalMap_var = UnpackNormal(tex2D(_NormalMap,TRANSFORM_TEX(i.uv0, _NormalMap)));
                float3 normalLocal = _NormalMap_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float4 _Albedo_var = tex2D(_Albedo,TRANSFORM_TEX(i.uv0, _Albedo));
                float3 node_3704 = (_Albedo_var.rgb*_Color.rgb);
                float3 BlackShadow = node_3704;
                float3 node_5551 = BlackShadow;
                float Out = max(0,dot(lightDirection,normalDirection));
                float node_7029 = (_Tone-1.0);
                float3 finalColor = lerp((node_5551*_ShadowDarkness),node_5551,(((round((Out*node_7029))/node_7029)*_AllRoundMultiply)*_LightColor0.rgb*attenuation*_LightMultiply));
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Standard"
    CustomEditor "ShaderForgeMaterialInspector"
}
