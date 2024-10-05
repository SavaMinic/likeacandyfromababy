using UnityEngine;

public class GrabController : MonoBehaviour
{
    [SerializeField] private LayerMask grabLayerMask;

    public ThrowableObject LatestGrabObject { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & grabLayerMask) != 0)
        {
            LatestGrabObject = other.gameObject.GetComponent<ThrowableObject>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (LatestGrabObject != null && LatestGrabObject.gameObject == other.gameObject)
        {
            LatestGrabObject = null;
        }
    }
}