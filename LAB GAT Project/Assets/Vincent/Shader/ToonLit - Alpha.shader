// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:Standard,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:0,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:True,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:0,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:4013,x:33381,y:32200,varname:node_4013,prsc:2|emission-6043-OUT,custl-4683-OUT,alpha-6008-A;n:type:ShaderForge.SFN_LightVector,id:7529,x:30942,y:32348,varname:node_7529,prsc:2;n:type:ShaderForge.SFN_NormalVector,id:7249,x:30942,y:32546,prsc:2,pt:True;n:type:ShaderForge.SFN_Dot,id:2637,x:31132,y:32433,varname:node_2637,prsc:2,dt:1|A-7529-OUT,B-7249-OUT;n:type:ShaderForge.SFN_Set,id:1527,x:31289,y:32457,varname:Out,prsc:2|IN-2637-OUT;n:type:ShaderForge.SFN_Get,id:4114,x:31505,y:32699,varname:node_4114,prsc:2|IN-1527-OUT;n:type:ShaderForge.SFN_Multiply,id:8108,x:31740,y:32675,varname:node_8108,prsc:2|A-4114-OUT,B-7029-OUT;n:type:ShaderForge.SFN_ValueProperty,id:2039,x:31505,y:32519,ptovrint:False,ptlb:Tone,ptin:_Tone,varname:node_2039,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:3;n:type:ShaderForge.SFN_Round,id:5720,x:31901,y:32708,varname:node_5720,prsc:2|IN-8108-OUT;n:type:ShaderForge.SFN_Divide,id:1105,x:32109,y:32646,varname:node_1105,prsc:2|A-5720-OUT,B-7029-OUT;n:type:ShaderForge.SFN_Multiply,id:5454,x:32233,y:32895,varname:node_5454,prsc:2|A-1105-OUT,B-8756-RGB,C-4337-OUT;n:type:ShaderForge.SFN_LightColor,id:8756,x:31710,y:32866,varname:node_8756,prsc:2;n:type:ShaderForge.SFN_LightAttenuation,id:4337,x:31916,y:32980,varname:node_4337,prsc:2;n:type:ShaderForge.SFN_Lerp,id:4683,x:32661,y:32804,varname:node_4683,prsc:2|A-1545-OUT,B-5551-OUT,T-5454-OUT;n:type:ShaderForge.SFN_Tex2d,id:6008,x:32022,y:32171,ptovrint:False,ptlb:Albedo,ptin:_Albedo,varname:node_6008,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:9b7621fb338c8064395dd53fecd6d466,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Color,id:6700,x:32022,y:32406,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_6700,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:3704,x:32237,y:32369,varname:node_3704,prsc:2|A-6008-RGB,B-6700-RGB;n:type:ShaderForge.SFN_Get,id:5551,x:32699,y:32515,varname:node_5551,prsc:2|IN-2583-OUT;n:type:ShaderForge.SFN_Set,id:2583,x:32390,y:32295,varname:BlackShadow,prsc:2|IN-3704-OUT;n:type:ShaderForge.SFN_Slider,id:3440,x:32241,y:32818,ptovrint:False,ptlb:Shadow Darkness,ptin:_ShadowDarkness,varname:node_3440,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-1,cur:0.1941669,max:1;n:type:ShaderForge.SFN_Multiply,id:1545,x:32398,y:32678,varname:node_1545,prsc:2|A-5551-OUT,B-3440-OUT;n:type:ShaderForge.SFN_Multiply,id:6043,x:32757,y:32244,varname:node_6043,prsc:2|A-3704-OUT,B-7059-OUT;n:type:ShaderForge.SFN_AmbientLight,id:1574,x:32379,y:32495,varname:node_1574,prsc:2;n:type:ShaderForge.SFN_Subtract,id:7029,x:31675,y:32475,varname:node_7029,prsc:2|A-2039-OUT,B-9468-OUT;n:type:ShaderForge.SFN_Vector1,id:9468,x:31484,y:32596,varname:node_9468,prsc:2,v1:1;n:type:ShaderForge.SFN_Multiply,id:7059,x:32625,y:32363,varname:node_7059,prsc:2|A-1574-RGB,B-8864-OUT;n:type:ShaderForge.SFN_ValueProperty,id:8864,x:32545,y:32544,ptovrint:False,ptlb:Ambient Multiplier,ptin:_AmbientMultiplier,varname:node_8864,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;proporder:2039-6008-6700-3440-8864;pass:END;sub:END;*/

Shader "Shader Forge/ToonLit - Alpha" {
    Properties {
        _Tone ("Tone", Float ) = 3
        _Albedo ("Albedo", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _ShadowDarkness ("Shadow Darkness", Range(-1, 1)) = 0.1941669
        _AmbientMultiplier ("Ambient Multiplier", Float ) = 0
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float _Tone;
            uniform sampler2D _Albedo; uniform float4 _Albedo_ST;
            uniform float4 _Color;
            uniform float _ShadowDarkness;
            uniform float _AmbientMultiplier;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                UNITY_FOG_COORDS(3)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = 1;
////// Emissive:
                float4 _Albedo_var = tex2D(_Albedo,TRANSFORM_TEX(i.uv0, _Albedo));
                float3 node_3704 = (_Albedo_var.rgb*_Color.rgb);
                float3 emissive = (node_3704*(UNITY_LIGHTMODEL_AMBIENT.rgb*_AmbientMultiplier));
                float3 BlackShadow = node_3704;
                float3 node_5551 = BlackShadow;
                float Out = max(0,dot(lightDirection,normalDirection));
                float node_7029 = (_Tone-1.0);
                float3 finalColor = emissive + lerp((node_5551*_ShadowDarkness),node_5551,((round((Out*node_7029))/node_7029)*_LightColor0.rgb*attenuation));
                fixed4 finalRGBA = fixed4(finalColor,_Albedo_var.a);
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
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdadd
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float _Tone;
            uniform sampler2D _Albedo; uniform float4 _Albedo_ST;
            uniform float4 _Color;
            uniform float _ShadowDarkness;
            uniform float _AmbientMultiplier;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
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
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
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
                float3 finalColor = lerp((node_5551*_ShadowDarkness),node_5551,((round((Out*node_7029))/node_7029)*_LightColor0.rgb*attenuation));
                fixed4 finalRGBA = fixed4(finalColor * _Albedo_var.a,0);
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
