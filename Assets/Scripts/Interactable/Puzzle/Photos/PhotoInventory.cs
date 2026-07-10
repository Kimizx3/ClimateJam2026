using System.Collections.Generic;
using UnityEngine;

public class PhotoInventory : MonoBehaviour
{
    [SerializeField] private Transform photoContainer;

    private HashSet<string> capturedPhotoIds = new HashSet<string>();
    public void AddPhoto(PhotoData photoData)
    {
        if (photoData == null)
        {
            return;
        }

        bool isNewPhoto = capturedPhotoIds.Add(photoData.id);

        if (!isNewPhoto)
        {
            Debug.Log($"Already captured: {photoData.displayName}");
            return;
        }

        if (photoData.inventoryPhotoPrefab != null)
        {
            Instantiate(photoData.inventoryPhotoPrefab, photoContainer);
        }
        
        Debug.Log($"Added photo to inventory: {photoData.displayName}");
    }
}
