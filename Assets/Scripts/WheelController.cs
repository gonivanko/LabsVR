using UnityEngine;

public class WheelController : MonoBehaviour
{
    [SerializeField] private WheelCollider frontRight;
    [SerializeField] private WheelCollider frontLeft;
    [SerializeField] private WheelCollider backRight;
    [SerializeField] private WheelCollider backLeft;

    [SerializeField] private Transform frontRightTransform;
    [SerializeField] private Transform frontLeftTransform;
    [SerializeField] private Transform backRightTransform;
    [SerializeField] private Transform backLeftTransform;

    [SerializeField] private float acceleration = 500f;
    [SerializeField] private float brakeForce = 300f;

    [SerializeField] private float maxSteerAngle = 30f;

    private float currentAcceleration = 0f;
    private float currentSteerAngle = 0f;
    private float currentBrakeForce = 0f;

    private static void UpdateWheel(WheelCollider collider, Transform transform)
    {
        collider.GetWorldPose(out var position, out var rotation);

        transform.SetPositionAndRotation(position, rotation);
    }

    private void FixedUpdate()
    {
        currentAcceleration = acceleration * Input.GetAxis("Vertical");
        currentSteerAngle = maxSteerAngle * Input.GetAxis("Horizontal");

        currentBrakeForce = (Input.GetKey(KeyCode.Space) ? brakeForce : 0f);


        frontRight.motorTorque = currentAcceleration;
        frontLeft.motorTorque = currentAcceleration;

        frontRight.brakeTorque = currentBrakeForce;
        frontLeft.brakeTorque = currentBrakeForce;

        frontLeft.steerAngle = currentSteerAngle;
        frontRight.steerAngle = currentSteerAngle;

        UpdateWheel(frontRight, frontRightTransform);
        UpdateWheel(frontLeft, frontLeftTransform);
        UpdateWheel(backRight, backRightTransform);
        UpdateWheel(backLeft, backLeftTransform);
    }

    
}
