using UnityEngine;

public class Driver : MonoBehaviour
{
    [SerializeField] float velocity = -10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Start");
    }

    // Update is called once per frame
    void Update()
    {
        float verticalDelta = Input.GetAxis("Vertical");
        float horizontalDelta = Input.GetAxis("Horizontal");
        float timeDelta = Time.deltaTime;
        float verticalMovement = timeDelta * velocity * verticalDelta;
        float horizontalRotation = timeDelta * horizontalDelta * verticalDelta * 100;

        transform.Rotate(0, horizontalRotation, 0);
        transform.Translate(verticalMovement, 0, 0);

    }

}
