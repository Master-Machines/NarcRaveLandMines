using UnityEngine;
using System.Collections;

public class DestroyAfterTime : MonoBehaviour {
	public float timeToDeath;

	IEnumerator Start () {
		yield return new WaitForSeconds(timeToDeath);
		Destroy(gameObject);
	}
}
