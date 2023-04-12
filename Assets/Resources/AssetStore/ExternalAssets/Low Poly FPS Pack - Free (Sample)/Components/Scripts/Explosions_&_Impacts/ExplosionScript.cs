using UnityEngine;
using System.Collections;

// ----- Low Poly FPS Pack Free Version -----
public class ExplosionScript : MonoBehaviour {

	[Header("Customizable Options")]
	//How long before the explosion prefab is destroyed
	public float despawnTime = 10.0f;
	//How long the light flash is visible
	public float lightDuration = 0.02f;
	[Header("Light")]
	public Light lightFlash;

	[Header("Audio")]
	public AudioClip[] explosionSounds;
	public AudioSource audioSource;

	private void Start () {
		//Start the coroutines
		StartCoroutine (DestroyTimer ());
		StartCoroutine (LightFlash ());

		int random = Random.Range(0, 6);
		switch (random)
		{
			case 0:
				SoundManager.instance.Play3DSFX("explosion-1", transform.position);
				break;
			case 1:
				SoundManager.instance.Play3DSFX("explosion-2", transform.position);
				break;
			case 2:
				SoundManager.instance.Play3DSFX("explosion-3", transform.position);
				break;
			case 3:
				SoundManager.instance.Play3DSFX("explosion-4", transform.position);
				break;
			case 4:
				SoundManager.instance.Play3DSFX("explosion-5", transform.position);
				break;
			case 5:
				SoundManager.instance.Play3DSFX("explosion-6", transform.position);
				break;
		}
	}

	private IEnumerator LightFlash () {
		//Show the light
		lightFlash.GetComponent<Light>().enabled = true;
		//Wait for set amount of time
		yield return new WaitForSeconds (lightDuration);
		//Hide the light
		lightFlash.GetComponent<Light>().enabled = false;
	}

	private IEnumerator DestroyTimer () {
		//Destroy the explosion prefab after set amount of seconds
		yield return new WaitForSeconds (despawnTime);
		Destroy (gameObject);
	}
}
// ----- Low Poly FPS Pack Free Version -----