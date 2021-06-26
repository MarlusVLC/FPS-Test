// --------------------------------------
// This script is totally optional. It is an example of how you can use the
// destructible versions of the objects as demonstrated in my tutorial.
// Watch the tutorial over at http://youtube.com/brackeys/.
// --------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Destructible : MonoBehaviour {

	[SerializeField] private GameObject destroyedVersion;	// Reference to the shattered version of the object
	[SerializeField] private bool resetScene;
	
	private bool _isDestroyed;
	

	// If the player clicks on the object
	public void Die ()
	{
		if (resetScene)
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		if (!_isDestroyed && destroyedVersion)
		{
			// Spawn a shattered object
			Instantiate(destroyedVersion, transform.position, transform.rotation);
			_isDestroyed = true;
			// Remove the current object
			Destroy(gameObject);
		}

	}

}
