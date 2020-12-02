// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "LavaPoolSprite"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
		[HDR]_Color4("Color 4", Color) = (4,1.129412,0,1)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}

	SubShader
	{
		LOD 0

		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha
		
		
		Pass
		{
		CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"


			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				
			};
			
			uniform fixed4 _Color;
			uniform float _EnableExternalAlpha;
			uniform sampler2D _MainTex;
			uniform sampler2D _AlphaTex;
			uniform float4 _Color4;
			uniform float4 _MainTex_ST;
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
			

			
			v2f vert( appdata_t IN  )
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
				
				
				IN.vertex.xyz +=  float3(0,0,0) ; 
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

#if ETC1_EXTERNAL_ALPHA
				// get the color from an external texture (usecase: Alpha support for ETC1 on android)
				fixed4 alpha = tex2D (_AlphaTex, uv);
				color.a = lerp (color.a, alpha.r, _EnableExternalAlpha);
#endif //ETC1_EXTERNAL_ALPHA

				return color;
			}
			
			fixed4 frag(v2f IN  ) : SV_Target
			{
				float2 texCoord19 = IN.texcoord.xy * float2( 0.8,1.27 ) + float2( 0,0 );
				float simplePerlin2D15 = snoise( texCoord19*4.36 );
				simplePerlin2D15 = simplePerlin2D15*0.5 + 0.5;
				float4 transform29 = mul(unity_ObjectToWorld,float4( 0,0,0,1 ));
				float2 uv_MainTex = IN.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float4 temp_output_1_0_g1 = tex2D( _MainTex, uv_MainTex );
				float4 color7 = IsGammaSpace() ? float4(0,0,0,1) : float4(0,0,0,1);
				float4 temp_output_2_0_g1 = color7;
				float temp_output_11_0_g1 = distance( temp_output_1_0_g1 , temp_output_2_0_g1 );
				float4 lerpResult21_g1 = lerp( ( simplePerlin2D15 * ( ( ( sin( ( transform29 + _Time.y ) ) * _Color4 ) / 2.0 ) + ( _Color4 * 2.0 ) ) ) , temp_output_1_0_g1 , saturate( ( ( temp_output_11_0_g1 - 999999.0 ) / max( 0.1 , 1E-05 ) ) ));
				
				fixed4 c = lerpResult21_g1;
				c.rgb *= c.a;
				return c;
			}
		ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18707
612;463;1607;916;1515.434;656.8654;1.716061;True;False
Node;AmplifyShaderEditor.SimpleTimeNode;24;-653.0186,704.4739;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ObjectToWorldTransfNode;29;-935.9511,699.7788;Inherit;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;26;-384.0186,728.4739;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SinOpNode;23;-347.0186,613.4739;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ColorNode;8;-600.744,353.839;Inherit;False;Property;_Color4;Color 4;0;1;[HDR];Create;True;0;0;False;0;False;4,1.129412,0,1;1.335078,0.3774567,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;28;-134.9679,608.7878;Inherit;False;Constant;_Float4;Float 4;1;0;Create;True;0;0;False;0;False;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-81.91008,347.5175;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-209.8971,541.2883;Inherit;False;Constant;_Float0;Float 0;1;0;Create;True;0;0;False;0;False;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-155.806,235.4434;Inherit;False;Constant;_Float1;Float 1;1;0;Create;True;0;0;False;0;False;4.36;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;27;52.33215,379.7879;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;19;-251.288,137.2836;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.8,1.27;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-17.02492,579.5408;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;15;29.06614,121.3246;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;12;190.9687,326.8395;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;3;-504.5,-259;Inherit;False;0;0;_MainTex;Shader;False;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;9;20.47107,76.66161;Inherit;False;Constant;_Float3;Float 3;0;0;Create;True;0;0;False;0;False;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;4;-296.5,-236;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;22;157.885,10.05367;Inherit;False;Constant;_Float2;Float 2;1;0;Create;True;0;0;False;0;False;999999;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;7;-291.1553,-25.30695;Inherit;False;Constant;_Color3;Color 3;0;0;Create;True;0;0;False;0;False;0,0,0,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;365.9268,224.828;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.FunctionNode;6;464.8484,18.9544;Inherit;False;Replace Color;-1;;1;896dccb3016c847439def376a728b869;1,12,0;5;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;21;-569.4129,206.0867;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;30;-369.2816,203.9152;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.FunctionNode;31;-618.8807,279.3156;Inherit;False;Random Range;-1;;2;7b754edb8aebbfb4a9ace907af661cfc;0;3;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT;500;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinTimeNode;10;-577.1007,552.8088;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1;681.1772,-31.76929;Float;False;True;-1;2;ASEMaterialInspector;0;8;LavaPoolSprite;0f8ba0101102bb14ebf021ddadce9b49;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;2;True;3;1;False;-1;10;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;True;2;False;-1;False;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;26;0;29;0
WireConnection;26;1;24;0
WireConnection;23;0;26;0
WireConnection;11;0;23;0
WireConnection;11;1;8;0
WireConnection;27;0;11;0
WireConnection;27;1;28;0
WireConnection;13;0;8;0
WireConnection;13;1;14;0
WireConnection;15;0;19;0
WireConnection;15;1;16;0
WireConnection;12;0;27;0
WireConnection;12;1;13;0
WireConnection;4;0;3;0
WireConnection;20;0;15;0
WireConnection;20;1;12;0
WireConnection;6;1;4;0
WireConnection;6;2;7;0
WireConnection;6;3;20;0
WireConnection;6;4;22;0
WireConnection;6;5;9;0
WireConnection;21;0;10;1
WireConnection;21;1;10;2
WireConnection;30;0;21;0
WireConnection;30;1;31;0
WireConnection;31;1;29;0
WireConnection;1;0;6;0
ASEEND*/
//CHKSM=C4A6F0E6D19AB3E4A9F8F54EAE154E309877E0B8