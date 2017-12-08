using UnityEngine;
using System.Collections;

//[RequireComponent (typeof (AudioSource))]
public class SpectrumAnalysis : MonoBehaviour {

	//public SpectrumAnalysis instance;

	[HideInInspector]
	public float[] spectrumData;
    public float[] spectrumDataB;

	public AudioSource audioSource;
    public AudioSource audioSourceB;

	public float[] bandBuffer;
    public float[] bandBufferB;
	int bufferSize = 128;
	float[] bufferDecrease;
    float[] bufferDecreaseB;

	public float sampleRate;




	void Awake(){

		audioSource = gameObject.GetComponent<AudioSource> ();

        if (GetComponents<AudioSource>().Length > 0) {
            //max 2 audiosources for now
            AudioSource[] sources = GetComponents<AudioSource>();
            audioSource = sources[0];
            audioSourceB = sources[1];
        }

		/*Deleting Singleton Stuff for now
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (gameObject);

		} else if (instance != this) {
			Destroy (gameObject);
		}
		*/
	}

	// Use this for initialization
	void Start () {

		//audioSource = gameObject.GetComponent<AudioSource> ();
		spectrumData = new float[bufferSize];
        spectrumDataB = new float[bufferSize];
		bandBuffer = new float[bufferSize];
        bandBufferB = new float[bufferSize];
		bufferDecrease = new float[bufferSize];
        bufferDecreaseB = new float[bufferSize];
		sampleRate = AudioSettings.outputSampleRate;

		//audioSource = gameObject.GetComponent<AudioSource> ();
		//StartCoroutine (GetSpectrumFrameChanges());
	}




	IEnumerator GetSpectrumFrameChanges(){
		//		while (true) {
		//			
		//			yield return new WaitForSeconds (0.05f);
		//		}
		return null;
	}

	public float GetWholeEnergy(){
		float wholeEnergy = 0;
		for (int i = 0; i < spectrumData.Length; i++) {
			wholeEnergy += spectrumData [i] * spectrumData [i];

            if (audioSourceB != null) {
                wholeEnergy += spectrumDataB[i] * spectrumDataB[i];


            }
		}

		wholeEnergy *= 100f;
		wholeEnergy = CompressorExciter (wholeEnergy, 0.05f, 0.7f);

        if (audioSourceB != null) {
            //need to scale the result down by 4 if we have more than one spectrum
            wholeEnergy /= 4f;
        }


		return wholeEnergy;


	}



	// Update is called once per frame
	void FixedUpdate () {
		audioSource.GetSpectrumData( spectrumData, 0, FFTWindow.Blackman );
        if(audioSourceB != null) {
            audioSourceB.GetSpectrumData(spectrumDataB, 0, FFTWindow.Blackman);
        }

		BandBuffer ();
		float wholeEnergy = 0;
		for (int i = spectrumData.Length / 5 * 2; i < spectrumData.Length / 5 * 2 + spectrumData.Length / 2; i++) {
			wholeEnergy += spectrumData [i];

            if (audioSourceB != null) {
                wholeEnergy += spectrumDataB[i];
                wholeEnergy = wholeEnergy / 4f;
            }
		}

		//Debug.Log (wholeEnergy);

		//Camera.main.backgroundColor =  HSL2RGB(Time.time / 10 - Mathf.FloorToInt(Time.time/10), 0.8f,lightNow);



	}

	public static float Compressor(float input, float threshold, float ratio, float gain){
		float t = input;
		t = threshold + (t - threshold) * ratio + gain;
		return t;
	}

	public static float CompressorExciter(float input, float threshold, float thresholdDB){


		if (1f - threshold <= 0f)
			return input;

		if (threshold == 0f)
			return input;

		float k = 0; float b = 0;
		if (input > threshold) {
			// k = (1- thresholdDB) / (1-threshold)
			// 1 * k + b = 1; b = 1 - k;

			k = (1f - thresholdDB) / (1f - threshold);
			b = 1f - k;

		} else {
			// k = (thresholdDB) / threshold
			k = thresholdDB / threshold;
			b = 0f;
		}
		return (input * k + b);
	}

	void BandBuffer() {

		for (int i = 0; i < bufferSize; i++) {
			if (spectrumData [i] > bandBuffer [i]) {
				bandBuffer [i] = spectrumData [i];
				bufferDecrease[i] = 0.001f;
			}

			if (spectrumData [i] < bandBuffer [i]) {
				bandBuffer [i] -= bufferDecrease [i]; 
				bufferDecrease [i] *= 1.1f;
			}
		}
		
	}

	public float GetEnergyFrequencyRange(float lowBound, float hiBound) {
		if (((hiBound * bufferSize) / sampleRate) > bufferSize) {
			Debug.Log ("hi bound too large!");
			return 0f;
		}

		float energy = 0;
		int lowBin = Mathf.RoundToInt ((lowBound * bufferSize) / sampleRate);
		int hiBin = Mathf.RoundToInt ((hiBound * bufferSize) / sampleRate);

		for (int i = lowBin; i < hiBin; i++) {
			energy += spectrumData [i] * spectrumData [i];
		}

		energy *= 100f;
		energy = CompressorExciter (energy, 0.05f, 0.7f);

		return energy;
	}



}
