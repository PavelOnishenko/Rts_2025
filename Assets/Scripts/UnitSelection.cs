using UnityEngine;

public class UnitSelection : MonoBehaviour
{
    private SpriteRenderer unitSpriteRenderer;
    private bool isSelected;

    private void Start()
    {
        unitSpriteRenderer = GetComponent<SpriteRenderer>();
        isSelected = false;
        unitSpriteRenderer.color = GetColor();
    }

    public void SetSelected(bool newValue)
    {
        isSelected = newValue;
        unitSpriteRenderer.color = GetColor();
    }

    private Color GetColor()
    {
        return isSelected ? Color.red : Color.green;
    }
}
