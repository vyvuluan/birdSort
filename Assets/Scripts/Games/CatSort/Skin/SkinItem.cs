using UnityEngine;
public enum StatusState
{
    Buy, Ads, Own, Equip
}
[CreateAssetMenu(fileName = "SkinItem", menuName = "ScriptableObjects/SkinItem")]
public class SkinItem : ScriptableObject
{
    [SerializeField] private int id;
    [SerializeField] private int price;
    [SerializeField] private StatusState status;
    [SerializeField] private SkinType skinType;
    [SerializeField] private Sprite image;
    [SerializeField] private int levelUnlock;
    [SerializeField] private GameObject prefabBird;

    public SkinItem(int id, int price, StatusState status, SkinType skinType, Sprite image,int levelUnlock, GameObject prefabBird)
    {
        this.id = id;
        this.price = price;
        this.status = status;
        this.skinType = skinType;
        this.image = image;
        this.levelUnlock = levelUnlock;
        this.prefabBird = prefabBird;
    }

    public int Id { get => id; set => id = value; }
    public int Price { get => price; set => price = value; }
    public StatusState Status { get => status; set => status = value; }
    public SkinType SkinType { get => skinType; set => skinType = value; }
    public Sprite Image { get => image; set => image = value; }
    public GameObject PrefabBird { get => prefabBird; set => prefabBird = value; }
    public int LevelUnlock { get => levelUnlock; set => levelUnlock = value; }
}
