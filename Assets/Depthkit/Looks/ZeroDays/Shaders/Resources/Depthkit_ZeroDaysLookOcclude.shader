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

Shader "Depthkit/ZeroDaysLookOcclude" 
{
    Properties
    {

    }
    
    SubShader
    {
        // All Zero Days Look shaders are rendered in the transparency pass, with no shadowing
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" "ForceNoShadowCasting"="True" }

        ColorMask 0
        Cull Off
        ZWrite On

        Pass
        {
            CGPROGRAM

            #pragma exclude_renderers d3d11_9x
            #pragma vertex vert
            #pragma fragment frag

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

                o.worldPos = mul(unity_ObjectToWorld, vertOut).xyz;

                return o;
            }
        
            fixed4 frag (v2g i) : SV_Target
            {
                float3 dkColor;
                dkFragmentPass(i.uv2_MainTex2, i.uv_MainTex, i.worldPos, dkColor);

                return float4(0,0,0,0);
            }
            ENDCG
        }
    }
    Fallback "Diffuse"
}