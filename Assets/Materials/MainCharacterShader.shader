// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "MainCharacterShader"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
		[HDR]_SwordColor("SwordColor", Color) = (5.552941,8,4.705883,1)
		[HDR]_ScreenColor("ScreenColor", Color) = (0,0.9568627,1.364706,1)
		[HDR]_Color6("Color 6", Color) = (0.8235294,1,0.772549,1)
		_getHit("getHit", Float) = 0
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
			uniform float _getHit;
			uniform float4 _Color6;
			uniform float4 _ScreenColor;
			uniform float4 _SwordColor;
			uniform float4 _MainTex_ST;
			struct Gradient
			{
				int type;
				int colorsLength;
				int alphasLength;
				float4 colors[8];
				float2 alphas[8];
			};
			
			Gradient NewGradient(int type, int colorsLength, int alphasLength, 
			float4 colors0, float4 colors1, float4 colors2, float4 colors3, float4 colors4, float4 colors5, float4 colors6, float4 colors7,
			float2 alphas0, float2 alphas1, float2 alphas2, float2 alphas3, float2 alphas4, float2 alphas5, float2 alphas6, float2 alphas7)
			{
				Gradient g;
				g.type = type;
				g.colorsLength = colorsLength;
				g.alphasLength = alphasLength;
				g.colors[ 0 ] = colors0;
				g.colors[ 1 ] = colors1;
				g.colors[ 2 ] = colors2;
				g.colors[ 3 ] = colors3;
				g.colors[ 4 ] = colors4;
				g.colors[ 5 ] = colors5;
				g.colors[ 6 ] = colors6;
				g.colors[ 7 ] = colors7;
				g.alphas[ 0 ] = alphas0;
				g.alphas[ 1 ] = alphas1;
				g.alphas[ 2 ] = alphas2;
				g.alphas[ 3 ] = alphas3;
				g.alphas[ 4 ] = alphas4;
				g.alphas[ 5 ] = alphas5;
				g.alphas[ 6 ] = alphas6;
				g.alphas[ 7 ] = alphas7;
				return g;
			}
			
			float4 SampleGradient( Gradient gradient, float time )
			{
				float3 color = gradient.colors[0].rgb;
				UNITY_UNROLL
				for (int c = 1; c < 8; c++)
				{
				float colorPos = saturate((time - gradient.colors[c-1].w) / ( 0.00001 + (gradient.colors[c].w - gradient.colors[c-1].w)) * step(c, (float)gradient.colorsLength-1));
				color = lerp(color, gradient.colors[c].rgb, lerp(colorPos, step(0.01, colorPos), gradient.type));
				}
				#ifndef UNITY_COLORSPACE_GAMMA
				color = half3(GammaToLinearSpaceExact(color.r), GammaToLinearSpaceExact(color.g), GammaToLinearSpaceExact(color.b));
				#endif
				float alpha = gradient.alphas[0].x;
				UNITY_UNROLL
				for (int a = 1; a < 8; a++)
				{
				float alphaPos = saturate((time - gradient.alphas[a-1].y) / ( 0.00001 + (gradient.alphas[a].y - gradient.alphas[a-1].y)) * step(a, (float)gradient.alphasLength-1));
				alpha = lerp(alpha, gradient.alphas[a].x, lerp(alphaPos, step(0.01, alphaPos), gradient.type));
				}
				return float4(color, alpha);
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
				float2 uv_MainTex = IN.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float4 tex2DNode8 = tex2D( _MainTex, uv_MainTex );
				float4 temp_output_1_0_g7 = tex2DNode8;
				float4 color10 = IsGammaSpace() ? float4(0.6941177,1,0.5882353,1) : float4(0.4396573,1,0.3049874,1);
				float4 temp_output_2_0_g7 = color10;
				float temp_output_11_0_g7 = distance( temp_output_1_0_g7 , temp_output_2_0_g7 );
				float4 lerpResult21_g7 = lerp( _SwordColor , temp_output_1_0_g7 , saturate( ( ( temp_output_11_0_g7 - 0.02 ) / max( 0.0 , 1E-05 ) ) ));
				float4 temp_output_1_0_g8 = lerpResult21_g7;
				float4 color14 = IsGammaSpace() ? float4(0,0.6313726,0.9019608,1) : float4(0,0.3564003,0.791298,1);
				float4 temp_output_2_0_g8 = color14;
				float temp_output_11_0_g8 = distance( temp_output_1_0_g8 , temp_output_2_0_g8 );
				float4 lerpResult21_g8 = lerp( _ScreenColor , temp_output_1_0_g8 , saturate( ( ( temp_output_11_0_g8 - 0.02 ) / max( 0.0 , 1E-05 ) ) ));
				float4 temp_output_1_0_g9 = lerpResult21_g8;
				float4 color17 = IsGammaSpace() ? float4(0.8235294,1,0.772549,1) : float4(0.6444798,1,0.5583404,1);
				float4 temp_output_2_0_g9 = color17;
				float temp_output_11_0_g9 = distance( temp_output_1_0_g9 , temp_output_2_0_g9 );
				float4 lerpResult21_g9 = lerp( _Color6 , temp_output_1_0_g9 , saturate( ( ( temp_output_11_0_g9 - 0.02 ) / max( 0.0 , 1E-05 ) ) ));
				float4 temp_output_16_0 = lerpResult21_g9;
				Gradient gradient23 = NewGradient( 1, 3, 2, float4( 0.6698113, 0.6698113, 0.6698113, 0.3000076 ), float4( 0.4433962, 0.4433962, 0.4433962, 0.682353 ), float4( 0.1320755, 0.1320755, 0.1320755, 1 ), 0, 0, 0, 0, 0, float2( 1, 0 ), float2( 1, 1 ), 0, 0, 0, 0, 0, 0 );
				float mulTime21 = _Time.y * 15.0;
				float2 temp_cast_0 = (round( mulTime21 )).xx;
				float dotResult4_g6 = dot( temp_cast_0 , float2( 12.9898,78.233 ) );
				float lerpResult10_g6 = lerp( 0.0 , 1.0 , frac( ( sin( dotResult4_g6 ) * 43758.55 ) ));
				float4 ifLocalVar19 = 0;
				if( _getHit == 1.0 )
				ifLocalVar19 = ( tex2DNode8 * SampleGradient( gradient23, lerpResult10_g6 ) );
				else
				ifLocalVar19 = temp_output_16_0;
				
				fixed4 c = ifLocalVar19;
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
681;271;1607;934;1833.151;437.2515;1;True;False
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;7;-1234.253,-741.8087;Inherit;False;0;0;_MainTex;Shader;False;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;21;-1336.711,176.5531;Inherit;False;1;0;FLOAT;15;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;10;-946.2775,-528.8153;Inherit;False;Constant;_Color3;Color 3;0;0;Create;True;0;0;False;0;False;0.6941177,1,0.5882353,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RoundOpNode;26;-1138.801,177.4283;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;8;-997.2535,-732.8087;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;11;-684.2535,-264.8089;Inherit;False;Constant;_Float3;Float 3;0;0;Create;True;0;0;False;0;False;0.02;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;12;-945.7759,-342.8295;Inherit;False;Property;_SwordColor;SwordColor;0;1;[HDR];Create;True;0;0;False;0;False;5.552941,8,4.705883,1;0.7128872,1.471698,0.4512282,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;27;-854.8845,178.8832;Inherit;True;Random Range;-1;;6;7b754edb8aebbfb4a9ace907af661cfc;0;3;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;9;-423.2527,-632.8086;Inherit;False;Replace Color;-1;;7;896dccb3016c847439def376a728b869;1,12,0;5;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;14;-434.5912,-455.2823;Inherit;False;Constant;_Color5;Color 5;1;0;Create;True;0;0;False;0;False;0,0.6313726,0.9019608,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GradientNode;23;-1041.514,60.80238;Inherit;False;1;3;2;0.6698113,0.6698113,0.6698113,0.3000076;0.4433962,0.4433962,0.4433962,0.682353;0.1320755,0.1320755,0.1320755,1;1,0;1,1;0;1;OBJECT;0
Node;AmplifyShaderEditor.ColorNode;15;-435.4097,-238.4543;Inherit;False;Property;_ScreenColor;ScreenColor;1;1;[HDR];Create;True;0;0;False;0;False;0,0.9568627,1.364706,1;0,1.486311,2.118547,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GradientSampleNode;25;-574.0309,88.23202;Inherit;True;2;0;OBJECT;;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;17;-64.16102,-230.6977;Inherit;False;Constant;_Color4;Color 4;2;0;Create;True;0;0;False;0;False;0.8235294,1,0.772549,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;18;-68.05656,-24.82903;Inherit;False;Property;_Color6;Color 6;2;1;[HDR];Create;True;0;0;False;0;False;0.8235294,1,0.772549,1;1.008636,1.709594,0.798348,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;13;-26.31236,-446.8593;Inherit;False;Replace Color;-1;;8;896dccb3016c847439def376a728b869;1,12,0;5;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;16;253.681,-372.0351;Inherit;False;Replace Color;-1;;9;896dccb3016c847439def376a728b869;1,12,0;5;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-180.9964,153.5155;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;20;238.8646,67.53592;Inherit;False;Property;_getHit;getHit;3;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;19;522.4771,71.09698;Inherit;False;False;5;0;FLOAT;0;False;1;FLOAT;1;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;6;811.9833,-11.62056;Float;False;True;-1;2;ASEMaterialInspector;0;8;MainCharacterShader;0f8ba0101102bb14ebf021ddadce9b49;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;2;True;3;1;False;-1;10;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;True;2;False;-1;False;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;26;0;21;0
WireConnection;8;0;7;0
WireConnection;27;1;26;0
WireConnection;9;1;8;0
WireConnection;9;2;10;0
WireConnection;9;3;12;0
WireConnection;9;4;11;0
WireConnection;25;0;23;0
WireConnection;25;1;27;0
WireConnection;13;1;9;0
WireConnection;13;2;14;0
WireConnection;13;3;15;0
WireConnection;13;4;11;0
WireConnection;16;1;13;0
WireConnection;16;2;17;0
WireConnection;16;3;18;0
WireConnection;16;4;11;0
WireConnection;28;0;8;0
WireConnection;28;1;25;0
WireConnection;19;0;20;0
WireConnection;19;2;16;0
WireConnection;19;3;28;0
WireConnection;19;4;16;0
WireConnection;6;0;19;0
ASEEND*/
//CHKSM=AE0DCA034955745B1DAC0F64D4DDAAEE09622F9C