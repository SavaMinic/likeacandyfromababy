using UnityEngine;

public class ThrowableObject : MonoBehaviour
{
    private Collider _collider;
    private Rigidbody _rigidbody;
    private Transform _oldParent;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    public void Hold(Transform holder)
    {
        _rigidbody.isKinematic = true;
        _collider.enabled = false;
        _oldParent = transform.parent;
        transform.SetParent(holder);
        
        // hold it up
        transform.localPosition = new Vector3(0,0.5f, 0.25f);
    }

    public void Throw(Vector3 throwVector)
    {
        transform.SetParent(_oldParent);
        _rigidbody.isKinematic = false;
        _collider.enabled = true;
        
        _rigidbody.AddForce(throwVector);
    }
}