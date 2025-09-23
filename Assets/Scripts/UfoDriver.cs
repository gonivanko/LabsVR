using UnityEngine;

public class UfoDriver : MonoBehaviour
{
    public Transform center;

    [SerializeField] float radius = 10f;
    [SerializeField] float velocity = 4f;
    
    [SerializeField] float UfoHeight = 7.5f;

    float angle = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = new Vector3(center.position.x + radius, UfoHeight, center.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        float timeDelta = Time.deltaTime;
        angle += (velocity / radius) * timeDelta;
        float x = center.position.x + radius * Mathf.Cos(angle);
        float z = center.position.z + radius * Mathf.Sin(angle);

        transform.position = new Vector3(x, UfoHeight, z);
    }
}
