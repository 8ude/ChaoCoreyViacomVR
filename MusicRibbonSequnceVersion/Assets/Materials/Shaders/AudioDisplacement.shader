﻿ // Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/AudioDisplacement" {
	Properties {

		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_AudioPosition ("Audio Position", Vector) = (0,0,0,0)
		_AudioInput ("Audio Input", Float) = 0.0
		_InputAlpha("Input Alpha", Float) = 0.0

		_MaxAudioDistance("Max Audio Distance", Float) = 0.4

		A ("Amplitude", Float) = 0.5 //amplitude
		L ("Wavelength", Float) = 1 //wavelength
		S ("Speed", Float) = 0.1 //speed
		Q ("Steepness", Range(0,1)) = 0.5 //steepness
		i ("Number of Waves",Range(1,10)) = 1 //number of waves
		D ("Wave Direction", Vector) = (0.5,0.0,0.5,0.0) //wave direction

	}

	SubShader {
		Tags { "Queue" = "Transparent" "RenderType"="Transparent" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows vertex:vert alpha:fade

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0
		//#include "Common.cginc"
		#include "SimplexNoiseGrad3D.cginc"

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float dummy;
		};


		float4 _AudioPosition;
		float _AudioInput;
		float _MaxAudioDistance;
		float _InputAlpha;

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;


		float A = 0.5; //amplitude
		float L = 1; //wavelength
		float S = 0.1; //speed
		float Q = 0.5; //steepness
		float i = 1; //number of waves
		float2 D = float2(0.5,0.5); //wave direction
		float w;

		//w = 2/L
		float freq(float wavelength){
			return 2/L;
		}

		float QA(float w){
			i = ceil(i);
			//return A;
			return A*Q/(w*A*i);
		}

		float phaseConstant(float speed){
			return speed * w;
		}

		float gInner(float3 pos){
			return dot(D,pos.xy) * w + _Time.y * phaseConstant(S);
			
			//return sin(posXY.x)*A;
		}

		float3 gerstnerWave(float3 pos){
			w = freq(L);
			D = normalize(D);
			float ampCalc = QA(w);
			float waveCalc = gInner(pos);
			pos.x += ampCalc * D.x * cos(waveCalc);
			pos.z += ampCalc * D.y * cos(waveCalc);
			pos.y += A * sin(waveCalc);

			return pos;
		}

		float rand(float3 myVector) {

			return frac(sin(dot(myVector, float3(12.9898,78.233, 45.5432)))*43758.5453);

		}




		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_CBUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_CBUFFER_END



		void vert (inout appdata_full v) {

			float3 vertWorld = mul(unity_ObjectToWorld, v.vertex).xyz;

			float distPointToSound = distance(vertWorld, _AudioPosition.xyz);

			S += (S * _AudioInput * 0.05);
			float3 wsVertexOut = gerstnerWave(v.vertex.xyz);

			v.vertex.xyz = wsVertexOut;


			
			if (distPointToSound < _MaxAudioDistance) {

				S += (S * _AudioInput * 0.1 );
				float3 subWave = gerstnerWave(cross(v.vertex, v.normal) * _AudioInput * 
				((_MaxAudioDistance-distPointToSound)/_MaxAudioDistance) * (_AudioInput * 3.0 * 
				((_MaxAudioDistance-distPointToSound)/_MaxAudioDistance) * 2.0 * rand(v.vertex.xyz)));

				v.vertex.xyz += subWave * 0.3;

				//float3 newVertPos = (v.normal * _AudioInput * ((_MaxAudioDistance-distPointToSound)/_MaxAudioDistance))
				//v.vertex.xyz = v.vertex.xyz + (cross(v.vertex * rand(v.vertex), v.normal)* 0.54 * _AudioInput * ((_MaxAudioDistance-distPointToSound)/_MaxAudioDistance));
			}

		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb * _Color;
			//o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables

			//float2 bullshit = IN.screenPos.xy / IN.screenPos.w;
			//o.Metallic = _Metallic * bullshit.x ;

			o.Metallic = _AudioInput;

			//if(distToSound < _MaxAudioDistance) {
			//	o.Metallic += _AudioInput;
			//}

			o.Smoothness = _Glossiness;
			o.Alpha = _InputAlpha;
		}
		ENDCG
	}
	FallBack "Diffuse"
}