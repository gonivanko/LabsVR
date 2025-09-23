using UnityEngine;

public class Driver : MonoBehaviour
{
    [SerializeField] float velocity = -4f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Start");
    }

    // Update is called once per frame
    void Update()
    {
        float delta = Input.GetAxis("Vertical");
        float timeDelta = Time.deltaTime;
        float verticalDelta = timeDelta * velocity * delta;

        Debug.Log("Update");
        transform.Translate(verticalDelta, 0, 0);
    }
}
