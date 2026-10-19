using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform castOrigin;
    [SerializeField] private Transform targetMarker;
    [SerializeField] private float waterLevel = 0f;
    [SerializeField] private float minDistance = 4f;
    [SerializeField] private float maxDistance = 12f;
    [SerializeField] private TMP_Text distanceText;
    [SerializeField] private TMP_Text scoreText;

    public float TargetDistance { get; private set; }

    void Start() => NewRound();

    public void NewRound()
    {
        TargetDistance = Random.Range(minDistance, maxDistance);

        Vector3 fwd = castOrigin.forward;
        fwd.y = 0;
        fwd.Normalize();

        Vector3 pos = castOrigin.position + fwd * TargetDistance;
        pos.y = waterLevel;
        targetMarker.position = pos;

        distanceText.text = $"Aim for: {TargetDistance:F1} m";
        scoreText.text = "";
    }

    public int ComputeScore(Vector3 landingPoint)
    {
        Vector3 d = landingPoint - castOrigin.position;
        // ignor inaltimea, axa y
        d.y = 0;                                   
        float thrown = d.magnitude;

        // scor = 100 - eroare * 15 sau 0 :)
        float error = Mathf.Abs(thrown - TargetDistance);
        int score = Mathf.Max(0, Mathf.RoundToInt(100f - error * 15f));

        scoreText.text = $"You threw {thrown:F1} m\nError: {error:F1} m\nScore: {score}";
        return score;
    }
}