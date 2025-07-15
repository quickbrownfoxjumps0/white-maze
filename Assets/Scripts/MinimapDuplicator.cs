using UnityEngine;

public class MinimapGeometryDuplicator : MonoBehaviour
{
    [Header("Tag used by your maze pieces in the scene")]
    public string mazePieceTag = "Maze";

    [Header("Layer to assign to minimap duplicates")]
    public LayerMask minimapLayer;

    [Header("Material for minimap geometry (black unlit)")]
    public Material minimapMaterial;

    void Start()
    {
        DuplicateAllTagged();
    }

    void DuplicateAllTagged()
    {
        GameObject[] mazePieces = GameObject.FindGameObjectsWithTag(mazePieceTag);

        foreach (GameObject piece in mazePieces)
        {
            DuplicatePiece(piece);
        }
    }

    void DuplicatePiece(GameObject original)
    {
        GameObject copy = Instantiate(original, original.transform.position, original.transform.rotation, this.transform);
        copy.transform.localScale = original.transform.localScale;

        // Set layer recursively
        SetLayerRecursively(copy.transform, LayerMaskToLayer(minimapLayer));

		// Replace materials with copies of the black unlit minimap material
		Renderer[] renderers = copy.GetComponentsInChildren<Renderer>();
		foreach (Renderer rend in renderers)
		{
			Material[] newMats = new Material[rend.sharedMaterials.Length];
			for (int i = 0; i < newMats.Length; i++)
			{
				newMats[i] = new Material(minimapMaterial);
			}
			rend.materials = newMats;
		}


        // Remove colliders
        Collider[] colliders = copy.GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            Destroy(col);
        }

        // Remove scripts (optional)
        MonoBehaviour[] scripts = copy.GetComponentsInChildren<MonoBehaviour>();
        foreach (var script in scripts)
        {
            Destroy(script);
        }
    }

    int LayerMaskToLayer(LayerMask layerMask)
    {
        int layer = 0;
        int mask = layerMask.value;
        while (mask > 1)
        {
            mask = mask >> 1;
            layer++;
        }
        return layer;
    }

    void SetLayerRecursively(Transform obj, int newLayer)
    {
        obj.gameObject.layer = newLayer;
        foreach (Transform child in obj)
        {
            SetLayerRecursively(child, newLayer);
        }
    }
}

