using UnityEngine;

[System.Serializable]
public abstract class ClumpController : MonoBehaviour
{
    private enum ForceType
    {
        Force,
        RelativeForce,
        RelativeTorque,
        Torque,
    }

    // Take directional input
    // Calculate the force direction based on input
    // Apply force in that direction
    [Header("Controller setup")]
    [SerializeField] protected SphereCollider Collider;
    [SerializeField] protected Rigidbody Body;
    [SerializeField] private Transform Telemetry;
    [SerializeField] private bool _showTelemetry;

    [Header("Movement")]
    [SerializeField] private ForceType _forceType;
    [SerializeField] private float _force;
    [SerializeField] private ForceMode _forceMode;
    [SerializeField] private float _maxSpeed;

    [Header("DEBUG ==========")]
    [SerializeField] private Vector3 _directionalInput;

    private void Awake()
    {
        Telemetry.gameObject.SetActive(false);
        Telemetry.SetParent(null);
    }

    public virtual void Update()
    {
        ShowTelemetry(_showTelemetry);
        CalculateForceDirection();
    }

    public virtual void FixedUpdate()
    {
        ApplyDirectionalForce();
    }

    protected void SetDirectionalInput(Vector2 input)
    {
        _directionalInput = new Vector3(input.x, 0, input.y);
    }

    public void ConfigureController(float radius, float torque, float maxSpeed)
    {
        Collider.radius = radius;
        _force = torque;
        _maxSpeed = maxSpeed;
    }

    public void SetMoveConfig(float force)
    {
        _force = force;
    }

    private void CalculateForceDirection()
    {
        if (_directionalInput != Vector3.zero)
        {
            var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
            var skewedInput = matrix.MultiplyPoint3x4(_directionalInput);

            var relative = (Telemetry.position + skewedInput)
                - Telemetry.position;
            var rot = Quaternion.LookRotation(relative, Vector3.up);

            Telemetry.rotation = Quaternion.RotateTowards(
                Telemetry.rotation, rot, 360);
        }
    }

    private void ApplyDirectionalForce()
    {
        if (_directionalInput != Vector3.zero
            && Body.velocity.magnitude < _maxSpeed)
        {
            switch (_forceType)
            {
                case ForceType.Force:
                    Body.AddForce(
                        _force * Time.deltaTime * Telemetry.forward,
                        _forceMode);
                    break;

                case ForceType.RelativeForce:
                    Body.AddRelativeForce(
                        _force * Time.deltaTime * Telemetry.forward,
                        _forceMode);
                    break;

                case ForceType.RelativeTorque:
                    Body.AddRelativeTorque(
                        _force * Time.deltaTime * Telemetry.right,
                        _forceMode);
                    break;

                case ForceType.Torque:
                    Body.AddTorque(
                        _force * Time.deltaTime * Telemetry.right,
                        _forceMode);
                    break;

                default:
                    throw new System.Exception("Unmapped ForceType");
            }
        }
    }

    private void ShowTelemetry(bool show)
    {
        Telemetry.gameObject.SetActive(show);

        if (show) Telemetry.position = transform.position;
    }
}
