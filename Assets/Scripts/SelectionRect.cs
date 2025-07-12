using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelectionRect : MonoBehaviour
{
    [SerializeField] RectTransform selectionBox;
    [SerializeField] GameObject moveOrderMarkerPrefab; 

    Vector2 startPos;
    readonly List<UnitMovement> selectedUnits = new();

    void HandleRightClick()
    {
        if (!Input.GetMouseButtonDown(1)) return;

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPos.z = 0f;

        foreach (var unit in selectedUnits)
            unit.MoveTo(worldPos);

        if (moveOrderMarkerPrefab) 
            Instantiate(moveOrderMarkerPrefab, worldPos, Quaternion.identity);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            selectedUnits.Clear();
            startPos = Input.mousePosition;
            selectionBox.gameObject.SetActive(true);
        }
        else if (Input.GetMouseButton(0))
        {
            Vector2 currentPos = Input.mousePosition;
            Vector2 boxStart = new(Mathf.Min(startPos.x, currentPos.x), Mathf.Min(startPos.y, currentPos.y));
            Vector2 boxSize = new(Mathf.Abs(currentPos.x - startPos.x), Mathf.Abs(currentPos.y - startPos.y));

            selectionBox.anchoredPosition = boxStart;
            selectionBox.sizeDelta = boxSize;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            selectionBox.gameObject.SetActive(false);
            SelectUnitsInBox();
        }

        HandleRightClick();
    }

    void SelectUnitsInBox()
    {
        foreach (var unit in FindObjectsByType<UnitMovement>(FindObjectsSortMode.None))
        {
            var unitSel = unit.GetComponent<UnitSelection>();
            unitSel.SetSelected(false);

            Vector2 screenPos = Camera.main.WorldToScreenPoint(unit.transform.position);
            if (RectTransformUtility.RectangleContainsScreenPoint(selectionBox, screenPos))
            {
                unitSel.SetSelected(true);
                selectedUnits.Add(unit);
            }
        }
    }
}
