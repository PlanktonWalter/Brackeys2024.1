using UnityEngine;
using UnityEngine.UI;

public sealed class ItemDisplayUI : MonoBehaviour
{

    [SerializeField] private Image _image;

    public void Init(Item item)
    {
        _image.sprite = item.Sprite;
    }

}
