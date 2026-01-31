using TMPro;
using UnityEngine;

public class RaycastText : MonoBehaviour
{
    public GameObject floatingTextPrefab;
    public float heightOffset = 1.5f;
    LayerMask layerMask;
    int maskLayer;
    int interactableLayer;
    private const float DISTANCE = 2.5f;

    GameObject currentText;
    Transform currentTarget;

    void Awake()
    {
        layerMask = LayerMask.GetMask("Interactable Object", "Mask");
        maskLayer = LayerMask.NameToLayer("Mask");
        interactableLayer = LayerMask.NameToLayer("Interactable Object");
    }

    void Update()
    {

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out RaycastHit hit, DISTANCE, layerMask))
        {
            // New target
            if (currentTarget != hit.collider.transform)
            {
                ClearText();

                currentTarget = hit.collider.transform;
                ShowText(hit);
            }

            // Keep text positioned above target
            UpdateTextPosition(hit);
        }
        else
        {
            ClearText();
        }

    }

    void ShowText(RaycastHit hit)
    {
        Vector3 pos = hit.collider.bounds.center + Vector3.up * heightOffset;
        currentText = Instantiate(floatingTextPrefab, pos, Quaternion.identity);

        if (hit.collider.gameObject.layer == maskLayer)
        {
            currentText.GetComponent<TextMeshPro>().text = "E to pick up";
        }
        else if (hit.collider.gameObject.layer == interactableLayer)
        {
            currentText.GetComponent<TextMeshPro>().text = "V to grab";
        }
        
    }

    void UpdateTextPosition(RaycastHit hit)
    {
        if (!currentText) return;

        currentText.transform.position =
            hit.collider.bounds.center + Vector3.up * heightOffset;
    }

    void ClearText()
    {
        if (currentText)
            Destroy(currentText);

        currentText = null;
        currentTarget = null;
    }
}
