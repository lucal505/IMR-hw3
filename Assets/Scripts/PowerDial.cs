using UnityEngine;
using UnityEngine.UI;

public class PowerDial : MonoBehaviour
{
    [SerializeField] private Image fill;
    [SerializeField] private GameObject panel;    
    [SerializeField] private float speed = 0.8f;

    public float Value { get; private set; }
    private bool running;
    private float startTime;

    void Start()
    {
        Refresh();
        if (panel != null) panel.SetActive(false);
    }

    public void StartDial()
    {
        if (panel != null) panel.SetActive(true);
        running = true;
        startTime = Time.time;
    }

    public float StopDial()
    {
        running = false;
        return Value;
    }

    public void ResetDial()
    {
        running = false;
        Value = 0;
        Refresh();
        if (panel != null) panel.SetActive(false);
    }

    void Update()
    {
        if (!running) return;
        Value = Mathf.PingPong((Time.time - startTime) * speed, 1f);
        Refresh();
    }

    void Refresh()
    {
        RectTransform rt = fill.rectTransform;
        rt.anchorMin = new Vector2(0f, 0f);
        rt.anchorMax = new Vector2(Value, 1f);
        rt.offsetMin = new Vector2(5f, 5f);
        rt.offsetMax = new Vector2(-5f, -5f);

        fill.color = Color.Lerp(Color.red, Color.green, Value);
    }
}