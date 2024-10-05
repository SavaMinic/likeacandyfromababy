using UnityEngine;

public class BabyController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float pointRange = 10f;
    [SerializeField] private float absoluteDistanceLimit = 10f;
    [SerializeField] private float reachingPoint = 0.5f;
    [SerializeField] private Rigidbody charBody;
    [SerializeField] private Transform charTransform;

    private Vector3 _nextPoint;

    private void Start()
    {
        UpdateNextPoint();
    }

    private void FixedUpdate()
    {
        var direction = _nextPoint - transform.position;
        if (direction.magnitude < reachingPoint)
        {
            UpdateNextPoint();
            direction = _nextPoint - transform.position;
        }

        charBody.MovePosition(transform.position + Time.deltaTime * moveSpeed * direction.normalized);

        var rotationStep = rotationSpeed * Time.deltaTime;
        var newDirection = Vector3.RotateTowards(transform.forward, direction.normalized, rotationStep, 0.0f);
        charTransform.rotation = Quaternion.LookRotation(newDirection);
    }

    private void UpdateNextPoint()
    {
        var pointOffset = Random.insideUnitCircle * pointRange;
        _nextPoint = transform.position + new Vector3(pointOffset.x, 0f, pointOffset.y);
        if (_nextPoint.magnitude >= absoluteDistanceLimit)
        {
            // go back towards center
            //Debug.LogError($"{gameObject.name} too far away {_nextPoint}");
            _nextPoint = transform.position + (Vector3.zero - transform.position).normalized * pointRange;
        }
    }
}