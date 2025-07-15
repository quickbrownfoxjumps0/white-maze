using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPortalTrigger : MonoBehaviour
{
	public Transform portalSurface; // Assign the portal quad here in inspector

	private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("Player")) return;

		// Vector from portal to player
		Vector3 toPlayer = other.transform.position - portalSurface.position;

		// Portal's forward direction (normal)
		Vector3 portalForward = -portalSurface.forward;

		// Check if player is entering from the front
		if (Vector3.Dot(toPlayer, portalForward) > 0)
		{
			Debug.Log("Player entered from front - exiting game");
			Application.Quit();

#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#endif
		}
		else
		{
			// Player entered from behind, do nothing
			Debug.Log("Player entered from back - no exit");
		}
	}
}
