using UnityEngine;

public class Bobber : MonoBehaviour
{
    [SerializeField] private FishingRod rod;   // legat din Inspector
    private Rigidbody rb;
    private bool hasLanded;

    void Awake() => rb = GetComponent<Rigidbody>();

    public void Launch(Vector3 velocity)
    {
        hasLanded = false;
        transform.SetParent(null);          
        rb.isKinematic = false;
        rb.linearVelocity = velocity;     
    }

    public void Attach(Transform tip)
    {
        rb.linearVelocity = Vector3.zero;   
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        transform.SetParent(tip);
        transform.localPosition = Vector3.zero;
    }

    void OnTriggerEnter(Collider other)
    {
        if (hasLanded || !other.CompareTag("Water")) return;
        hasLanded = true;
        rod.OnBobberLanded(transform.position);
    }
}