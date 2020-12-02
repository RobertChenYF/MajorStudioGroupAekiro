// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "TankBossShader"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
		_flashAmount("flashAmount", Float) = 0
		[HDR]_FlashColor("FlashColor", Color) = (1,1,1,1)
		_Color3("Color 3", Color) = (0,0,0,0)
		[HDR]_Color4("Color 4", Color) = (0,0,0,0)
		[HDR]_Color6("Color 6", Color) = (0,0,0,0)
		_Color5("Color 5", Color) = (0,0,0,0)
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
			uniform float4 _Color6;
			uniform float4 _Color4;
			uniform float4 _MainTex_ST;
			uniform float4 _Color3;
			uniform float4 _Color5;
			uniform float4 _FlashColor;
			uniform float _flashAmount;

			
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
				float4 tex2DNode5 = tex2D( _MainTex, uv_MainTex );
				float4 temp_output_1_0_g1 = tex2DNode5;
				float4 temp_output_2_0_g1 = _Color3;
				float temp_output_11_0_g1 = distance( temp_output_1_0_g1 , temp_output_2_0_g1 );
				float4 lerpResult21_g1 = lerp( _Color4 , temp_output_1_0_g1 , saturate( ( ( temp_output_11_0_g1 - 0.05 ) / max( 0.0 , 1E-05 ) ) ));
				float4 temp_output_1_0_g2 = lerpResult21_g1;
				float4 temp_output_2_0_g2 = _Color5;
				float temp_output_11_0_g2 = distance( temp_output_1_0_g2 , temp_output_2_0_g2 );
				float4 lerpResult21_g2 = lerp( _Color6 , temp_output_1_0_g2 , saturate( ( ( temp_output_11_0_g2 - 0.05 ) / max( 0.0 , 1E-05 ) ) ));
				float ifLocalVar12 = 0;
				if( tex2DNode5.a <= 0.0 )
				ifLocalVar12 = 0.0;
				else
				ifLocalVar12 = 1.0;
				float4 lerpResult10 = lerp( lerpResult21_g2 , ( ifLocalVar12 * _FlashColor ) , _flashAmount);
				
				fixed4 c = lerpResult10;
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
519;930;1607;928;938.504;283.1381;1;True;False
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;3;-946.2459,-148.644;Inherit;True;0;0;_MainTex;Shader;False;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;5;-700.9621,-126.5716;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;13;-543.0702,124.7404;Inherit;False;Constant;_Float0;Float 0;2;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-519.0702,211.7404;Inherit;False;Constant;_Float1;Float 1;2;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;17;-331.6125,-190.005;Inherit;False;Property;_Color4;Color 4;3;1;[HDR];Create;True;0;0;False;0;False;0,0,0,0;4.237095,0,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;18;-84.24014,-147.2039;Inherit;False;Constant;_Float3;Float 3;4;0;Create;True;0;0;False;0;False;0.05;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;16;-318.942,-385.7945;Inherit;False;Property;_Color3;Color 3;2;0;Create;True;0;0;False;0;False;0,0,0,0;0.9960785,0.1803922,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ConditionalIfNode;12;-239.736,81.95837;Inherit;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;15;32.75802,-380.5448;Inherit;False;Replace Color;-1;;1;896dccb3016c847439def376a728b869;1,12,0;5;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;8;-406.9908,328.2126;Inherit;False;Property;_FlashColor;FlashColor;1;1;[HDR];Create;True;0;0;False;0;False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;20;46.49817,-187.4064;Inherit;False;Property;_Color5;Color 5;5;0;Create;True;0;0;False;0;False;0,0,0,0;0.9960785,0.9411765,0.9568628,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;21;44.49817,14.59363;Inherit;False;Property;_Color6;Color 6;4;1;[HDR];Create;True;0;0;False;0;False;0,0,0,0;2,2,2,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;19;332.4984,-244.7064;Inherit;False;Replace Color;-1;;2;896dccb3016c847439def376a728b869;1,12,0;5;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;11;246.4191,291.0282;Inherit;False;Property;_flashAmount;flashAmount;0;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;25.89492,231.1059;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;10;527.9478,104.9108;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1;787.9281,64.79527;Float;False;True;-1;2;ASEMaterialInspector;0;8;TankBossShader;0f8ba0101102bb14ebf021ddadce9b49;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;2;True;3;1;False;-1;10;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;True;2;False;-1;False;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;5;0;3;0
WireConnection;12;0;5;4
WireConnection;12;2;13;0
WireConnection;12;3;14;0
WireConnection;12;4;14;0
WireConnection;15;1;5;0
WireConnection;15;2;16;0
WireConnection;15;3;17;0
WireConnection;15;4;18;0
WireConnection;19;1;15;0
WireConnection;19;2;20;0
WireConnection;19;3;21;0
WireConnection;19;4;18;0
WireConnection;7;0;12;0
WireConnection;7;1;8;0
WireConnection;10;0;19;0
WireConnection;10;1;7;0
WireConnection;10;2;11;0
WireConnection;1;0;10;0
ASEEND*/
//CHKSM=32F8863BAB9200BB83A1528297AEEA5FC8B2CF90