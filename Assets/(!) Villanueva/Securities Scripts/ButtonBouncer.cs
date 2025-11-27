using UnityEngine;

public class ButtonBouncer : MonoBehaviour
{
    private RectTransform rect;
    private RectTransform parentPanel;
    private Vector2 direction;
    private float speed;

    public void Init(RectTransform panel, float moveSpeed)
    {
        rect = GetComponent<RectTransform>();
        parentPanel = panel;
        speed = moveSpeed;

        // Random diagonal direction
        direction = new Vector2(
            Random.Range(0, 2) == 0 ? 1 : -1,
            Random.Range(0, 2) == 0 ? 1 : -1
        ).normalized;
    }

    void Update()
    {
        if (rect == null || parentPanel == null) return;

        Vector2 pos = rect.anchoredPosition;
        pos += direction * speed * Time.deltaTime;

        float halfWidth = parentPanel.rect.width / 2f;
        float halfHeight = parentPanel.rect.height / 2f;
        float buttonHalfW = rect.rect.width / 2f;
        float buttonHalfH = rect.rect.height / 2f;

        // Check Horizontal Bounce
        if (pos.x + buttonHalfW >= halfWidth || pos.x - buttonHalfW <= -halfWidth)
        {
            direction.x *= -1;
            pos.x = Mathf.Clamp(pos.x, -halfWidth + buttonHalfW, halfWidth - buttonHalfW);
        }

        // Check Vertical Bounce
        if (pos.y + buttonHalfH >= halfHeight || pos.y - buttonHalfH <= -halfHeight)
        {
            direction.y *= -1;
            pos.y = Mathf.Clamp(pos.y, -halfHeight + buttonHalfH, halfHeight - buttonHalfH);
        }

        rect.anchoredPosition = pos;
    }
}
