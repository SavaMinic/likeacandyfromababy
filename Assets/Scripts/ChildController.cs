using System.Collections;
using UnityEngine;

public class ChildController : MonoBehaviour
{
    private static readonly int IsWalkingAnimKey = Animator.StringToHash("isWalking");
    private static readonly int JumpAnimKey = Animator.StringToHash("jump");
    private static readonly int CryAnimKey = Animator.StringToHash("cry");
    private static readonly int IsCryingAnimKey = Animator.StringToHash("isCrying");

    [SerializeField] private int playerIndex;

    [SerializeField] private string horizontalAxis = "Horizontal";

    [SerializeField] private string verticalAxis = "Vertical";

    [SerializeField] private KeyCode jumpKey = KeyCode.Space;

    [SerializeField] private KeyCode holdThrowKey = KeyCode.E;

    [SerializeField] private LayerMask jumpLayerMask;

    [SerializeField] private LayerMask hitLayerMask;

    [SerializeField] private AnimationCurve hitMagnitudeToDamage;

    [SerializeField] private float invulnerableDelay = 0.5f;

    [SerializeField] private float speed;

    [SerializeField] private float rotationSpeed;

    [SerializeField] private float jumpForce = 2.5f;

    [SerializeField] private float maximumHitPoints = 100;

    [SerializeField] private float cryDelay = 3f;

    [SerializeField] private Vector2 throwForce;

    [SerializeField] private Transform charTransform;

    [SerializeField] private Rigidbody charBody;

    [SerializeField] private Animator animator;

    private GameManager _gameManager;
    private GrabController _grabController;
    private HitFeedback _hitFeedback;

    private float _hitPoints;
    private float _invulnerableTime;
    private bool _isJumping;
    private IEnumerator _cryAnimation;

    private Camera _mainCamera;

    private float _rotation;
    private ThrowableObject _throwableObject;
    private ThrowableObject _thrownObject;

    private bool IsAlive => _hitPoints > 0;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _grabController = GetComponentInChildren<GrabController>();
        _hitFeedback = GetComponentInChildren<HitFeedback>();
        _gameManager = FindObjectOfType<GameManager>();

        _hitPoints = maximumHitPoints;
    }

    private void Update()
    {
        var inputVector = new Vector3(Input.GetAxis(horizontalAxis), 0, Input.GetAxis(verticalAxis));
        var cameraFacing = _mainCamera.transform.eulerAngles.y;
        var moveVector = Quaternion.Euler(0, cameraFacing, 0) * inputVector;

        if (IsAlive)
        {
            var velocity = moveVector * speed;
            charBody.velocity = new Vector3(velocity.x, charBody.velocity.y, velocity.z);
        }

        // Rotation
        var isMoving = Input.GetAxis(horizontalAxis) != 0f || Input.GetAxis(verticalAxis) != 0f;
        if (IsAlive && isMoving)
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
        if (IsAlive && !_isJumping && Input.GetKeyDown(jumpKey))
        {
            _isJumping = true;
            charBody.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);
            animator.SetTrigger(JumpAnimKey);
        }

        // hold/throw
        if (IsAlive && _throwableObject == null && Input.GetKeyDown(holdThrowKey))
        {
            if (_grabController.LatestGrabObject != null)
            {
                _thrownObject = null;
                _throwableObject = _grabController.LatestGrabObject;
                // hold and lock in place
                _throwableObject.Hold(transform);

                var candy = _throwableObject.GetComponent<CandyObject>();
                if (candy != null)
                {
                    candy.GrabByPlayer((eatenCandy) =>
                    {
                        Destroy(eatenCandy);
                        //TODO: increase score
                        _gameManager.IncreaseScore(playerIndex);
                    });
                }
            }
        }
        else if (_throwableObject != null
                 && (Input.GetKeyUp(holdThrowKey) || !IsAlive))
        {
            // throw it away
            var throwVector = throwForce.x * transform.forward;
            throwVector.y = throwForce.y;
            _throwableObject.Throw(throwVector);
            
            var candy = _throwableObject.GetComponent<CandyObject>();
            if (candy != null) { candy.ReleaseIt(); }

            _thrownObject = _throwableObject;
            _throwableObject = null;
        }

        animator.SetBool(IsWalkingAnimKey, isMoving);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & jumpLayerMask) != 0) _isJumping = false;

        // check for hit
        if (IsAlive && Time.time > _invulnerableTime && ((1 << collision.gameObject.layer) & hitLayerMask) != 0)
        {
            var currentThrowable = collision.gameObject.GetComponent<ThrowableObject>();
            if (currentThrowable != null && currentThrowable != _thrownObject)
            {
                var magnitude = currentThrowable.VelocityMagnitude;
                var damage = hitMagnitudeToDamage.Evaluate(magnitude);
                if (damage > 0.1f)
                {
                    _invulnerableTime = Time.time + invulnerableDelay;
                    _hitFeedback.DoHitAnimation();

                    _hitPoints -= damage;
                    if (_hitPoints <= 0)
                    {
                        _hitPoints = 0;
                        if (_cryAnimation != null) StopCoroutine(_cryAnimation);
                        StartCoroutine(_cryAnimation = CryBabyCry());
                    }
                    
                    //Debug.LogWarning($"{gameObject.name} hit {currentThrowable.name}  d:{damage} hp:{_hitPoints}");
                }
            }
        }
    }

    private IEnumerator CryBabyCry()
    {
        animator.SetBool(IsCryingAnimKey, true);
        animator.SetTrigger(CryAnimKey);
        _hitFeedback.ToggleCrying(true);
        
        yield return new WaitForSeconds(cryDelay);
        _hitPoints = maximumHitPoints;
        animator.SetBool(IsCryingAnimKey, false);
        _hitFeedback.ToggleCrying(false);
    }
}