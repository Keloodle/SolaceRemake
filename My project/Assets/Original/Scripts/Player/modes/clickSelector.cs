using UnityEngine;
using UnityEngine.EventSystems; // 1

public class clickSelector : MonoBehaviour
    , IPointerClickHandler
// ... And many more available!
{
    public int thisMode;
    SpriteRenderer sprite;
    Color target = Color.red;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (sprite)
            sprite.color = Vector4.MoveTowards(sprite.color, target, Time.deltaTime * 10);
    }

    public void OnPointerClick(PointerEventData eventData) // 3
    {
        GameObject.FindObjectOfType<modeSelector>().combatMode(thisMode);
    }
}