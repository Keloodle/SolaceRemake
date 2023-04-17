// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "James/toonFire"
{
	Properties
	{
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		[ASEBegin]_fireDetail("fireDetail", Range( 0 , 20)) = 10
		_Speed("Speed", Float) = -0.15
		_Tiling("Tiling", Vector) = (1,2,0,0)
		_Offset("Offset", Vector) = (0,0,0,0)
		_speedUp("speedUp", Range( -0.3 , 0)) = -0.14
		_firePower("firePower", Range( 0 , 1.5)) = 0.5
		_fireTreshold("fireTreshold", Range( -1 , 1)) = -0.5
		_noiseTiling("noiseTiling", Range( 0 , 1)) = 0.25
		[HDR]_Colourtop("Colour top", Color) = (1,0.4902101,0.3443396,1)
		[HDR]_Colourbottom("Colour bottom", Color) = (0.9475898,1,0.2971698,1)
		_MainTex("MainTex", 2D) = "white" {}
		[ASEEnd]_minomaxnminomaxn("min o, max n, min o, max n", Vector) = (0.5,1.2,0,1)

	}

	SubShader
	{
		LOD 0

		

		Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Transparent" "Queue"="Transparent" }

		Cull Off
		HLSLINCLUDE
		#pragma target 2.0
		ENDHLSL

		
		Pass
		{
			Name "Sprite Lit"
			Tags { "LightMode"="Universal2D" }
			
			Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
			ZTest LEqual
			ZWrite Off
			Offset 0 , 0
			ColorMask RGBA
			

			HLSLPROGRAM
			#define ASE_SRP_VERSION 999999

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x 

			#pragma vertex vert
			#pragma fragment frag

			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
			#pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_0
			#pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_1
			#pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_2
			#pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_3

			#define _SURFACE_TYPE_TRANSPARENT 1
			#define SHADERPASS_SPRITELIT

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/LightingUtility.hlsl"
			
			#if USE_SHAPE_LIGHT_TYPE_0
			SHAPE_LIGHT(0)
			#endif

			#if USE_SHAPE_LIGHT_TYPE_1
			SHAPE_LIGHT(1)
			#endif

			#if USE_SHAPE_LIGHT_TYPE_2
			SHAPE_LIGHT(2)
			#endif

			#if USE_SHAPE_LIGHT_TYPE_3
			SHAPE_LIGHT(3)
			#endif

			#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/CombinedShapeLightShared.hlsl"

			

			sampler2D _MainTex;
			CBUFFER_START( UnityPerMaterial )
			float4 _Colourtop;
			float4 _Colourbottom;
			float4 _minomaxnminomaxn;
			float2 _Tiling;
			float2 _Offset;
			float _Speed;
			float _fireDetail;
			float _noiseTiling;
			float _speedUp;
			float _firePower;
			float _fireTreshold;
			CBUFFER_END


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float4 uv0 : TEXCOORD0;
				float4 color : COLOR;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				float4 texCoord0 : TEXCOORD0;
				float4 color : TEXCOORD1;
				float4 screenPosition : TEXCOORD2;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			#if ETC1_EXTERNAL_ALPHA
				TEXTURE2D(_AlphaTex); SAMPLER(sampler_AlphaTex);
				float _EnableAlphaTexture;
			#endif

			float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }
			float snoise( float2 v )
			{
				const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
				float2 i = floor( v + dot( v, C.yy ) );
				float2 x0 = v - i + dot( i, C.xx );
				float2 i1;
				i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
				float4 x12 = x0.xyxy + C.xxzz;
				x12.xy -= i1;
				i = mod2D289( i );
				float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
				float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
				m = m * m;
				m = m * m;
				float3 x = 2.0 * frac( p * C.www ) - 1.0;
				float3 h = abs( x ) - 0.5;
				float3 ox = floor( x + 0.5 );
				float3 a0 = x - ox;
				m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
				float3 g;
				g.x = a0.x * x0.x + h.x * x0.y;
				g.yz = a0.yz * x12.xz + h.yz * x12.yw;
				return 130.0 * dot( m, g );
			}
			
					float2 voronoihash2( float2 p )
					{
						
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi2( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
					{
						float2 n = floor( v );
						float2 f = frac( v );
						float F1 = 8.0;
						float F2 = 8.0; float2 mg = 0;
						for ( int j = -1; j <= 1; j++ )
						{
							for ( int i = -1; i <= 1; i++ )
						 	{
						 		float2 g = float2( i, j );
						 		float2 o = voronoihash2( n + g );
								o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
								float d = 0.5 * dot( r, r );
						 		if( d<F1 ) {
						 			F2 = F1;
						 			F1 = d; mg = g; mr = r; id = o;
						 		} else if( d<F2 ) {
						 			F2 = d;
						
						 		}
						 	}
						}
						return F1;
					}
			

			VertexOutput vert ( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif
				v.normal = v.normal;
				v.tangent.xyz = v.tangent.xyz;

				VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);

				o.texCoord0 = v.uv0;
				o.color = v.color;
				o.clipPos = vertexInput.positionCS;
				o.screenPosition = ComputeScreenPos( o.clipPos, _ProjectionParams.x );
				return o;
			}

			half4 frag ( VertexOutput IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				float2 texCoord5 = IN.texCoord0.xy * _Tiling + _Offset;
				float2 temp_cast_0 = (( _Speed * _TimeParameters.x )).xx;
				float2 texCoord14 = IN.texCoord0.xy * float2( 1,1 ) + temp_cast_0;
				float simplePerlin2D1 = snoise( texCoord14*_fireDetail );
				simplePerlin2D1 = simplePerlin2D1*0.5 + 0.5;
				float time2 = 2.0;
				float2 voronoiSmoothId2 = 0;
				float2 temp_cast_1 = (_noiseTiling).xx;
				float4 appendResult30 = (float4(0.0 , ( _TimeParameters.x * _speedUp ) , 0.0 , 0.0));
				float2 texCoord21 = IN.texCoord0.xy * temp_cast_1 + appendResult30.xy;
				float2 temp_cast_3 = (_noiseTiling).xx;
				float2 texCoord27 = IN.texCoord0.xy * temp_cast_3 + appendResult30.xy;
				float simplePerlin2D22 = snoise( texCoord27*_fireDetail );
				simplePerlin2D22 = simplePerlin2D22*0.5 + 0.5;
				float2 temp_cast_5 = (simplePerlin2D22).xx;
				float2 lerpResult23 = lerp( texCoord21 , temp_cast_5 , float2( 0.5,0.5 ));
				float2 coords2 = lerpResult23 * _fireDetail;
				float2 id2 = 0;
				float2 uv2 = 0;
				float voroi2 = voronoi2( coords2, time2, id2, uv2, 0, voronoiSmoothId2 );
				float2 temp_cast_6 = (( simplePerlin2D1 * voroi2 )).xx;
				float4 appendResult33 = (float4(0.0 , _firePower , 0.0 , 0.0));
				float2 lerpResult4 = lerp( texCoord5 , temp_cast_6 , appendResult33.xy);
				float4 tex2DNode179 = tex2D( _MainTex, lerpResult4 );
				float grayscale100 = Luminance(( tex2DNode179 * tex2DNode179.a ).rgb);
				float fireOutput47 = grayscale100;
				float smoothstepResult99 = smoothstep( 0.0 , 1.0 , (_minomaxnminomaxn.z + (fireOutput47 - _minomaxnminomaxn.x) * (_minomaxnminomaxn.w - _minomaxnminomaxn.z) / (_minomaxnminomaxn.y - _minomaxnminomaxn.x)));
				float4 lerpResult56 = lerp( _Colourtop , _Colourbottom , smoothstepResult99);
				float4 dualColour116 = lerpResult56;
				float2 texCoord36 = IN.texCoord0.xy * float2( 1,1 ) + float2( 0,0 );
				float clampResult44 = clamp( ( ( ( 1.0 - sqrt( texCoord36 ).y ) + _fireTreshold ) * 2.0 ) , 0.0 , 1.0 );
				float clampResult49 = clamp( ( fireOutput47 - clampResult44 ) , 0.0 , 1.0 );
				float fire68 = clampResult49;
				float2 texCoord118 = IN.texCoord0.xy * float2( 1,2 ) + float2( 0,-1 );
				float clampResult119 = clamp( texCoord118.y , 0.0 , 1.0 );
				
				float4 Color = ( ( dualColour116 * fire68 * 1.5 ) * ( ( 1.0 - clampResult119 ) * round( pow( fire68 , 0.15 ) ) ) );
				float Mask = 1;
				float3 Normal = float3( 0, 0, 1 );

				#if ETC1_EXTERNAL_ALPHA
					float4 alpha = SAMPLE_TEXTURE2D(_AlphaTex, sampler_AlphaTex, IN.texCoord0.xy);
					Color.a = lerp ( Color.a, alpha.r, _EnableAlphaTexture);
				#endif
				
				Color *= IN.color;

				return CombinedShapeLightShared( Color, Mask, IN.screenPosition.xy / IN.screenPosition.w );
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "Sprite Normal"
			Tags { "LightMode"="NormalsRendering" }
			
			Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
			ZTest LEqual
			ZWrite Off
			Offset 0 , 0
			ColorMask RGBA
			

			HLSLPROGRAM
			#define ASE_SRP_VERSION 999999

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x 

			#pragma vertex vert
			#pragma fragment frag

			#define _SURFACE_TYPE_TRANSPARENT 1
			#define SHADERPASS_SPRITENORMAL

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/NormalsRenderingShared.hlsl"
			
			

			sampler2D _MainTex;
			CBUFFER_START( UnityPerMaterial )
			float4 _Colourtop;
			float4 _Colourbottom;
			float4 _minomaxnminomaxn;
			float2 _Tiling;
			float2 _Offset;
			float _Speed;
			float _fireDetail;
			float _noiseTiling;
			float _speedUp;
			float _firePower;
			float _fireTreshold;
			CBUFFER_END


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float4 uv0 : TEXCOORD0;
				float4 color : COLOR;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				float4 texCoord0 : TEXCOORD0;
				float4 color : TEXCOORD1;
				float3 normalWS : TEXCOORD2;
				float4 tangentWS : TEXCOORD3;
				float3 bitangentWS : TEXCOORD4;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }
			float snoise( float2 v )
			{
				const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
				float2 i = floor( v + dot( v, C.yy ) );
				float2 x0 = v - i + dot( i, C.xx );
				float2 i1;
				i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
				float4 x12 = x0.xyxy + C.xxzz;
				x12.xy -= i1;
				i = mod2D289( i );
				float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
				float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
				m = m * m;
				m = m * m;
				float3 x = 2.0 * frac( p * C.www ) - 1.0;
				float3 h = abs( x ) - 0.5;
				float3 ox = floor( x + 0.5 );
				float3 a0 = x - ox;
				m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
				float3 g;
				g.x = a0.x * x0.x + h.x * x0.y;
				g.yz = a0.yz * x12.xz + h.yz * x12.yw;
				return 130.0 * dot( m, g );
			}
			
					float2 voronoihash2( float2 p )
					{
						
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi2( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
					{
						float2 n = floor( v );
						float2 f = frac( v );
						float F1 = 8.0;
						float F2 = 8.0; float2 mg = 0;
						for ( int j = -1; j <= 1; j++ )
						{
							for ( int i = -1; i <= 1; i++ )
						 	{
						 		float2 g = float2( i, j );
						 		float2 o = voronoihash2( n + g );
								o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
								float d = 0.5 * dot( r, r );
						 		if( d<F1 ) {
						 			F2 = F1;
						 			F1 = d; mg = g; mr = r; id = o;
						 		} else if( d<F2 ) {
						 			F2 = d;
						
						 		}
						 	}
						}
						return F1;
					}
			

			VertexOutput vert ( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif
				v.normal = v.normal;
				v.tangent.xyz = v.tangent.xyz;

				VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);

				o.texCoord0 = v.uv0;
				o.color = v.color;
				o.clipPos = vertexInput.positionCS;

				float3 normalWS = TransformObjectToWorldNormal( v.normal );
				o.normalWS = NormalizeNormalPerVertex( normalWS );
				float4 tangentWS = float4( TransformObjectToWorldDir( v.tangent.xyz ), v.tangent.w );
				o.tangentWS = normalize( tangentWS );
				o.bitangentWS = cross( normalWS, tangentWS.xyz ) * tangentWS.w;
				return o;
			}

			half4 frag ( VertexOutput IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				float2 texCoord5 = IN.texCoord0.xy * _Tiling + _Offset;
				float2 temp_cast_0 = (( _Speed * _TimeParameters.x )).xx;
				float2 texCoord14 = IN.texCoord0.xy * float2( 1,1 ) + temp_cast_0;
				float simplePerlin2D1 = snoise( texCoord14*_fireDetail );
				simplePerlin2D1 = simplePerlin2D1*0.5 + 0.5;
				float time2 = 2.0;
				float2 voronoiSmoothId2 = 0;
				float2 temp_cast_1 = (_noiseTiling).xx;
				float4 appendResult30 = (float4(0.0 , ( _TimeParameters.x * _speedUp ) , 0.0 , 0.0));
				float2 texCoord21 = IN.texCoord0.xy * temp_cast_1 + appendResult30.xy;
				float2 temp_cast_3 = (_noiseTiling).xx;
				float2 texCoord27 = IN.texCoord0.xy * temp_cast_3 + appendResult30.xy;
				float simplePerlin2D22 = snoise( texCoord27*_fireDetail );
				simplePerlin2D22 = simplePerlin2D22*0.5 + 0.5;
				float2 temp_cast_5 = (simplePerlin2D22).xx;
				float2 lerpResult23 = lerp( texCoord21 , temp_cast_5 , float2( 0.5,0.5 ));
				float2 coords2 = lerpResult23 * _fireDetail;
				float2 id2 = 0;
				float2 uv2 = 0;
				float voroi2 = voronoi2( coords2, time2, id2, uv2, 0, voronoiSmoothId2 );
				float2 temp_cast_6 = (( simplePerlin2D1 * voroi2 )).xx;
				float4 appendResult33 = (float4(0.0 , _firePower , 0.0 , 0.0));
				float2 lerpResult4 = lerp( texCoord5 , temp_cast_6 , appendResult33.xy);
				float4 tex2DNode179 = tex2D( _MainTex, lerpResult4 );
				float grayscale100 = Luminance(( tex2DNode179 * tex2DNode179.a ).rgb);
				float fireOutput47 = grayscale100;
				float smoothstepResult99 = smoothstep( 0.0 , 1.0 , (_minomaxnminomaxn.z + (fireOutput47 - _minomaxnminomaxn.x) * (_minomaxnminomaxn.w - _minomaxnminomaxn.z) / (_minomaxnminomaxn.y - _minomaxnminomaxn.x)));
				float4 lerpResult56 = lerp( _Colourtop , _Colourbottom , smoothstepResult99);
				float4 dualColour116 = lerpResult56;
				float2 texCoord36 = IN.texCoord0.xy * float2( 1,1 ) + float2( 0,0 );
				float clampResult44 = clamp( ( ( ( 1.0 - sqrt( texCoord36 ).y ) + _fireTreshold ) * 2.0 ) , 0.0 , 1.0 );
				float clampResult49 = clamp( ( fireOutput47 - clampResult44 ) , 0.0 , 1.0 );
				float fire68 = clampResult49;
				float2 texCoord118 = IN.texCoord0.xy * float2( 1,2 ) + float2( 0,-1 );
				float clampResult119 = clamp( texCoord118.y , 0.0 , 1.0 );
				
				float4 Color = ( ( dualColour116 * fire68 * 1.5 ) * ( ( 1.0 - clampResult119 ) * round( pow( fire68 , 0.15 ) ) ) );
				float3 Normal = float3( 0, 0, 1 );
				
				Color *= IN.color;

				return NormalsRenderingShared( Color, Normal, IN.tangentWS.xyz, IN.bitangentWS, IN.normalWS);
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "Sprite Forward"
			Tags { "LightMode"="UniversalForward" }

			Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
			ZTest LEqual
			ZWrite Off
			Offset 0 , 0
			ColorMask RGBA
			

			HLSLPROGRAM
			#define ASE_SRP_VERSION 999999

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x 

			#pragma vertex vert
			#pragma fragment frag

			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA

			#define _SURFACE_TYPE_TRANSPARENT 1
			#define SHADERPASS_SPRITEFORWARD

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

			

			sampler2D _MainTex;
			CBUFFER_START( UnityPerMaterial )
			float4 _Colourtop;
			float4 _Colourbottom;
			float4 _minomaxnminomaxn;
			float2 _Tiling;
			float2 _Offset;
			float _Speed;
			float _fireDetail;
			float _noiseTiling;
			float _speedUp;
			float _firePower;
			float _fireTreshold;
			CBUFFER_END


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float4 uv0 : TEXCOORD0;
				float4 color : COLOR;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				float4 texCoord0 : TEXCOORD0;
				float4 color : TEXCOORD1;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			#if ETC1_EXTERNAL_ALPHA
				TEXTURE2D( _AlphaTex ); SAMPLER( sampler_AlphaTex );
				float _EnableAlphaTexture;
			#endif

			float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }
			float snoise( float2 v )
			{
				const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
				float2 i = floor( v + dot( v, C.yy ) );
				float2 x0 = v - i + dot( i, C.xx );
				float2 i1;
				i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
				float4 x12 = x0.xyxy + C.xxzz;
				x12.xy -= i1;
				i = mod2D289( i );
				float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
				float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
				m = m * m;
				m = m * m;
				float3 x = 2.0 * frac( p * C.www ) - 1.0;
				float3 h = abs( x ) - 0.5;
				float3 ox = floor( x + 0.5 );
				float3 a0 = x - ox;
				m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
				float3 g;
				g.x = a0.x * x0.x + h.x * x0.y;
				g.yz = a0.yz * x12.xz + h.yz * x12.yw;
				return 130.0 * dot( m, g );
			}
			
					float2 voronoihash2( float2 p )
					{
						
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi2( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
					{
						float2 n = floor( v );
						float2 f = frac( v );
						float F1 = 8.0;
						float F2 = 8.0; float2 mg = 0;
						for ( int j = -1; j <= 1; j++ )
						{
							for ( int i = -1; i <= 1; i++ )
						 	{
						 		float2 g = float2( i, j );
						 		float2 o = voronoihash2( n + g );
								o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
								float d = 0.5 * dot( r, r );
						 		if( d<F1 ) {
						 			F2 = F1;
						 			F1 = d; mg = g; mr = r; id = o;
						 		} else if( d<F2 ) {
						 			F2 = d;
						
						 		}
						 	}
						}
						return F1;
					}
			

			VertexOutput vert( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif
				v.normal = v.normal;

				VertexPositionInputs vertexInput = GetVertexPositionInputs( v.vertex.xyz );

				o.texCoord0 = v.uv0;
				o.color = v.color;
				o.clipPos = vertexInput.positionCS;

				return o;
			}

			half4 frag( VertexOutput IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				float2 texCoord5 = IN.texCoord0.xy * _Tiling + _Offset;
				float2 temp_cast_0 = (( _Speed * _TimeParameters.x )).xx;
				float2 texCoord14 = IN.texCoord0.xy * float2( 1,1 ) + temp_cast_0;
				float simplePerlin2D1 = snoise( texCoord14*_fireDetail );
				simplePerlin2D1 = simplePerlin2D1*0.5 + 0.5;
				float time2 = 2.0;
				float2 voronoiSmoothId2 = 0;
				float2 temp_cast_1 = (_noiseTiling).xx;
				float4 appendResult30 = (float4(0.0 , ( _TimeParameters.x * _speedUp ) , 0.0 , 0.0));
				float2 texCoord21 = IN.texCoord0.xy * temp_cast_1 + appendResult30.xy;
				float2 temp_cast_3 = (_noiseTiling).xx;
				float2 texCoord27 = IN.texCoord0.xy * temp_cast_3 + appendResult30.xy;
				float simplePerlin2D22 = snoise( texCoord27*_fireDetail );
				simplePerlin2D22 = simplePerlin2D22*0.5 + 0.5;
				float2 temp_cast_5 = (simplePerlin2D22).xx;
				float2 lerpResult23 = lerp( texCoord21 , temp_cast_5 , float2( 0.5,0.5 ));
				float2 coords2 = lerpResult23 * _fireDetail;
				float2 id2 = 0;
				float2 uv2 = 0;
				float voroi2 = voronoi2( coords2, time2, id2, uv2, 0, voronoiSmoothId2 );
				float2 temp_cast_6 = (( simplePerlin2D1 * voroi2 )).xx;
				float4 appendResult33 = (float4(0.0 , _firePower , 0.0 , 0.0));
				float2 lerpResult4 = lerp( texCoord5 , temp_cast_6 , appendResult33.xy);
				float4 tex2DNode179 = tex2D( _MainTex, lerpResult4 );
				float grayscale100 = Luminance(( tex2DNode179 * tex2DNode179.a ).rgb);
				float fireOutput47 = grayscale100;
				float smoothstepResult99 = smoothstep( 0.0 , 1.0 , (_minomaxnminomaxn.z + (fireOutput47 - _minomaxnminomaxn.x) * (_minomaxnminomaxn.w - _minomaxnminomaxn.z) / (_minomaxnminomaxn.y - _minomaxnminomaxn.x)));
				float4 lerpResult56 = lerp( _Colourtop , _Colourbottom , smoothstepResult99);
				float4 dualColour116 = lerpResult56;
				float2 texCoord36 = IN.texCoord0.xy * float2( 1,1 ) + float2( 0,0 );
				float clampResult44 = clamp( ( ( ( 1.0 - sqrt( texCoord36 ).y ) + _fireTreshold ) * 2.0 ) , 0.0 , 1.0 );
				float clampResult49 = clamp( ( fireOutput47 - clampResult44 ) , 0.0 , 1.0 );
				float fire68 = clampResult49;
				float2 texCoord118 = IN.texCoord0.xy * float2( 1,2 ) + float2( 0,-1 );
				float clampResult119 = clamp( texCoord118.y , 0.0 , 1.0 );
				
				float4 Color = ( ( dualColour116 * fire68 * 1.5 ) * ( ( 1.0 - clampResult119 ) * round( pow( fire68 , 0.15 ) ) ) );

				#if ETC1_EXTERNAL_ALPHA
					float4 alpha = SAMPLE_TEXTURE2D( _AlphaTex, sampler_AlphaTex, IN.texCoord0.xy );
					Color.a = lerp( Color.a, alpha.r, _EnableAlphaTexture );
				#endif

				Color *= IN.color;

				return Color;
			}

			ENDHLSL
		}
		
	}
	CustomEditor "UnityEditor.ShaderGraph.PBRMasterGUI"
	Fallback "Hidden/InternalErrorShader"
	
}
/*ASEBEGIN
Version=18935
1174;73;1740;771;2195.137;2463.298;1;True;False
Node;AmplifyShaderEditor.CommentaryNode;57;-4409.72,-1486.872;Inherit;False;3709.782;1871.214;Comment;30;47;7;4;5;3;33;20;2;1;19;34;23;14;17;22;21;26;18;27;30;25;9;29;31;16;32;100;101;179;198;fireEffect;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleTimeNode;16;-4323.536,-867.5835;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;32;-4352.72,-137.9089;Inherit;False;Property;_speedUp;speedUp;6;0;Create;True;0;0;0;False;0;False;-0.14;-0.5;-0.3;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;-3955.31,-240.4887;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-3898.141,-490.7683;Inherit;False;Constant;_Float0;Float 0;5;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-2868.727,-620.6727;Inherit;False;Property;_fireDetail;fireDetail;2;0;Create;True;0;0;0;False;0;False;10;10;0;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;101;-3986.041,90.81342;Inherit;False;Property;_noiseTiling;noiseTiling;9;0;Create;True;0;0;0;False;0;False;0.25;0.25;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;25;-2678.25,-386.5149;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;30;-3676.141,-250.917;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.WireNode;26;-3092.249,-369.5149;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;27;-3392.31,-94.48875;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;18;-3463.246,-1077.481;Inherit;False;Property;_Speed;Speed;3;0;Create;True;0;0;0;False;0;False;-0.15;-0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-3249.246,-893.4813;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;22;-3126.527,-213.7165;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;100;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;21;-3388.541,-376.4032;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;14;-2955.247,-983.4813;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;23;-2847.778,-300.2827;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT2;0.5,0.5;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;20;-2552.811,-1140.872;Inherit;False;Property;_Offset;Offset;5;0;Create;True;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.VoronoiNode;2;-2553.105,-545.9428;Inherit;False;0;0;1;0;1;False;1;False;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;2;False;2;FLOAT;10;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.RangedFloatNode;34;-2546.657,-248.2976;Inherit;False;Property;_firePower;firePower;7;0;Create;True;0;0;0;False;0;False;0.5;0.5;0;1.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;19;-2535.811,-1436.872;Inherit;False;Property;_Tiling;Tiling;4;0;Create;True;0;0;0;False;0;False;1,2;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.NoiseGeneratorNode;1;-2608.041,-791.2933;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;33;-2245.984,-311.0871;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-2340.225,-593.257;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;5;-2249.303,-970.3363;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;71;-583.4257,-494.9139;Inherit;False;2139.028;539.5544;Comment;12;68;49;45;44;48;42;41;40;39;38;37;36;fireFinished;1,1,1,1;0;0
Node;AmplifyShaderEditor.LerpOp;4;-2005.389,-655.3363;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;36;-533.4256,-444.9138;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;179;-1751.381,-564.9952;Inherit;True;Property;_MainTex;MainTex;13;0;Create;True;0;0;0;False;0;False;-1;8edf0137d94fb544d968a3de701aed25;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SqrtOpNode;37;-319.9535,-446.5628;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.BreakToComponentsNode;38;-156.8288,-446.3295;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;198;-1376.406,-595.9279;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;39;51.09726,-426.5092;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;40;-55.22959,-211.2454;Inherit;False;Property;_fireTreshold;fireTreshold;8;0;Create;True;0;0;0;False;0;False;-0.5;-0.5;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCGrayscale;100;-1152.799,-775.873;Inherit;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;58;-534.9711,-1589.141;Inherit;False;1609.302;859.5372;Comment;10;73;114;113;72;56;54;55;99;116;185;dualColours;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;41;237.1581,-329.9399;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;47;-958.3176,-796.1932;Inherit;False;fireOutput;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;72;-126.2742,-1530.512;Inherit;False;47;fireOutput;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;42;442.6555,-330.9546;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;113;76.9758,-1335.244;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;48;677.6411,-444.6324;Inherit;False;47;fireOutput;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;44;674.044,-246.5924;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;114;-163.0242,-1315.244;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector4Node;185;-443.9378,-1271.02;Inherit;False;Property;_minomaxnminomaxn;min o, max n, min o, max n;14;0;Create;True;0;0;0;False;0;False;0.5,1.2,0,1;0.5,1.2,0,1;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;45;902.5268,-350.8661;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;73;-140.3512,-1266.053;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0.8;False;2;FLOAT;0.8;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;49;1106.39,-352.0326;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;68;1326.689,-357.9449;Inherit;False;fire;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;55;167.3908,-1333.782;Inherit;False;Property;_Colourbottom;Colour bottom;12;1;[HDR];Create;True;0;0;0;False;0;False;0.9475898,1,0.2971698,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;54;154.7526,-1526.734;Inherit;False;Property;_Colourtop;Colour top;11;1;[HDR];Create;True;0;0;0;False;0;False;1,0.4902101,0.3443396,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;99;143.0742,-1143.258;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;56;412.5628,-1340.558;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;118;-1757.451,-2034.091;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,2;False;1;FLOAT2;0,-1;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;70;-1786.264,-1760.884;Inherit;False;68;fire;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;116;653.2205,-1346.22;Inherit;False;dualColour;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;130;-1557.69,-1749.285;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;0.15;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;119;-1531.908,-2023.942;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;204;-2057.325,-2215.399;Inherit;False;Constant;_Float2;Float 2;15;0;Create;True;0;0;0;False;0;False;1.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RoundOpNode;129;-1299.852,-1750.371;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;117;-1825.003,-2489.546;Inherit;False;116;dualColour;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;69;-1821.183,-2257.915;Inherit;False;68;fire;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;120;-1292.09,-2030.074;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;51;-1513.48,-2284.304;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;132;-1049.685,-1915.652;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;175;-2463.482,-1913.488;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;173;-985.2721,-2442.93;Inherit;False;Constant;_Float1;Float 1;13;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;7;-1664.152,-765.51;Inherit;True;Property;_Fire;Fire;1;0;Create;True;0;0;0;False;0;False;-1;8edf0137d94fb544d968a3de701aed25;ca28a6baf78616145ab7c03ffb54833e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;50;-2101.489,-2770.894;Inherit;False;Property;_Colour;Colour;10;1;[HDR];Create;True;0;0;0;False;0;False;3.1,1.1,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleDivideOpNode;177;-2374.271,-1896.378;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;176;-2441.259,-1867.938;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;174;-1986.657,-1987.123;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;178;-2165.288,-1910.82;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;202;-825.6963,-2137.328;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;200;-676.1705,-2194.752;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;1;New Amplify Shader;199187dac283dbe4a8cb1ea611d70c58;True;Sprite Normal;0;1;Sprite Normal;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;False;0;False;True;2;5;False;-1;10;False;-1;3;1;False;-1;10;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;2;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;LightMode=NormalsRendering;False;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;199;-598.1705,-2206.752;Float;False;True;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;13;James/toonFire;199187dac283dbe4a8cb1ea611d70c58;True;Sprite Lit;0;0;Sprite Lit;6;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;False;0;False;True;2;5;False;-1;10;False;-1;3;1;False;-1;10;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;2;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;LightMode=Universal2D;False;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;Hidden/InternalErrorShader;0;0;Standard;1;Vertex Position;1;0;0;3;True;True;True;False;;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;201;-676.1705,-2194.752;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;1;New Amplify Shader;199187dac283dbe4a8cb1ea611d70c58;True;Sprite Forward;0;2;Sprite Forward;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;False;0;False;True;2;5;False;-1;10;False;-1;3;1;False;-1;10;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;2;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;LightMode=UniversalForward;False;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
WireConnection;29;0;16;0
WireConnection;29;1;32;0
WireConnection;25;0;9;0
WireConnection;30;0;31;0
WireConnection;30;1;29;0
WireConnection;26;0;25;0
WireConnection;27;0;101;0
WireConnection;27;1;30;0
WireConnection;17;0;18;0
WireConnection;17;1;16;0
WireConnection;22;0;27;0
WireConnection;22;1;26;0
WireConnection;21;0;101;0
WireConnection;21;1;30;0
WireConnection;14;1;17;0
WireConnection;23;0;21;0
WireConnection;23;1;22;0
WireConnection;2;0;23;0
WireConnection;2;2;9;0
WireConnection;1;0;14;0
WireConnection;1;1;9;0
WireConnection;33;1;34;0
WireConnection;3;0;1;0
WireConnection;3;1;2;0
WireConnection;5;0;19;0
WireConnection;5;1;20;0
WireConnection;4;0;5;0
WireConnection;4;1;3;0
WireConnection;4;2;33;0
WireConnection;179;1;4;0
WireConnection;37;0;36;0
WireConnection;38;0;37;0
WireConnection;198;0;179;0
WireConnection;198;1;179;4
WireConnection;39;0;38;1
WireConnection;100;0;198;0
WireConnection;41;0;39;0
WireConnection;41;1;40;0
WireConnection;47;0;100;0
WireConnection;42;0;41;0
WireConnection;113;0;72;0
WireConnection;44;0;42;0
WireConnection;114;0;113;0
WireConnection;45;0;48;0
WireConnection;45;1;44;0
WireConnection;73;0;114;0
WireConnection;73;1;185;1
WireConnection;73;2;185;2
WireConnection;73;3;185;3
WireConnection;73;4;185;4
WireConnection;49;0;45;0
WireConnection;68;0;49;0
WireConnection;99;0;73;0
WireConnection;56;0;54;0
WireConnection;56;1;55;0
WireConnection;56;2;99;0
WireConnection;116;0;56;0
WireConnection;130;0;70;0
WireConnection;119;0;118;2
WireConnection;129;0;130;0
WireConnection;120;0;119;0
WireConnection;51;0;117;0
WireConnection;51;1;69;0
WireConnection;51;2;204;0
WireConnection;132;0;120;0
WireConnection;132;1;129;0
WireConnection;175;0;19;1
WireConnection;7;1;4;0
WireConnection;177;0;176;0
WireConnection;176;0;19;2
WireConnection;174;0;175;0
WireConnection;174;1;178;0
WireConnection;178;0;177;0
WireConnection;202;0;51;0
WireConnection;202;1;132;0
WireConnection;199;1;202;0
ASEEND*/
//CHKSM=258E0DDB5BB503608B40550D6BABC59CD2E108CA