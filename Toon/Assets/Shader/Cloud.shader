// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:4013,x:33706,y:32319,varname:node_4013,prsc:2|emission-7608-RGB,alpha-4466-OUT,clip-4466-OUT;n:type:ShaderForge.SFN_Tex2d,id:3999,x:32216,y:32820,varname:node_3999,prsc:2,tex:28c7aad1372ff114b90d330f8a2dd938,ntxv:0,isnm:False|UVIN-9508-OUT,TEX-2384-TEX;n:type:ShaderForge.SFN_Add,id:9508,x:31934,y:32860,varname:node_9508,prsc:2|A-8322-OUT,B-221-OUT;n:type:ShaderForge.SFN_FragmentPosition,id:9882,x:31474,y:32815,varname:node_9882,prsc:2;n:type:ShaderForge.SFN_Append,id:8322,x:31746,y:32860,varname:node_8322,prsc:2|A-9882-X,B-9882-Z;n:type:ShaderForge.SFN_Time,id:5931,x:31420,y:33024,varname:node_5931,prsc:2;n:type:ShaderForge.SFN_Append,id:221,x:31746,y:33007,varname:node_221,prsc:2|A-5931-T,B-5931-T;n:type:ShaderForge.SFN_Tex2dAsset,id:2384,x:31669,y:32579,ptovrint:False,ptlb:node_2384,ptin:_node_2384,varname:node_2384,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:28c7aad1372ff114b90d330f8a2dd938,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:9268,x:32235,y:32483,varname:node_9268,prsc:2,tex:28c7aad1372ff114b90d330f8a2dd938,ntxv:0,isnm:False|UVIN-4076-OUT,TEX-2384-TEX;n:type:ShaderForge.SFN_Multiply,id:4076,x:32036,y:32606,varname:node_4076,prsc:2|A-3709-OUT,B-8841-OUT;n:type:ShaderForge.SFN_Subtract,id:8841,x:31895,y:32676,varname:node_8841,prsc:2|A-8322-OUT,B-221-OUT;n:type:ShaderForge.SFN_ValueProperty,id:3709,x:31742,y:32405,ptovrint:False,ptlb:node_3709,ptin:_node_3709,varname:node_3709,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Color,id:7608,x:33217,y:32256,ptovrint:False,ptlb:node_7608,ptin:_node_7608,varname:node_7608,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:5895,x:32410,y:32677,varname:node_5895,prsc:2|A-9268-R,B-3999-R;n:type:ShaderForge.SFN_Slider,id:9289,x:32428,y:32582,ptovrint:False,ptlb:node_9289,ptin:_node_9289,varname:node_9289,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.0442711,max:1;n:type:ShaderForge.SFN_Step,id:4466,x:32766,y:32675,varname:node_4466,prsc:2|A-9289-OUT,B-5895-OUT;proporder:2384-3709-7608-9289;pass:END;sub:END;*/

Shader "Shader Forge/Cloud" {
    Properties {
        _node_2384 ("node_2384", 2D) = "white" {}
        _node_3709 ("node_3709", Float ) = 1
        _node_7608 ("node_7608", Color) = (0.5,0.5,0.5,1)
        _node_9289 ("node_9289", Range(0, 1)) = 0.0442711
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
            Blend One OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _node_2384; uniform float4 _node_2384_ST;
            uniform float _node_3709;
            uniform float4 _node_7608;
            uniform float _node_9289;
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                UNITY_FOG_COORDS(1)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float2 node_8322 = float2(i.posWorld.r,i.posWorld.b);
                float4 node_5931 = _Time;
                float2 node_221 = float2(node_5931.g,node_5931.g);
                float2 node_4076 = (_node_3709*(node_8322-node_221));
                float4 node_9268 = tex2D(_node_2384,TRANSFORM_TEX(node_4076, _node_2384));
                float2 node_9508 = (node_8322+node_221);
                float4 node_3999 = tex2D(_node_2384,TRANSFORM_TEX(node_9508, _node_2384));
                float node_5895 = (node_9268.r*node_3999.r);
                float node_4466 = step(_node_9289,node_5895);
                clip(node_4466 - 0.5);
////// Lighting:
////// Emissive:
                float3 emissive = _node_7608.rgb;
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,node_4466);
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
            Cull Back
            
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
            uniform sampler2D _node_2384; uniform float4 _node_2384_ST;
            uniform float _node_3709;
            uniform float _node_9289;
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float4 posWorld : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float2 node_8322 = float2(i.posWorld.r,i.posWorld.b);
                float4 node_5931 = _Time;
                float2 node_221 = float2(node_5931.g,node_5931.g);
                float2 node_4076 = (_node_3709*(node_8322-node_221));
                float4 node_9268 = tex2D(_node_2384,TRANSFORM_TEX(node_4076, _node_2384));
                float2 node_9508 = (node_8322+node_221);
                float4 node_3999 = tex2D(_node_2384,TRANSFORM_TEX(node_9508, _node_2384));
                float node_5895 = (node_9268.r*node_3999.r);
                float node_4466 = step(_node_9289,node_5895);
                clip(node_4466 - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
