using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    public GameObject turretPrefab;  // Reference to your turret prefab
    private GameObject currentTurret; // The currently held turret object
    public GameObject MapGenerator;
    public int type = 0;
    public int cost = 0;
    public float multiplicateur = 1;

    private void Start()
    {
        MapGenerator = GameObject.Find("MapGenerator");
        multiplicateur = MapGenerator.GetComponent<MapGenerator>().multiplicateur;
        cost = (int)(cost * multiplicateur);
    }

    private void Update()
    {
        // Check if the mouse is over the TMP image area
        if (IsMouseOverTMP())
        {
            // If the left mouse button is clicked, instantiate a turret at the mouse position
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos = GetMouseWorldPosition();
                currentTurret = Instantiate(turretPrefab, mousePos, Quaternion.identity);
            }

        }
        // If a turret is being held and the left mouse button is held down, drag the turret
        if (currentTurret != null && Input.GetMouseButton(0))
        {
            Vector3 mousePos = GetMouseWorldPosition();
            currentTurret.transform.position = new Vector3(mousePos.x, 1f, mousePos.z);
        }

        if (currentTurret != null && Input.GetMouseButtonUp(0))
        {
            Vector3 mousePos = GetMouseWorldPosition();
            bool canBuy = MapGenerator.GetComponent<MapGenerator>().addMoney(-cost);
            if (canBuy)
                if (!MapGenerator.GetComponent<MapGenerator>().findTowerPos(mousePos.x, mousePos.z, type))
                    MapGenerator.GetComponent<MapGenerator>().addMoney(cost);
            else 
                Debug.Log("Not enough money");
            Destroy(currentTurret);
            currentTurret = null;
        }
    }

    // Check if the mouse is over the TMP image
    private bool IsMouseOverTMP()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 localMousePosition = rectTransform.InverseTransformPoint(Input.mousePosition);

        return rectTransform.rect.Contains(localMousePosition);
    }

    // Get the mouse position in world space
    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, 0);
        if (groundPlane.Raycast(ray, out float hitDistance))
        {
            Vector3 hitPoint = ray.GetPoint(hitDistance);
            return hitPoint;
        }
        return Vector3.zero; // Return (0,0,0) if the ray doesn't hit the ground
    }
}
