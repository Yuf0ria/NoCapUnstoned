using UnityEngine;

public class ButtonMover : MonoBehaviour
{
    private RectTransform rect;
    private float xMin;
    private float xMax;
    private float speed;
    private int direction = 1;

    public void Init(float min, float max, float moveSpeed)
    {
        rect = GetComponent<RectTransform>();
        xMin = min;
        xMax = max;
        speed = moveSpeed;
    }

    void Update()
    {
        if (rect == null) return;

        Vector2 pos = rect.anchoredPosition;
        pos.x += direction * speed * Time.deltaTime;

        // Bounce at boundaries
        if (pos.x >= xMax)
        {
            pos.x = xMax;
            direction = -1;
        }
        else if (pos.x <= xMin)
        {
            pos.x = xMin;
            direction = 1;
        }

        rect.anchoredPosition = pos;
    }
}
