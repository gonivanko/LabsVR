using UnityEngine;

public class UfoDriver : MonoBehaviour
{
    public Transform center;

    [SerializeField] private float radius = 10f;
    [SerializeField] private float velocity = 4f;
    
    [SerializeField] private float height = 10f;

    float currentAngle = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = new Vector3(center.position.x + radius, height, center.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        float timeDelta = Time.deltaTime;
        currentAngle += (velocity / radius) * timeDelta;
        float x = center.position.x + radius * Mathf.Cos(currentAngle);
        float z = center.position.z + radius * Mathf.Sin(currentAngle);

        transform.position = new Vector3(x, height, z);
    }
}
