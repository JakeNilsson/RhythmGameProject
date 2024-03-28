using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Spectrum : MonoBehaviour
{
    private float[] audioSpectrum;
    public static float val {get; private set;}
    void Start()
    {
        audioSpectrum = new float[128];
    }

    void Update()
    {
        AudioListener.GetSpectrumData(audioSpectrum, 0, FFTWindow.Hamming);
        if (audioSpectrum != null && audioSpectrum.Length > 0) {
            val = audioSpectrum[0] * 100;
        }
    }
}
