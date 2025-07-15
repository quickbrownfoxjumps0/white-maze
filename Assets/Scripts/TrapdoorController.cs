using UnityEngine;

public class TrapdoorController : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private Collider trapdoorCollider;

    private PlayerFallController playerFallController;
    private bool triggered = false;

    void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        trapdoorCollider = GetComponentInChildren<Collider>();

        playerFallController = FindObjectOfType<PlayerFallController>();

        if (playerFallController == null)
            Debug.LogError("PlayerFallController not found in scene!");
    }

    void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;

            if (meshRenderer != null)
                meshRenderer.enabled = false;

            if (trapdoorCollider != null)
                trapdoorCollider.enabled = false;

            playerFallController?.StartFalling();
        }
    }
}

