Shader "Interactive Grass/Model" {
	Properties {
		_MainTex       ("Base", 2D) = "white" {}
		_Cutoff        ("Cutoff", Range(0, 1)) = 0.5
		[Header(Animation)]
		_Amplitude     ("Amplitude", Float) = 0.2
		_WaveX         ("Waves X", Float) = 1
		_WaveY         ("Waves Y", Float) = 1
		_TimeScale     ("Time Scale", Float) = 1
		_MoveVec       ("Move Vec", Vector) = (0, 0, 0, 0)
		[Header(Burn)]
		_BurnAmount    ("Burn Amount", Range(0.0, 1.0)) = 0.0
		_BurnLineWidth ("Burn Line Width", Range(0.0, 0.2)) = 0.1
		_BurnColor1    ("Burn First Color", Color) = (1, 0, 0, 1)
		_BurnColor2    ("Burn Second Color", Color) = (1, 0, 0, 1)
		_BurnTex       ("Burn", 2D) = "white"{}
	}
	SubShader {
		Tags { "Queue" = "AlphaTest" "IgnoreProjector" = "True" "RenderType" = "TransparentCutout" }
		Cull Off
		
		CGPROGRAM
		#pragma surface surf Grass vertex:vert addshadow
		
		// basic
		sampler2D _MainTex;
		fixed _Cutoff;

		// vertex animation
		float _Amplitude, _WaveX, _WaveY, _TimeScale;
		float3 _MoveVec;

		// burn
		fixed _BurnAmount, _BurnLineWidth;
		fixed4 _BurnColor1, _BurnColor2;
		sampler2D _BurnTex;
		
		void vert (inout appdata_full v)
		{
			float4 p = v.vertex;
			p.x += _Amplitude * sin(p.x * _WaveX + _Time.y * _TimeScale + p.z * _WaveY) * v.color.r + _MoveVec.x * v.color.r;
			p.z += _Amplitude * sin(p.x * _WaveX + _Time.y * _TimeScale + p.z * _WaveY) * v.color.r + _MoveVec.z * v.color.r;
			v.vertex = p;
        }
		struct Input
		{
			float2 uv_MainTex;
			float2 uv_BurnTex;
		};
		void surf (Input IN, inout SurfaceOutput o)
		{
			fixed4 base = tex2D(_MainTex, IN.uv_MainTex);
			clip(base.a - _Cutoff);	

			fixed4 burn = tex2D(_BurnTex, IN.uv_BurnTex);
			clip(burn.r - _BurnAmount);

			fixed t = 1 - smoothstep(0.0, _BurnLineWidth, burn.r - _BurnAmount);
			fixed3 burnColor = lerp(_BurnColor1, _BurnColor2, t);
			burnColor = pow(burnColor, 5);
			fixed3 rgb = lerp(base.rgb, burnColor, t * step(0.0001, _BurnAmount));
			
			o.Albedo = rgb;
			o.Alpha = base.a;
		}
		inline fixed4 LightingGrass (SurfaceOutput s, fixed3 lightDir, half3 viewDir, fixed atten)
		{
			float3 V = normalize(viewDir);
			float3 N = normalize(s.Normal);
			float3 L = normalize(lightDir);
			float3 H = normalize(L + N);
			
			float ndl = dot(N, L);
			float back = clamp(dot(V, -L), 0.0, 1.0);
			back = lerp(clamp(-ndl, 0.0, 1.0), back, 0.85);
			ndl = max(0.0, ndl * 0.7 + 0.3);

			fixed4 c;
			c.rgb = (back + ndl) * s.Albedo * _LightColor0.rgb;
			c.a = s.Alpha;
			return c;
		}
		ENDCG
	}
	FallBack "Transparent/Cutout/VertexLit"
}
