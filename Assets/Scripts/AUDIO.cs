using UnityEngine;
using System.Collections;

public class AUDIO : MonoBehaviour {
	public Material[] mats;
	private GameObject[] lights;
	private bool started = false;
	private float extraIntensity = 0f;
	public AudioClip[] daClips;
	// Use this for initialization
	IEnumerator Start () {
		yield return new WaitForSeconds(.5f);
		audio.Play();
		lights = GameObject.FindGameObjectsWithTag("light");
		started = true;
		yield return new WaitForSeconds(3f);
		audio.clip = daClips[Random.Range(0, daClips.Length)];
		audio.Play ();
	}
	
	// Update is called once per frame
	void Update () {
		if(started) {
			DoMusicShit();
			extraIntensity *= 0.8f;
			extraIntensity -= .005f;
			if(extraIntensity < 0) {
				extraIntensity = 0;
			}
		}
		
	}
	
	void DoMusicShit() {
		float[] daJams = new float[256];
		audio.GetSpectrumData(daJams, 1, FFTWindow.Rectangular);
		float daBeat = 0f;
		for(int i = 0; i < 256; i++) {
			daBeat += Mathf.Pow(daJams[i], 3f);
		}
		extraIntensity += daBeat;
		for(int i = 0; i < lights.Length; i++) {
			lights[i].light.range = 25f + extraIntensity * 120f;
			//mats[i].SetFloat("_OutlineWidth", .1f * daBeat);
		}
	}
}
