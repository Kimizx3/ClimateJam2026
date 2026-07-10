using UnityEngine;

[CreateAssetMenu(menuName = "Photo/PhotoData")]
public class PhotoData : ScriptableObject
{
    public string id;
    public string displayName;
    public Sprite photoSprite;
    public GameObject inventoryPhotoPrefab;
}
