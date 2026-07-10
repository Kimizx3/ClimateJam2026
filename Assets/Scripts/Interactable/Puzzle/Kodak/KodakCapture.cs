using UnityEngine;

public class KodakCapture : MonoBehaviour
{
    [SerializeField] private Camera kodakCamera;
    [SerializeField] private float captureDistance = 15f;
    [SerializeField] private LayerMask photographableLayer;
    [SerializeField] private PhotoInventory photoInventory;

    public void TryCapture()
    {
        Ray ray = kodakCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out RaycastHit hit, captureDistance, photographableLayer))
        {
            Photographable target = hit.collider.GetComponentInParent<Photographable>();

            if (target != null)
            {
                photoInventory.AddPhoto(target.photoData);
                Debug.Log($"Captured: {target.photoData.displayName}");
            }
        }
    }
}
