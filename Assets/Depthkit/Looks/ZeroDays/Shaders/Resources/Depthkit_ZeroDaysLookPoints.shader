/************************************************************************************

Depthkit Unity SDK License v1
Copyright 2016-2018 Scatter All Rights reserved.  

Licensed under the Scatter Software Development Kit License Agreement (the "License"); 
you may not use this SDK except in compliance with the License, 
which is provided at the time of installation or download, 
or which otherwise accompanies this software in either electronic or hard copy form.  

You may obtain a copy of the License at http://www.depthkit.tv/license-agreement-v1

Unless required by applicable law or agreed to in writing, 
the SDK distributed under the License is distributed on an "AS IS" BASIS, 
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
See the License for the specific language governing permissions and limitations under the License. 

************************************************************************************/

Shader "Depthkit/ZeroDaysLookPoints"
{
    Properties
    {
        _Sprite 	("Texture", 2D) = "white" {}
        _UseSprite 	("Use Sprite", Float) = 0.0
        _Width 		("Width", Float) = 1.0
        _Height 	("Height", Float) = 1.0
        _Opacity 	("Opacity", Range(0,1)) = 1.0

        //NOTE: These are set per material, not per layer, as they cannot be controlled by the MaterialPropertyBlock
        _SrcMode	("Blend Src Mode", Float) = 0.0
        _DstMode	("Blend Dst Mode", Float) = 0.0

        _BlendEnum("Blend Enum", Float) = 0.0
    }

    SubShader
    {
        // All Zero Days Look shaders are rendered in the transparency pass, with no shadowing
        Tags { "Queue"="Transparent+1" "RenderType"="Transparent" "IgnoreProjector"="True" "ForceNoShadowCasting" = "True" }
        LOD 100

        Blend [_SrcMode] [_DstMode]
        ZWrite Off
        Cull Off
        ZTest LEqual
        Offset -32, -1 // large bias to camera to ensure our lines do not fail depth test when co-planar with depth

        Pass
        {
            CGPROGRAM

            #pragma exclude_renderers d3d11_9x
            #pragma vertex vert
            #pragma fragment frag
            #pragma geometry geom
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "../../../Resources/Depthkit.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2g
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                DEPTHKIT_TEX_COORDS(1, 2, 3)    // reserve texcoords/interpolants 1-3 for depthkit use
                UNITY_FOG_COORDS(4)	            // if Unity distance fogging enabled
            };

            sampler2D _Sprite;
            float4 _Sprite_ST;

            //Creates the necessary declarations for DK textures
            DEPTHKIT_TEX_ST

            float _UseSprite;
            float _Width;
            float _Height;
            float _Opacity;
            float _BlendEnum;

            v2g vert (appdata v)
            {
                v2g o;
                UNITY_INITIALIZE_OUTPUT(v2g,o);

                float2 colorTexCoord;
                float2 depthTexCoord;
                float4 vertOut;

                dkVertexPass(v.vertex, colorTexCoord, depthTexCoord, vertOut);

                o.vertex = mul(unity_ObjectToWorld, vertOut);
                o.uv_MainTex   = colorTexCoord;
                o.uv2_MainTex2 = depthTexCoord;
                o.worldPos = vertOut.xyz;

                UNITY_TRANSFER_FOG(o, vertOut);

                return o;
            }

            [maxvertexcount(4)]
            void geom(point v2g input[1], inout TriangleStream<v2g> OutputStream)
            {

                //filter for segements that are pulled to the near plane
                float depth = mul(_InverseExtrinsics, float4(input[0].worldPos, 1)).z;
                if (depth <= (_NearClip + _ClipEpsilon) ||
                    depth >= (_FarClip  - _ClipEpsilon))
                {
                    return;
                }

                float3 up = float3(0, 1, 0);
                float3 look = _WorldSpaceCameraPos.xyz - input[0].vertex.xyz;
                float3 right = normalize(cross(up, look));
                up = normalize(cross(look, right));

                float halfWidth  = 0.5f * _Width;
                float halfHeight = 0.5f * _Height;

                float4 v[4];
                v[0] = float4(input[0].vertex.xyz + halfWidth * right - halfHeight * up, 1.0);
                v[1] = float4(input[0].vertex.xyz + halfWidth * right + halfHeight * up, 1.0);
                v[2] = float4(input[0].vertex.xyz - halfWidth * right - halfHeight * up, 1.0);
                v[3] = float4(input[0].vertex.xyz - halfWidth * right + halfHeight * up, 1.0);
                
                v2g output;
                UNITY_INITIALIZE_OUTPUT(v2g, output);

                output.vertex = mul(UNITY_MATRIX_VP, v[0]);
                output.worldPos = v[0].xyz;
                output.uv = float2(0.0f, 0.0f);
                output.uv_MainTex   = input[0].uv_MainTex;
                output.uv2_MainTex2 = input[0].uv2_MainTex2;
                OutputStream.Append(output);

                output.vertex = mul(UNITY_MATRIX_VP, v[1]);
                output.worldPos = v[1].xyz;
                output.uv = float2(0.0f, 1.0f);
                output.uv_MainTex   = input[0].uv_MainTex;
                output.uv2_MainTex2 = input[0].uv2_MainTex2;
                OutputStream.Append(output);

                output.vertex = mul(UNITY_MATRIX_VP, v[2]);
                output.worldPos = v[2].xyz;
                output.uv = float2(1.0f, 0.0f);
                output.uv_MainTex   = input[0].uv_MainTex;
                output.uv2_MainTex2 = input[0].uv2_MainTex2;
                OutputStream.Append(output);

                output.vertex = mul(UNITY_MATRIX_VP, v[3]);
                output.worldPos = v[3].xyz;
                output.uv = float2(1.0f, 1.0f);
                output.uv_MainTex   = input[0].uv_MainTex;
                output.uv2_MainTex2 = input[0].uv2_MainTex2;
                OutputStream.Append(output);
            }

            fixed4 frag (v2g i) : SV_Target
            {
                float3 dkColor;
                dkFragmentPass(i.uv2_MainTex2, i.uv_MainTex, i.worldPos, dkColor, false);

                // sample the texture
                float4 spriteCol = lerp( float4(1.0,1.0,1.0,1.0), tex2D(_Sprite, i.uv), _UseSprite);

                float4 finalCol;
                finalCol.rgb = spriteCol.rgb * dkColor;
                finalCol.a = 1.0;

                // note; BLEND_MULTIPLY has a known issue with respecting transparency on sprites.
                if (_BlendEnum == BLEND_ALPHA || _BlendEnum == BLEND_MULTIPLY)
                {
                    finalCol.a *= spriteCol.a * _Opacity;
                }
                else if (_BlendEnum == BLEND_ADD || _BlendEnum == BLEND_SCREEN)
                {
                    finalCol.rgb *= spriteCol.a * _Opacity;
                }

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, finalCol);

                return finalCol;
            }
            ENDCG
        }
    }
}
