using UnityEngine;

public class MinimapShaderOverride : MonoBehaviour
{
    public Shader replacementShader;

    void Start()
    {
        Camera cam = GetComponent<Camera>();
        if (cam && replacementShader)
        {
            cam.SetReplacementShader(replacementShader, null);
        }
    }
}

