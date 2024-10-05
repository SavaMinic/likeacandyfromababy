using UnityEngine;

public class ChildController : MonoBehaviour
{
    private static readonly int IsWalkingAnimKey = Animator.StringToHash("isWalking");
    private static readonly int JumpAnimKey = Animator.StringToHash("jump");

    [SerializeField] private string horizontalAxis = "Horizontal";

    [SerializeField] private string verticalAxis = "Vertical";

    [SerializeField] private KeyCode jumpKey = KeyCode.Space;

    [SerializeField] private KeyCode holdThrowKey = KeyCode.E;

    [SerializeField] private LayerMask jumpLayerMask;

    [SerializeField] private float speed;

    [SerializeField] private float rotationSpeed;

    [SerializeField] private float jumpForce = 2.5f;

    [SerializeField] private Vector2 throwForce;

    [SerializeField] private Transform charTransform;

    [SerializeField] private Rigidbody charBody;

    [SerializeField] private Animator animator;

    private GrabController _grabController;
    private ThrowableObject _throwableObject;

    private Camera _mainCamera;

    private float _rotation;
    private bool _isJumping;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _grabController = GetComponentInChildren<GrabController>();
    }

    private void Update()
    {
        var isMoving = Input.GetAxis(horizontalAxis) != 0f || Input.GetAxis(verticalAxis) != 0f;

        var inputVector = new Vector3(Input.GetAxis(horizontalAxis), 0, Input.GetAxis(verticalAxis));
        var cameraFacing = _mainCamera.transform.eulerAngles.y;
        var moveVector = Quaternion.Euler(0, cameraFacing, 0) * inputVector;

        var velocity = moveVector * speed;
        charBody.velocity = new Vector3(velocity.x, charBody.velocity.y, velocity.z);

        // Rotation
        if (isMoving)
        {
            var rotationStep = rotationSpeed * Time.deltaTime;
            var newDirection = Vector3.RotateTowards(transform.forward, moveVector.normalized, rotationStep, 0.0f);
            charTransform.rotation = Quaternion.LookRotation(newDirection);
        }
        else
        {
            charBody.angularVelocity = Vector3.zero;
        }

        // Jumping
        if (!_isJumping && Input.GetKeyDown(jumpKey))
        {
            _isJumping = true;
            charBody.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);
            animator.SetTrigger(JumpAnimKey);
        }

        // hold/throw
        if (_throwableObject == null && Input.GetKeyDown(holdThrowKey))
        {
            if (_grabController.LatestGrabObject != null)
            {
                _throwableObject = _grabController.LatestGrabObject;
                // hold and lock in place
                _throwableObject.Hold(transform);
            }
        }
        else if (_throwableObject != null && Input.GetKeyUp(holdThrowKey))
        {
            if (_throwableObject != null)
            {
                // throw it away
                var throwVector = throwForce.x * transform.forward;
                throwVector.y = throwForce.y;
                _throwableObject.Throw(throwVector);
                
                _throwableObject = null;
            }
        }

        animator.SetBool(IsWalkingAnimKey, isMoving);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & jumpLayerMask) != 0) _isJumping = false;
    }
}