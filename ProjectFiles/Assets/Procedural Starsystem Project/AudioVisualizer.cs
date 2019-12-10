using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioVisualizer : MonoBehaviour
{

    private AudioSource source;


    // private static float[] spectrum { get; set; }
    public static float strikeValue;

    int qSamples = 1024;  // array size
    float refValue = 0.1f; // RMS value for 0 dB
    float threshold = 0.02f;      // minimum amplitude to extract pitch
    float rmsValue;   // sound level - RMS
    float dbValue;    // sound level - dB
    public static float pitchValue; // sound pitch - Hz


    private float[] samples;
    private float[] spectrum;
    private int fSample;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();

        samples = new float[qSamples];
        spectrum = new float[qSamples];
        fSample = AudioSettings.outputSampleRate;
    }

    // Update is called once per frame
    void Update()
    {
        //  source.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);


        //for (int i = 0; i < (spectrum.Length); i++)
        //{
        //    strikeValue = spectrum[i * 10];
        //    //strikeValue = -strikeValue;
        //    strikeValue = strikeValue * 1000000;
        //    Debug.Log(strikeValue);
        //}


        AnalyzeSound();

    }


    void AnalyzeSound()
    {
        source.GetOutputData(samples, 0); // fill array with samples
        int i;
        float sum = 0;
        for (i = 0; i < qSamples; i++)
        {
            sum += samples[i] * samples[i]; // sum squared samples
        }
        rmsValue = Mathf.Sqrt(sum / qSamples); // rms = square root of average
        dbValue = 20 * Mathf.Log10(rmsValue / refValue); // calculate dB
        if (dbValue < -160) dbValue = -160; // clamp it to -160dB min
                                            // get sound spectrum
        source.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
        float maxV = 0;
        int maxN = 0;
        for (i = 0; i < qSamples; i++)
        { // find max 
            if (spectrum[i] > maxV && spectrum[i] > threshold)
            {
                maxV = spectrum[i];
                maxN = i; // maxN is the index of max
            }
        }
        double freqN = maxN; // pass the index to a float variable
        if (maxN > 0 && maxN < qSamples - 1)
        { // interpolate index using neighbours
            var dL = spectrum[maxN - 1] / spectrum[maxN];
            var dR = spectrum[maxN + 1] / spectrum[maxN];
            freqN += 0.5 * (dR * dR - dL * dL);
        }
       // Debug.Log(pitchValue);
        pitchValue = System.Convert.ToSingle(freqN * (fSample / 2) / qSamples); // convert index to frequency


    }
}
//        public static float GetSpectrumValue(int index)
//    {
//        if (index >= 0 && index < spectrum.Length)
//        {
//            return spectrum[index];
//        }
//        else
//        {
//            return 0f;
//        }
//    }
//}
