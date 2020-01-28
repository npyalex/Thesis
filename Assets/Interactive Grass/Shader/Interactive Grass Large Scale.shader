Shader "Interactive Grass/Large Scale" {
	Properties {
		[Header(Shading)]
		_MainTex             ("Base", 2D) = "white" {}
		_TopColor            ("Top Color", Color) = (1, 1, 1, 1)
		_BottomColor         ("Bottom Color", Color) = (1, 1, 1, 1)
		_TranslucentGain     ("Translucent Gain", Range(0, 1)) = 0.5
		[Space]
		_TessellationUniform ("Tessellation Uniform", Range(1, 64)) = 1
		[Header(Blades)]
		_BladeWidth          ("Blade Width", Float) = 0.05
		_BladeWidthRandom    ("Blade Width Random", Float) = 0.02
		_BladeHeight         ("Blade Height", Float) = 0.5
		_BladeHeightRandom   ("Blade Height Random", Float) = 0.3
		_BendRotationRandom  ("Bend Rotation Random", Range(0, 1)) = 0.2
		_BladeForward        ("Blade Forward Amount", Float) = 0.38
		[Header(Wind)]
		_WindDistortionMap   ("Wind Distortion Map", 2D) = "white" {}
		_WindStrength        ("Wind Strength", Float) = 1
		_WindFrequency       ("Wind Frequency", Vector) = (0.05, 0.05, 0, 0)
		[Header(Force)]
		_ForceParam          ("Force (xy:dir zw:pos)", Vector) = (0, 0, 0, 0)
		_ForceRange          ("Force Range", Float) = 2
	}

	CGINCLUDE
	#include "UnityCG.cginc"
	#include "Autolight.cginc"
	#include "Lighting.cginc"
	
	float _TessellationUniform;
	float4 _TopColor, _BottomColor;
	float _TranslucentGain;
	float _BladeHeight, _BladeHeightRandom, _BladeWidthRandom, _BladeWidth, _BladeForward, _BendRotationRandom;
	sampler2D _WindDistortionMap, _MainTex;
	float4 _WindDistortionMap_ST;
	float _WindStrength;
	float2 _WindFrequency;
	float4 _ForceParam;
	float _ForceRange;
	
	struct vertexInput
	{
		float4 vertex : POSITION;
		float3 normal : NORMAL;
		float4 tangent : TANGENT;
	};
	struct vertexOutput
	{
		float4 vertex : SV_POSITION;
		float3 normal : NORMAL;
		float4 tangent : TANGENT;
	};
	struct geometryOutput
	{
		float4 pos : SV_POSITION;
		float3 normal : NORMAL;
		float2 uv : TEXCOORD0;
		unityShadowCoord4 _ShadowCoord : TEXCOORD1;
	};

	// tessellation part of shader ///////////////////////////////////////////////////////////////////////////////////////////////////////////
	struct TessellationFactors 
	{
		float edge[3] : SV_TessFactor;
		float inside : SV_InsideTessFactor;
	};
	TessellationFactors patchConstantFunction (InputPatch<vertexInput, 3> patch)
	{
		TessellationFactors f;
		f.edge[0] = _TessellationUniform;
		f.edge[1] = _TessellationUniform;
		f.edge[2] = _TessellationUniform;
		f.inside = _TessellationUniform;
		return f;
	}
	[UNITY_domain("tri")]
	[UNITY_outputcontrolpoints(3)]
	[UNITY_outputtopology("triangle_cw")]
	[UNITY_partitioning("integer")]
	[UNITY_patchconstantfunc("patchConstantFunction")]
	vertexInput hull (InputPatch<vertexInput, 3> patch, uint id : SV_OutputControlPointID)
	{
		return patch[id];
	}
	[UNITY_domain("tri")]
	vertexOutput domain (TessellationFactors factors, OutputPatch<vertexInput, 3> patch, float3 barycentricCoordinates : SV_DomainLocation)
	{
		vertexInput v;

		#define MY_DOMAIN_INTERPOLATE(fieldName) v.fieldName = \
			patch[0].fieldName * barycentricCoordinates.x + \
			patch[1].fieldName * barycentricCoordinates.y + \
			patch[2].fieldName * barycentricCoordinates.z;

		MY_DOMAIN_INTERPOLATE(vertex)
		MY_DOMAIN_INTERPOLATE(normal)
		MY_DOMAIN_INTERPOLATE(tangent)

		vertexOutput o;
		o.vertex = v.vertex;
		o.normal = v.normal;
		o.tangent = v.tangent;
		return o;
	}
	// tessellation part of shader ///////////////////////////////////////////////////////////////////////////////////////////////////////////
	float rand (float3 v) { return frac(sin(dot(v.xyz, float3(12.9898, 78.233, 53.539))) * 43758.5453); }
	float3x3 AngleAxis3x3 (float angle, float3 axis)
	{
		float c, s;
		sincos(angle, s, c);
		float t = 1 - c;
		float x = axis.x;
		float y = axis.y;
		float z = axis.z;
		return float3x3(
			t * x * x + c, t * x * y - s * z, t * x * z + s * y,
			t * x * y + s * z, t * y * y + c, t * y * z - s * x,
			t * x * z - s * y, t * y * z + s * x, t * z * z + c);
	}
	vertexInput vert (vertexInput v) { return v; }
	[maxvertexcount(3)]
	void geo (point vertexOutput IN[1], inout TriangleStream<geometryOutput> triStream)
	{
		float3 pos = IN[0].vertex.xyz;

		// build tangent to local matrix
		float3 nor = IN[0].normal;
		float4 tgt = IN[0].tangent;
		float3 bin = cross(nor, tgt) * tgt.w;
		float3x3 t2l = float3x3(
			tgt.x, bin.x, nor.x,
			tgt.y, bin.y, nor.y,
			tgt.z, bin.z, nor.z);

		// config size
		float height = (rand(pos.zyx) * 2 - 1) * _BladeHeightRandom + _BladeHeight;
		float width = (rand(pos.xzy) * 2 - 1) * _BladeWidthRandom + _BladeWidth;
		float forward = rand(pos.yyz) * _BladeForward;

		// wind animation
		float2 uv = pos.xz * _WindDistortionMap_ST.xy + _WindDistortionMap_ST.zw + _WindFrequency * _Time.y;
		float2 s = (tex2Dlod(_WindDistortionMap, float4(uv, 0, 0)).xy * 2 - 1) * _WindStrength;
		float3 wind = normalize(float3(s.x, s.y, 0));
		float3x3 windMat = AngleAxis3x3(UNITY_PI * s, wind);

		// transform
		float3x3 m1 = AngleAxis3x3(rand(pos) * UNITY_TWO_PI, float3(0, 0, 1));
		float3x3 m2 = AngleAxis3x3(rand(pos.zzx) * _BendRotationRandom * UNITY_PI * 0.5, float3(-1, 0, 0));
		float3x3 mCompAll = mul(mul(mul(t2l, windMat), m1), m2);
		float3x3 mFacing = mul(t2l, m1);
		
		// position after above calculation
		float3 pos1 = pos + mul(mFacing, float3(width, 0, 0));
		float3 pos2 = pos + mul(mFacing, float3(-width, 0, 0));
		float3 pos3 = pos + mul(mCompAll, float3(0, 0, height));
		
		// interactive force
		float3 wldpos = mul((half3x3)unity_ObjectToWorld, pos3);
		float3 wlddir = float3(mul((half3x3)unity_WorldToObject, float3(_ForceParam.x, 0, _ForceParam.y)));
		float weight = 1 - saturate(length(_ForceParam.zw - wldpos.xz) / _ForceRange);
//		pos3.xyz += wlddir * v.color.a * weight;
		pos3.xyz += wlddir * weight;
		
		geometryOutput o;
		o.pos = UnityObjectToClipPos(pos1);
		o.uv = float2(0, 0);
		o.normal = UnityObjectToWorldNormal(mul(mFacing, normalize(float3(0, -1, forward))));
		o._ShadowCoord = ComputeScreenPos(o.pos);
		triStream.Append(o);
		
		o.pos = UnityObjectToClipPos(pos2);
		o.uv = float2(1, 0);
		o.normal = UnityObjectToWorldNormal(mul(mFacing, normalize(float3(0, -1, forward))));
		o._ShadowCoord = ComputeScreenPos(o.pos);
		triStream.Append(o);
		
		o.pos = UnityObjectToClipPos(pos3);
		o.uv = float2(0, 1);
		o.normal = UnityObjectToWorldNormal(mul(mCompAll, normalize(float3(0, -1, forward))));
		o._ShadowCoord = ComputeScreenPos(o.pos);
		triStream.Append(o);
	}
	ENDCG
	SubShader {
		Cull Off
		Pass {
			Tags { "RenderType" = "Opaque" "LightMode" = "ForwardBase" }
			CGPROGRAM
			#pragma vertex vert
			#pragma geometry geo
			#pragma fragment frag
			#pragma hull hull
			#pragma domain domain
			#pragma target 4.6
			#pragma multi_compile_fwdbase
			float4 frag (geometryOutput i,  fixed facing : VFACE) : SV_Target
			{
				float3 nor = facing > 0 ? i.normal : -i.normal;
				float shadow = SHADOW_ATTENUATION(i);
				float ndl = saturate(saturate(dot(nor, _WorldSpaceLightPos0)) + _TranslucentGain) * shadow;
				float4 lit = ndl * _LightColor0 + float4(ShadeSH9(float4(nor, 1.0)), 1.0);
				float4 albedo = tex2D(_MainTex, i.uv);
				return lerp(albedo * _BottomColor, albedo * _TopColor * lit, i.uv.y);
//				return lerp(_BottomColor, _TopColor * lit, i.uv.y);
			}
			ENDCG
		}
		Pass {
			Tags { "LightMode" = "ShadowCaster" }
			CGPROGRAM
			#pragma vertex vert
			#pragma geometry geo
			#pragma fragment frag
			#pragma hull hull
			#pragma domain domain
			#pragma target 4.6
			#pragma multi_compile_shadowcaster
			float4 frag (geometryOutput i) : SV_Target
			{
				SHADOW_CASTER_FRAGMENT(i)
			}
			ENDCG
		}
    }
}
