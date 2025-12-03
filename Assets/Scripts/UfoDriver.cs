using UnityEngine;

public class UfoDriver : MonoBehaviour
{
    public Transform center;

    [SerializeField] private float radius = 10f;
    [SerializeField] private float velocity = 4f;

    [SerializeField] private float height = 10f;

    private float currentAngle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        transform.position = new Vector3(center.position.x + radius, height, center.position.z);
    }

    // Update is called once per frame
    private void Update()
    {
        var timeDelta = Time.deltaTime;
        currentAngle += velocity / radius * timeDelta;
        var x = center.position.x + radius * Mathf.Cos(currentAngle);
        var z = center.position.z + radius * Mathf.Sin(currentAngle);

        transform.position = new Vector3(x, height, z);
    }
}