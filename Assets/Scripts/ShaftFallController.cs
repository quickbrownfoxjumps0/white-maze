using UnityEngine;
using UnityEngine.SceneManagement;

public class ShaftFallController : MonoBehaviour
{
	public Transform playerTransform;
    public float fallSpeed = 8f;
    public string nextSceneName;

    private bool sceneLoaded = false;

    void Update()
    {
        // playerTransform.position += Vector3.down * fallSpeed * Time.deltaTime;

        float playerY = playerTransform.position.y;
        if (playerY < -100f)
        {
            if (!sceneLoaded)
            {
                sceneLoaded = true;

                SceneManager.sceneLoaded += OnSceneLoaded;
                SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
            }
        }
    }

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		SceneManager.SetActiveScene(scene);

		// Force re-apply lighting
		RenderSettings.skybox = RenderSettings.skybox;
		DynamicGI.UpdateEnvironment();

		// Force refresh reflection probes
		var probes = Object.FindObjectsOfType<ReflectionProbe>();
		foreach (var probe in probes)
		{
			probe.RenderProbe();
		}

		SceneManager.sceneLoaded -= OnSceneLoaded;
	}
}

