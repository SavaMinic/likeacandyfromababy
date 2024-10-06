using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BabyController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float pointRange = 10f;
    [SerializeField] private float absoluteDistanceLimit = 10f;
    [SerializeField] private float reachingPoint = 0.5f;
    [SerializeField] private float changePointManuallyAfterSeconds = 10f;
    [SerializeField] private Rigidbody charBody;
    [SerializeField] private Transform charTransform;
    [SerializeField] private LayerMask candyLayerMask;
    [SerializeField] private float magnitudeForDropping = 2f;
    [SerializeField] private Vector2 throwForce;
    [SerializeField] private float unableToPickDelay = 2f;
    private HitFeedback _hitFeedback;

    private Vector3 _nextPoint;
    private float _timeToChangeNexPoint = float.MaxValue;
    private ThrowableObject _throwableObject;
    private float _unableToPickTime;

    private void Awake()
    {
        _hitFeedback = GetComponentInChildren<HitFeedback>();
    }

    private void Start()
    {
        UpdateNextPoint();
    }

    private void Update()
    {
        if (Time.time >= _timeToChangeNexPoint) UpdateNextPoint();
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
            // go back towards center
            //Debug.LogError($"{gameObject.name} too far away {_nextPoint}");
            _nextPoint = transform.position + (Vector3.zero - transform.position).normalized * pointRange;

        _timeToChangeNexPoint = Time.time + changePointManuallyAfterSeconds;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & candyLayerMask) != 0)
        {
            var currentThrowable = collision.gameObject.GetComponent<ThrowableObject>();
            
            var magnitude = currentThrowable.VelocityMagnitude;
            if (magnitude > magnitudeForDropping && _throwableObject != null)
            {
                // throw it away
                var throwVector = throwForce.x * transform.forward;
                throwVector.y = throwForce.y;
                _throwableObject.Throw(throwVector);
                _throwableObject = null;
                
                // add time
                _unableToPickTime = Time.time + unableToPickDelay;
            }
            
            var candy = currentThrowable.GetComponent<CandyObject>();
            if (candy != null && _throwableObject == null && Time.time > _unableToPickTime)
            {
                currentThrowable.Hold(transform, new Vector3(0,0.25f, 0.1f));
                _throwableObject = currentThrowable;
            }
        }
    }

    public void ShowHitFeedback()
    {
        _hitFeedback.DoHitAnimation();
    }
}