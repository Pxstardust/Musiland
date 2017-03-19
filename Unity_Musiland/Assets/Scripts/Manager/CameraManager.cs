using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class CameraManager : MonoBehaviour {

    // ===== BLOOM ===== //
    Bloom bloom;
    bool isblooming;
    float bloomduration;
    float bloomstart;
    float bloomdestination;

    NoiseAndScratches noise;
    bool isnoisy;
    float noiseduration;
    float noisestart;
    float noisedestination;


	// Use this for initialization
	void Start () {
        bloom = Camera.main.GetComponent<Bloom>();
        noise = Camera.main.GetComponent<NoiseAndScratches>();
	}
	
	// Update is called once per frame
	void Update () {

     
		if (isblooming)
        {
            bloom.bloomIntensity = Mathf.Lerp(bloom.bloomIntensity, bloomdestination, (Time.time - bloomstart) / bloomduration);
          //if (bloom.bloomIntensity == 0.3f) { isblooming = false; bloomstart = 0; bloomdestination = 0; bloomduration = 0; }
        }

        if (isnoisy)
        {
            noise.grainIntensityMax = Mathf.Lerp(noise.grainIntensityMax, noisedestination, (Time.time - noisestart) / noiseduration);
            noise.grainIntensityMin = Mathf.Lerp(noise.grainIntensityMin, noisedestination, (Time.time - noisestart) / noiseduration);
        }

	}

    public void SwitchBloomOn(float duration, float to, float howlong = 0)
    {
        isblooming = true;
        bloomstart = Time.time;
        bloomduration = duration;
        bloomdestination = to;
        if (howlong > 0)
        {
            StartCoroutine(BloomMinuteur(howlong));
            //howlong = 0;
        }
    }
        IEnumerator BloomMinuteur(float howlong)
    {

        yield return new WaitForSeconds(howlong);

        SwitchBloomOn(3, 0);
    }

    public void SwitchNoiseOn (float duration, float to, float howlong = 0)
    {
        isnoisy = true;
        noisestart = Time.time;
        noiseduration = duration;
        noisedestination = to;
        if (howlong > 0)
        {
            StartCoroutine(NoiseMinuteur(howlong));
        }   


    }

    IEnumerator NoiseMinuteur(float howlong)
    {
        yield return new WaitForSeconds(howlong);
        SwitchNoiseOn(0.5f, 0);
    }
}
