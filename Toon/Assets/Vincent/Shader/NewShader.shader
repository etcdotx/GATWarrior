// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:3,spmd:0,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:2,hqsc:True,nrmq:0,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:36109,y:32270,varname:node_3138,prsc:2|emission-6815-OUT,alpha-7181-OUT,clip-9207-OUT,voffset-2648-OUT;n:type:ShaderForge.SFN_Tex2dAsset,id:1858,x:32492,y:32775,ptovrint:False,ptlb:Noise Map,ptin:_NoiseMap,varname:node_1858,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:e9a4aa7782b2be54384cff370544c467,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:1839,x:32766,y:32640,varname:node_1839,prsc:2,tex:e9a4aa7782b2be54384cff370544c467,ntxv:0,isnm:False|UVIN-2504-OUT,TEX-1858-TEX;n:type:ShaderForge.SFN_Tex2d,id:3064,x:32766,y:32825,varname:node_3064,prsc:2,tex:e9a4aa7782b2be54384cff370544c467,ntxv:0,isnm:False|UVIN-773-OUT,TEX-1858-TEX;n:type:ShaderForge.SFN_Multiply,id:9193,x:32251,y:32642,varname:node_9193,prsc:2|A-4452-OUT,B-2991-OUT;n:type:ShaderForge.SFN_Multiply,id:6532,x:32252,y:32965,varname:node_6532,prsc:2|A-4452-OUT,B-5319-OUT;n:type:ShaderForge.SFN_Panner,id:3522,x:32492,y:32600,varname:node_3522,prsc:2,spu:1,spv:1|UVIN-9193-OUT;n:type:ShaderForge.SFN_Panner,id:3429,x:32492,y:32965,varname:node_3429,prsc:2,spu:1,spv:1|UVIN-6532-OUT;n:type:ShaderForge.SFN_ValueProperty,id:2991,x:31988,y:32699,ptovrint:False,ptlb:Noise 1 Scale,ptin:_Noise1Scale,varname:node_2991,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:5319,x:32034,y:32999,ptovrint:False,ptlb:Noise 2 Scale,ptin:_Noise2Scale,varname:_node_2991_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.5;n:type:ShaderForge.SFN_Multiply,id:4277,x:33009,y:32838,varname:node_4277,prsc:2|A-1839-R,B-3064-R;n:type:ShaderForge.SFN_TexCoord,id:6960,x:31786,y:32738,varname:node_6960,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Multiply,id:4452,x:32061,y:32830,varname:node_4452,prsc:2|A-6960-UVOUT,B-1085-OUT;n:type:ShaderForge.SFN_Vector2,id:1085,x:31786,y:32926,varname:node_1085,prsc:2,v1:1.5,v2:1;n:type:ShaderForge.SFN_Append,id:2999,x:32380,y:33174,varname:node_2999,prsc:2|A-2877-OUT,B-8796-OUT;n:type:ShaderForge.SFN_ValueProperty,id:2877,x:32136,y:33153,ptovrint:False,ptlb:Non-Usage,ptin:_NonUsage,varname:node_2877,prsc:2,glob:False,taghide:True,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:5426,x:32538,y:33214,varname:node_5426,prsc:2|A-2999-OUT,B-2518-T;n:type:ShaderForge.SFN_Time,id:2518,x:32365,y:33345,varname:node_2518,prsc:2;n:type:ShaderForge.SFN_Add,id:773,x:32675,y:33036,varname:node_773,prsc:2|A-3429-UVOUT,B-5426-OUT;n:type:ShaderForge.SFN_Append,id:6046,x:32075,y:32218,varname:node_6046,prsc:2|A-5871-OUT,B-911-OUT;n:type:ShaderForge.SFN_Time,id:7955,x:32001,y:31974,varname:node_7955,prsc:2;n:type:ShaderForge.SFN_Multiply,id:2242,x:32322,y:32135,varname:node_2242,prsc:2|A-6046-OUT,B-7955-T;n:type:ShaderForge.SFN_Add,id:2504,x:32629,y:32224,varname:node_2504,prsc:2|A-3522-UVOUT,B-2242-OUT;n:type:ShaderForge.SFN_ValueProperty,id:5871,x:31808,y:32185,ptovrint:False,ptlb:Non-Usage 2,ptin:_NonUsage2,varname:node_5871,prsc:2,glob:False,taghide:True,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:-1;n:type:ShaderForge.SFN_Tex2d,id:5519,x:33156,y:32600,ptovrint:False,ptlb:Mask,ptin:_Mask,varname:node_5519,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:7115b29482847fb4a85e2bcee27c247f,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Add,id:8023,x:33201,y:32814,varname:node_8023,prsc:2|A-5519-R,B-4277-OUT;n:type:ShaderForge.SFN_Multiply,id:333,x:33417,y:32713,varname:node_333,prsc:2|A-5519-R,B-8023-OUT;n:type:ShaderForge.SFN_Step,id:7035,x:33858,y:32264,varname:node_7035,prsc:2|A-1491-OUT,B-8085-OUT;n:type:ShaderForge.SFN_Slider,id:1491,x:33433,y:32227,ptovrint:False,ptlb:Inner Fire(White),ptin:_InnerFireWhite,varname:node_1491,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.3780058,max:1;n:type:ShaderForge.SFN_Relay,id:8085,x:33718,y:32368,varname:node_8085,prsc:2|IN-333-OUT;n:type:ShaderForge.SFN_Step,id:1890,x:34088,y:32663,varname:node_1890,prsc:2|A-7933-OUT,B-8085-OUT;n:type:ShaderForge.SFN_Slider,id:7933,x:33464,y:32531,ptovrint:False,ptlb:Outer Fire(Black),ptin:_OuterFireBlack,varname:node_7933,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.3129299,max:1;n:type:ShaderForge.SFN_Slider,id:8796,x:32012,y:33278,ptovrint:False,ptlb:Noise 2 Speed,ptin:_Noise2Speed,varname:node_8796,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-3,cur:-2,max:-2;n:type:ShaderForge.SFN_Slider,id:911,x:31729,y:32303,ptovrint:False,ptlb:Noise 1 Speed,ptin:_Noise1Speed,varname:node_911,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-3,cur:-2,max:-2;n:type:ShaderForge.SFN_OneMinus,id:9127,x:34126,y:32000,varname:node_9127,prsc:2|IN-7035-OUT;n:type:ShaderForge.SFN_Multiply,id:5557,x:34464,y:31948,varname:node_5557,prsc:2|A-5198-OUT,B-9127-OUT;n:type:ShaderForge.SFN_Multiply,id:452,x:34284,y:32213,varname:node_452,prsc:2|A-6966-RGB,B-7035-OUT;n:type:ShaderForge.SFN_Color,id:3005,x:33784,y:31396,ptovrint:False,ptlb:Outer Color,ptin:_OuterColor,varname:node_3005,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0.1348233,c3:0,c4:0;n:type:ShaderForge.SFN_Color,id:6966,x:34095,y:32360,ptovrint:False,ptlb:Inner Color,ptin:_InnerColor,varname:node_6966,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0.8855018,c3:0,c4:0;n:type:ShaderForge.SFN_Add,id:5488,x:34500,y:32149,varname:node_5488,prsc:2|A-5557-OUT,B-452-OUT;n:type:ShaderForge.SFN_TexCoord,id:421,x:33033,y:31764,varname:node_421,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Smoothstep,id:1447,x:33567,y:31803,varname:node_1447,prsc:2|A-8284-OUT,B-1755-OUT,V-421-V;n:type:ShaderForge.SFN_Slider,id:1755,x:33196,y:32037,ptovrint:False,ptlb:Outer Color Blend,ptin:_OuterColorBlend,varname:node_1755,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.7580363,max:1;n:type:ShaderForge.SFN_ValueProperty,id:8284,x:33372,y:31748,ptovrint:False,ptlb:Non-Usage 3,ptin:_NonUsage3,varname:node_8284,prsc:2,glob:False,taghide:True,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Color,id:3793,x:33857,y:31883,ptovrint:False,ptlb:Outer Color Top,ptin:_OuterColorTop,varname:node_3793,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.05782976,c2:0,c3:0.9339623,c4:0;n:type:ShaderForge.SFN_OneMinus,id:6542,x:33826,y:31661,varname:node_6542,prsc:2|IN-1447-OUT;n:type:ShaderForge.SFN_Multiply,id:9540,x:34066,y:31762,varname:node_9540,prsc:2|A-1447-OUT,B-3793-RGB;n:type:ShaderForge.SFN_Multiply,id:5616,x:34073,y:31463,varname:node_5616,prsc:2|A-3005-RGB,B-6542-OUT;n:type:ShaderForge.SFN_Add,id:5198,x:34303,y:31646,varname:node_5198,prsc:2|A-5616-OUT,B-9540-OUT;n:type:ShaderForge.SFN_Add,id:4179,x:35009,y:31941,varname:node_4179,prsc:2|A-1618-OUT,B-9142-OUT;n:type:ShaderForge.SFN_Get,id:9142,x:34621,y:31901,varname:node_9142,prsc:2|IN-7920-OUT;n:type:ShaderForge.SFN_Set,id:7920,x:34074,y:32561,varname:node_7920,prsc:2|IN-8085-OUT;n:type:ShaderForge.SFN_Slider,id:2017,x:34502,y:31702,ptovrint:False,ptlb:Shading Amount,ptin:_ShadingAmount,varname:node_2017,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.3957081,max:1;n:type:ShaderForge.SFN_Multiply,id:6815,x:35496,y:32158,varname:node_6815,prsc:2|A-847-OUT,B-5488-OUT,C-4312-OUT;n:type:ShaderForge.SFN_ValueProperty,id:4312,x:34899,y:32266,ptovrint:False,ptlb:Brightness,ptin:_Brightness,varname:node_4312,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1.25;n:type:ShaderForge.SFN_Slider,id:296,x:34534,y:32403,ptovrint:False,ptlb:Premultiply Blend,ptin:_PremultiplyBlend,varname:node_296,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.3022388,max:1;n:type:ShaderForge.SFN_Relay,id:7807,x:34626,y:32673,varname:node_7807,prsc:2|IN-1890-OUT;n:type:ShaderForge.SFN_Add,id:7815,x:34963,y:32521,varname:node_7815,prsc:2|A-296-OUT,B-7807-OUT;n:type:ShaderForge.SFN_Step,id:9207,x:35227,y:32663,varname:node_9207,prsc:2|A-7041-OUT,B-7807-OUT;n:type:ShaderForge.SFN_Clamp01,id:8162,x:35161,y:32436,varname:node_8162,prsc:2|IN-7815-OUT;n:type:ShaderForge.SFN_Slider,id:7041,x:34732,y:32849,ptovrint:False,ptlb:Opacity Cutoff,ptin:_OpacityCutoff,varname:node_7041,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.5167054,max:1;n:type:ShaderForge.SFN_Clamp01,id:847,x:35128,y:31988,varname:node_847,prsc:2|IN-4179-OUT;n:type:ShaderForge.SFN_OneMinus,id:1618,x:34841,y:31719,varname:node_1618,prsc:2|IN-2017-OUT;n:type:ShaderForge.SFN_DepthBlend,id:6224,x:35411,y:32502,varname:node_6224,prsc:2|DIST-4438-OUT;n:type:ShaderForge.SFN_Multiply,id:7181,x:35773,y:32475,varname:node_7181,prsc:2|A-8162-OUT,B-6224-OUT;n:type:ShaderForge.SFN_Slider,id:4438,x:35070,y:32595,ptovrint:False,ptlb:Depth Opacity,ptin:_DepthOpacity,varname:node_4438,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-10,cur:3.560155,max:10;n:type:ShaderForge.SFN_ViewPosition,id:379,x:35183,y:33132,varname:node_379,prsc:2;n:type:ShaderForge.SFN_Multiply,id:2648,x:35865,y:33004,varname:node_2648,prsc:2|A-8655-OUT,B-7152-OUT;n:type:ShaderForge.SFN_ValueProperty,id:8655,x:35483,y:32953,ptovrint:False,ptlb:Push Forward,ptin:_PushForward,varname:node_8655,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.18;n:type:ShaderForge.SFN_Subtract,id:3279,x:35406,y:33188,varname:node_3279,prsc:2|A-379-XYZ,B-7051-XYZ;n:type:ShaderForge.SFN_FragmentPosition,id:7051,x:35183,y:33291,varname:node_7051,prsc:2;n:type:ShaderForge.SFN_Normalize,id:7152,x:35570,y:33168,varname:node_7152,prsc:2|IN-3279-OUT;proporder:1858-2991-5319-2877-5871-5519-1491-7933-8796-911-3005-6966-1755-8284-3793-2017-4312-296-7041-4438-8655;pass:END;sub:END;*/

Shader "Shader Forge/NewShader" {
    Properties {
        _NoiseMap ("Noise Map", 2D) = "white" {}
        _Noise1Scale ("Noise 1 Scale", Float ) = 1
        _Noise2Scale ("Noise 2 Scale", Float ) = 0.5
        [HideInInspector]_NonUsage ("Non-Usage", Float ) = 0
        [HideInInspector]_NonUsage2 ("Non-Usage 2", Float ) = -1
        _Mask ("Mask", 2D) = "white" {}
        _InnerFireWhite ("Inner Fire(White)", Range(0, 1)) = 0.3780058
        _OuterFireBlack ("Outer Fire(Black)", Range(0, 1)) = 0.3129299
        _Noise2Speed ("Noise 2 Speed", Range(-3, -1)) = -2
        _Noise1Speed ("Noise 1 Speed", Range(-3, -1)) = -2
        _OuterColor ("Outer Color", Color) = (1,0.1348233,0,0)
        _InnerColor ("Inner Color", Color) = (1,0.8855018,0,0)
        _OuterColorBlend ("Outer Color Blend", Range(0, 1)) = 0.7580363
        [HideInInspector]_NonUsage3 ("Non-Usage 3", Float ) = 0
        _OuterColorTop ("Outer Color Top", Color) = (0.05782976,0,0.9339623,0)
        _ShadingAmount ("Shading Amount", Range(0, 1)) = 0.3957081
        _Brightness ("Brightness", Float ) = 1.25
        _PremultiplyBlend ("Premultiply Blend", Range(0, 1)) = 0.3022388
        _OpacityCutoff ("Opacity Cutoff", Range(0, 1)) = 0.5167054
        _DepthOpacity ("Depth Opacity", Range(-10, 10)) = 3.560155
        _PushForward ("Push Forward", Float ) = 0.18
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
			_ScaleX("Scale X", Float) = 1.0
		_ScaleY("Scale Y", Float) = 1.0
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
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _CameraDepthTexture;
            uniform sampler2D _NoiseMap; uniform float4 _NoiseMap_ST;
            uniform float _Noise1Scale;
            uniform float _Noise2Scale;
            uniform float _NonUsage;
            uniform float _NonUsage2;
            uniform sampler2D _Mask; uniform float4 _Mask_ST;
            uniform float _InnerFireWhite;
            uniform float _OuterFireBlack;
            uniform float _Noise2Speed;
            uniform float _Noise1Speed;
            uniform float4 _OuterColor;
            uniform float4 _InnerColor;
            uniform float _OuterColorBlend;
            uniform float _NonUsage3;
            uniform float4 _OuterColorTop;
            uniform float _ShadingAmount;
            uniform float _Brightness;
            uniform float _PremultiplyBlend;
            uniform float _OpacityCutoff;
            uniform float _DepthOpacity;
            uniform float _PushForward;
			uniform float _ScaleX;
			uniform float _ScaleY;
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
                float4 projPos : TEXCOORD3;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                v.vertex.xyz += (_PushForward*normalize((_WorldSpaceCameraPos-mul(unity_ObjectToWorld, v.vertex).rgb)));
                //o.posWorld = mul(unity_ObjectToWorld, v.vertex);
				// The world position of the center of the object
				float3 worldPos = mul(unity_ObjectToWorld, float4(0, 0, 0, 1)).xyz;

				// Distance between the camera and the center
				float3 dist = _WorldSpaceCameraPos - worldPos;

				// atan2(dist.x, dist.z) = atan (dist.x / dist.z)
				// With atan the tree inverts when the camera has the same z position
				float angle = atan2(dist.x, dist.z);

				float3x3 rotMatrix;
				float cosinus = cos(angle);
				float sinus = sin(angle);

				// Rotation matrix in Y
				rotMatrix[0].xyz = float3(cosinus, 0, sinus);
				rotMatrix[1].xyz = float3(0, 1, 0);
				rotMatrix[2].xyz = float3(-sinus, 0, cosinus);

				// The position of the vertex after the rotation
				float4 newPos = float4(mul(rotMatrix, v.vertex * float4(_ScaleX, _ScaleY, 0, 0)), 1);

				// The model matrix without the rotation and scale
				float4x4 matrix_M_noRot = unity_ObjectToWorld;
				matrix_M_noRot[0][0] = 1;
				matrix_M_noRot[0][1] = 0;
				matrix_M_noRot[0][2] = 0;

				matrix_M_noRot[1][0] = 0;
				matrix_M_noRot[1][1] = 1;
				matrix_M_noRot[1][2] = 0;

				matrix_M_noRot[2][0] = 0;
				matrix_M_noRot[2][1] = 0;
				matrix_M_noRot[2][2] = 1;

	
				//UNITY_INITIALIZE_OUTPUT(VertexOutput, output);
				// The position of the vertex in clip space ignoring the rotation and scale of the object
#if IGNORE_ROTATION_AND_SCALE
				o.pos = mul(UNITY_MATRIX_VP, mul(matrix_M_noRot, newPos));
#else
				o.pos = mul(UNITY_MATRIX_VP, mul(unity_ObjectToWorld, newPos));
#endif

				o.uv0 = TRANSFORM_TEX(v.texcoord0, _Mask);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
				float isFrontFace = (facing >= 0 ? 1 : 0);
					float faceSign = (facing >= 0 ? 1 : -1);
					float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
					float3 normalDirection = i.normalDir;
					float3 viewReflectDirection = reflect(-viewDirection, normalDirection);
					float sceneZ = max(0,LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)))) - _ProjectionParams.g);
					float partZ = max(0,i.projPos.z - _ProjectionParams.g);
					float4 _Mask_var = tex2D(_Mask,TRANSFORM_TEX(i.uv0, _Mask));
					float4 node_4292 = _Time;
					float2 node_4452 = (i.uv0*float2(1.5,1));
					float4 node_7955 = _Time;
					float2 node_2504 = (((node_4452*_Noise1Scale) + node_4292.g*float2(1,1)) + (float2(_NonUsage2,_Noise1Speed)*node_7955.g));
					float4 node_1839 = tex2D(_NoiseMap,TRANSFORM_TEX(node_2504, _NoiseMap));
					float4 node_2518 = _Time;
					float2 node_773 = (((node_4452*_Noise2Scale) + node_4292.g*float2(1,1)) + (float2(_NonUsage,_Noise2Speed)*node_2518.g));
					float4 node_3064 = tex2D(_NoiseMap,TRANSFORM_TEX(node_773, _NoiseMap));
					float node_8085 = (_Mask_var.r*(_Mask_var.r + (node_1839.r*node_3064.r)));
					float node_7807 = step(_OuterFireBlack,node_8085);
					clip(step(_OpacityCutoff,node_7807) - 0.5);
					////// Lighting:
					////// Emissive:
									float node_7920 = node_8085;
									float node_1447 = smoothstep(_NonUsage3, _OuterColorBlend, i.uv0.g);
									float node_7035 = step(_InnerFireWhite,node_8085);
									float3 emissive = (saturate(((1.0 - _ShadingAmount) + node_7920))*((((_OuterColor.rgb*(1.0 - node_1447)) + (node_1447*_OuterColorTop.rgb))*(1.0 - node_7035)) + (_InnerColor.rgb*node_7035))*_Brightness);
									float3 finalColor = emissive;
									return fixed4(finalColor,(saturate((_PremultiplyBlend + node_7807))*saturate((sceneZ - partZ) / _DepthOpacity)));
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
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _NoiseMap; uniform float4 _NoiseMap_ST;
            uniform float _Noise1Scale;
            uniform float _Noise2Scale;
            uniform float _NonUsage;
            uniform float _NonUsage2;
            uniform sampler2D _Mask; uniform float4 _Mask_ST;
            uniform float _OuterFireBlack;
            uniform float _Noise2Speed;
            uniform float _Noise1Speed;
            uniform float _OpacityCutoff;
            uniform float _PushForward;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
                float4 posWorld : TEXCOORD2;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                v.vertex.xyz += (_PushForward*normalize((_WorldSpaceCameraPos-mul(unity_ObjectToWorld, v.vertex).rgb)));
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float4x4 bbmv = UNITY_MATRIX_MV;
                bbmv._m00 = -1.0/length(unity_WorldToObject[0].xyz);
                bbmv._m10 = 0.0f;
                bbmv._m20 = 0.0f;
                bbmv._m01 = 0.0f;
                bbmv._m11 = -1.0/length(unity_WorldToObject[1].xyz);
                bbmv._m21 = 0.0f;
                bbmv._m02 = 0.0f;
                bbmv._m12 = 0.0f;
                bbmv._m22 = -1.0/length(unity_WorldToObject[2].xyz);
                o.pos = mul( UNITY_MATRIX_P, mul( bbmv, v.vertex ));
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float4 _Mask_var = tex2D(_Mask,TRANSFORM_TEX(i.uv0, _Mask));
                float4 node_8441 = _Time;
                float2 node_4452 = (i.uv0*float2(1.5,1));
                float4 node_7955 = _Time;
                float2 node_2504 = (((node_4452*_Noise1Scale)+node_8441.g*float2(1,1))+(float2(_NonUsage2,_Noise1Speed)*node_7955.g));
                float4 node_1839 = tex2D(_NoiseMap,TRANSFORM_TEX(node_2504, _NoiseMap));
                float4 node_2518 = _Time;
                float2 node_773 = (((node_4452*_Noise2Scale)+node_8441.g*float2(1,1))+(float2(_NonUsage,_Noise2Speed)*node_2518.g));
                float4 node_3064 = tex2D(_NoiseMap,TRANSFORM_TEX(node_773, _NoiseMap));
                float node_8085 = (_Mask_var.r*(_Mask_var.r+(node_1839.r*node_3064.r)));
                float node_7807 = step(_OuterFireBlack,node_8085);
                clip(step(_OpacityCutoff,node_7807) - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
