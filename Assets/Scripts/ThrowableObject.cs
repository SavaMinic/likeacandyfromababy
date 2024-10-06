using UnityEngine;

public class ThrowableObject : MonoBehaviour
{
    private Collider _collider;
    private Rigidbody _rigidbody;
    private Transform _oldParent;

    public float VelocityMagnitude => _rigidbody.velocity.magnitude;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    public void Hold(Transform holder, Vector3 holdPosition)
    {
        _rigidbody.isKinematic = true;
        _collider.enabled = false;
        _oldParent = transform.parent;
        transform.SetParent(holder);
        
        // hold it up
        transform.localPosition = holdPosition;
    }

    public void Throw(Vector3 throwVector)
    {
        transform.SetParent(_oldParent);
        _rigidbody.isKinematic = false;
        _collider.enabled = true;
        
        _rigidbody.AddForce(throwVector);
    }
}