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

Shader "Depthkit/ZeroDaysLookFill" 
{
    Properties
    {
        _Opacity 	("Opacity", Range(0,1)) = 1.0

        //NOTE: These are set per material, not per layer, as they cannot be controlled by the MaterialPropertyBlock
        _SrcMode	  ("Blend Src Mode", Float) = 0.0
        _DstMode	  ("Blend Dst Mode", Float) = 0.0
    }
    
    SubShader
    {
        // All Zero Days Look shaders are rendered in the transparency pass, with no shadowing
        Tags { "Queue"="Transparent+1" "RenderType"="Transparent" "IgnoreProjector"="True" "ForceNoShadowCasting"="True" }
        LOD 100

        Blend [_SrcMode] [_DstMode]
        ZWrite Off
        Cull Off
        ZTest LEqual

        Pass
        {
            CGPROGRAM

            #pragma exclude_renderers d3d11_9x
            #pragma vertex vert
            #pragma fragment frag
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
                UNITY_FOG_COORDS(4)             // if Unity distance fogging enabled
            };

            //Creates the necessary declarations for DK textures
            DEPTHKIT_TEX_ST

            float _Opacity;

            v2g vert (appdata v)
            {
                v2g o;
                UNITY_INITIALIZE_OUTPUT(v2g, o);

                float2 colorTexCoord;
                float2 depthTexCoord;
                float4 vertOut;

                dkVertexPass(v.vertex, colorTexCoord, depthTexCoord, vertOut);

                o.vertex = UnityObjectToClipPos(vertOut.xyz);
                o.uv = v.uv;
                o.uv_MainTex   = colorTexCoord;
                o.uv2_MainTex2 = depthTexCoord;
                o.worldPos = mul(unity_ObjectToWorld, vertOut);

                UNITY_TRANSFER_FOG(o, vertOut);

                return o;
            }
        
            fixed4 frag (v2g i) : SV_Target
            {
                float3 dkColor;
                dkFragmentPass(i.uv2_MainTex2, i.uv_MainTex, i.worldPos, dkColor);

                float4 finalCol;
                finalCol.rgb = dkColor;

                finalCol.rgb *= _Opacity;
                finalCol.a    = _Opacity;

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, finalCol);

                return finalCol;
            }
            ENDCG
        }
    }
    Fallback "Diffuse"
}