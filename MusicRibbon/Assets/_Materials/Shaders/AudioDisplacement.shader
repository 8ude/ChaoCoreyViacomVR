﻿// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/AudioDisplacement" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)

        //_MainTex will be adjusted by AudioTexture
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
        
		_AudioPosition ("Audio Position", Vector) = (0,0,0,0)

        //AudioInput is averaged audio data from the ribbon audio sources
		_AudioInput ("Audio Input", Float) = 0.0

        //not used currently
		_InputAlpha("Input Alpha", Float) = 0.0

        //position of wand after ontriggerenter is called
		_WandPos("Wand Pos", Vector) = (0,0,0,0)


        //These properties could and should be tuned for each instrument:
        //How much the audio source creates a localized chaotic spikey effect
        _PosTurb("Positional Turbulence", Range(0,3)) = 1.0
        //How much the audio input changes the wave speed/phase
        _WaveShud("Wave Shudder", Range(0,1)) = 0.1
        //How much the audio input adjusts overall chaotic spikey displacement
        _Turbulence("Turbulence", Float) = 0.0
        //The speed of the chaotic spikey effect
        _TurbulenceSpeed("Turbulence Speed", Range(0, 40)) = 0.0

		//Controls how much spikes will displace from the mesh
		_Spikinees("Spikiness", Range(1,3)) = 1.0

		//Degree of color change around Audio Source and Wand
		_ColorShift("Color Shift", Range(0.1, 0.5)) = 0.2

		_MaxAudioDistance("Max Audio Distance", Float) = 0.4
		_MaxWandDistance("Max Wand Distance", Float) = 0.4
        

		A ("Amplitude", Float) = 0.5 //amplitude
		L ("Wavelength", Float) = 1 //wavelength
		S ("Speed", Float) = 0.1 //speed
		Q ("Steepness", Range(0,1)) = 0.5 //steepness
		i ("Number of Waves",Range(1,10)) = 1 //number of waves
		D ("Wave Direction", Vector) = (0.5,0.0,0.5,0.0) //wave direction
	}
	SubShader {
		Tags { "Queue" = "Geometry" "RenderType"="Geometry" }
        LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows vertex:vert
			//alpha:fade

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0
		#include "ClassicNoise3D.hlsl" 

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float3 customColor;
		};

        

		float4 _AudioPosition;
		
		float _AudioInput;
		float _MaxAudioDistance;
		float _InputAlpha;

		float4 _WandPos;
		float _MaxWandDistance;

        float _Turbulence;
        float _TurbulenceSpeed;

		float _Spikiness;

        float _PosTurb;
        float _WaveShud;

		float _ColorShift;

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		float noise;

		float A = 0.5; //amplitude
		float L; //wavelength
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

		float turbulence(float3 p) {
			float w = 100.0;
  			float t = -.5;

  			for (float f = 1.0 ; f <= 10.0 ; f++ ){
   				float power = pow( 2.0, f );
    			t += abs( pnoise( float3( power * p ), float3( 10.0, 10.0, 10.0 ) ) / power );
  			}

  			return t;

		}

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_CBUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_CBUFFER_END



		void vert (inout appdata_full v, out Input o) {

            UNITY_INITIALIZE_OUTPUT(Input, o);

			float3 vertWorld = mul(unity_ObjectToWorld, v.vertex).xyz;

			float distPointToSound = distance(vertWorld, _AudioPosition.xyz);

			float distPointToWand = distance(vertWorld, _WandPos.xyz);

			S += (S * _AudioInput * _WaveShud * 0.1);
			float3 wsVertexOut = gerstnerWave(v.vertex.xyz);

			v.vertex.xyz = wsVertexOut;

            o.customColor = float3(1.0,1.0,1.0);

            //create noise by scaling turbulence (from the other shader file), with the time scale
            //given by the _TurbulenceSpeed property)
            noise = 10.0 *  -0.10 * turbulence( 0.5 * v.normal + (_Time.x * _TurbulenceSpeed));
            // get a 3d noise using the position, low frequency
            float b = 5.0 * pnoise( 0.05 * v.vertex.xyz, float3( 100.0, 100.0, 100.0 ) );
            // compose both noises
            float displacement = -10.0 * noise + b;

			float spikeThreshold = 0.005;

			if (displacement > spikeThreshold) {
				displacement *= _Spikiness * 5;
			}
            
			 //
			if (distPointToSound < _MaxAudioDistance) {

				

				//S += (S * _AudioInput * 0.1 );
				float3 subWave = gerstnerWave(cross(v.vertex, v.normal) * _AudioInput * 
				((_MaxAudioDistance-distPointToSound)/_MaxAudioDistance) * (_AudioInput * _PosTurb * 
				((_MaxAudioDistance-distPointToSound)/_MaxAudioDistance) * displacement));

				v.vertex.xyz += subWave * 0.001;

                //NEED TO CHECK THIS!!!! getting color to change based on audio
                o.customColor.r = 1-((_MaxAudioDistance-distPointToSound)/_MaxAudioDistance);
                o.customColor.g = 1-((_MaxAudioDistance-distPointToSound)/_MaxAudioDistance);
                o.customColor.b = 1-((_MaxAudioDistance-distPointToSound)/_MaxAudioDistance);

                //o.customColor *= ((_MaxAudioDistance-distPointToSound)/_MaxAudioDistance);

				//float3 newVertPos = (v.normal * _AudioInput * ((_MaxAudioDistance-distPointToSound)/_MaxAudioDistance))
				//v.vertex.xyz += (v.normal * _AudioInput * ((_MaxAudioDistance-distPointToSound)/_MaxAudioDistance));
			}

			if (distPointToWand < _MaxWandDistance) {

				noise = 10.0 *  -0.10 * turbulence( 0.5 * v.normal + _Time.y);
 				// get a 3d noise using the position, low frequency
 				float b = 5.0 * pnoise( 0.05 * v.vertex.xyz, float3( 100.0, 100.0, 100.0 ) );
  				// compose both noises
  				float displacement = -10.0 * noise + b;

				//S += (S * 0.1 );
				float3 subWave = gerstnerWave(cross(v.vertex, v.normal) * 
				((_MaxWandDistance-distPointToWand)/_MaxWandDistance) * (_AudioInput * 3.0 * 
				((_MaxWandDistance-distPointToWand)/_MaxWandDistance) * displacement));

				//v.vertex.xyz += subWave * 1.0;
			}

            //Ambient Noise

            v.vertex.xyz += v.normal * displacement * ( _Turbulence * 0.001 ) * (_AudioInput);

		}




		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			//fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb * _Color;
	        o.Albedo = (o.Albedo * (1.0 - _ColorShift) + (IN.customColor * _ColorShift));
			//o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = _InputAlpha;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
