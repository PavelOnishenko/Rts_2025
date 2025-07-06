using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelectionRect : MonoBehaviour
{
    [SerializeField] GameObject selectionBoxContainerGo;
    [SerializeField] RectTransform selectionBox;
    Vector2 startPos;
    List<UnitMovement> selectedUnits = new();

    void HandleRightClick()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPos.z = 0f;

            foreach (var unit in selectedUnits)
                unit.MoveTo(worldPos); // method in UnitMovement
        }
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

            Vector2 boxStart = new(
                Mathf.Min(startPos.x, currentPos.x),
                Mathf.Min(startPos.y, currentPos.y));
            Vector2 boxSize = new(
                Mathf.Abs(currentPos.x - startPos.x),
                Mathf.Abs(currentPos.y - startPos.y));

            selectionBox.anchoredPosition = boxStart;
            selectionBox.sizeDelta = boxSize;

            var corners = new Vector3[4];
            selectionBox.GetWorldCorners(corners);
            var screenCorners = corners.Select(c => c).ToArray();
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
            var unitSelection = unit.GetComponent<UnitSelection>();
            unitSelection.SetSelected(false);
            Vector2 screenPos = Camera.main.WorldToScreenPoint(unit.transform.position);
            if (RectTransformUtility.RectangleContainsScreenPoint(selectionBox, screenPos))
            {
                unitSelection.SetSelected(true);
                selectedUnits.Add(unit);
            }
        }
    }
}
