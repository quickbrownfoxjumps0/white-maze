using UnityEngine;

public class PlayerFallController : MonoBehaviour
{
    public float fallSpeed = 5f;      // How fast the player should fall
    private bool isFalling = false;

    void Update()
    {
        if (isFalling)
        {
            // Move the player downward
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
        }
    }

    public void StartFalling()
    {
        isFalling = true;
    }

    public void StopFalling()
    {
        isFalling = false;
    }
}

