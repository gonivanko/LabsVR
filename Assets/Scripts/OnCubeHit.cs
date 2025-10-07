using UnityEngine;

public class OnCubeHit : MonoBehaviour
{
    private Renderer objectRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("LawnMower"))
        {
            objectRenderer.material.color = Color.goldenRod; // change to any color
        }
    }
}
