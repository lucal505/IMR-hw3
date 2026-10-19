using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;   
public class FishingRod : MonoBehaviour
{
    private enum State { Ready, Dial, Casting, Flying, Done }

    [Header("Reffs")]
    [SerializeField] private Transform rodTip;
    [SerializeField] private Bobber bobber;
    [SerializeField] private PowerDial dial;
    [SerializeField] private GameManager manager;
    [SerializeField] private Animator animator;         
    [SerializeField] private Transform lineRoot;

    [Header("Cast")]
    [SerializeField] private float maxCastDistance = 15f;
    [SerializeField] private float castAngle = 45f;

    private XRGrabInteractable grab;
    private State state = State.Ready;

    private float pendingPower;
    private Vector3 pendingAim;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
    }

    void OnEnable()  => grab.activated.AddListener(OnTrigger);
    void OnDisable() => grab.activated.RemoveListener(OnTrigger);

    void LateUpdate()
    {
        lineRoot.localScale = Vector3.zero;   
    }

    private void OnTrigger(ActivateEventArgs args)
    {
        switch (state)
        {
            case State.Ready:
                dial.StartDial();
                state = State.Dial;
                break;

            case State.Dial:
                pendingPower = dial.StopDial();

                // directia de lansare 
                pendingAim = transform.forward;
                pendingAim.y = 0;
                pendingAim.Normalize();

                animator.SetTrigger("Cast");
                Invoke(nameof(ReleaseBobber), 0.4f); 
                state = State.Casting;
                break;

            case State.Done:
                ResetRound();
                break;
        }
    }

    // logica de lansare a bobber-ului
    public void ReleaseBobber()
    {
        if (state != State.Casting) return;

        float desiredDistance = pendingPower * maxCastDistance;
        float g = Mathf.Abs(Physics.gravity.y);
        float speed = Mathf.Sqrt(desiredDistance * g);

        float rad = castAngle * Mathf.Deg2Rad;
        Vector3 dir = pendingAim * Mathf.Cos(rad) + Vector3.up * Mathf.Sin(rad);

        bobber.Launch(dir * speed);
        state = State.Flying;
    }

    public void OnBobberLanded(Vector3 landingPoint)
    {
        manager.ComputeScore(landingPoint);
        state = State.Done;
    }

    private void ResetRound()
    {
        animator.ResetTrigger("Cast");
        bobber.Attach(rodTip);
        dial.ResetDial();
        manager.NewRound();
        state = State.Ready;
    }
}