using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitSelectionHandler : MonoBehaviour
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
            Debug.Log("GetMouseButtonDown.");
            startPos = Input.mousePosition;
            Debug.Log($"startPos = [{startPos}]");
            selectionBox.gameObject.SetActive(true);
        }
        else if (Input.GetMouseButton(0))
        {
            Debug.Log("GetMouseButton.");
            Vector2 currentPos = Input.mousePosition;
            Debug.Log($"currentPos = [{currentPos}]");

            Vector2 boxStart = new(
                Mathf.Min(startPos.x, currentPos.x),
                Mathf.Min(startPos.y, currentPos.y));
            Vector2 boxSize = new(
                Mathf.Abs(currentPos.x - startPos.x),
                Mathf.Abs(currentPos.y - startPos.y));

            selectionBox.anchoredPosition = boxStart;
            selectionBox.sizeDelta = boxSize;

            Debug.Log($"boxStart = [{boxStart}], boxSize = [{boxSize}]");

            var corners = new Vector3[4];
            selectionBox.GetWorldCorners(corners);
            var screenCorners = corners.Select(c => c).ToArray();
            Debug.Log($"screenCorners = [{string.Join(", ", screenCorners.Select(x => $"[{x}]"))}]");
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("GetMouseButtonUp.");
            selectionBox.gameObject.SetActive(false);
            SelectUnitsInBox();
        }
    }

    void SelectUnitsInBox()
    {
        selectedUnits.Clear();
        foreach (var unit in FindObjectsOfType<UnitMovement>())
        {
            Vector2 screenPos = Camera.main.WorldToScreenPoint(unit.transform.position);
            if (RectTransformUtility.RectangleContainsScreenPoint(selectionBox, screenPos))
                selectedUnits.Add(unit);
        }
    }
}
