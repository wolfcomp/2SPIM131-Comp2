using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HotbarSlotUIBehaviour : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _text;
    [SerializeField]
    private Image _image;
    [SerializeField]
    private RectTransform _rectTransform;

    public void SetText(string text) => _text.text = text;
    public void SetImage(Sprite sprite) => _image.sprite = sprite;
    public float GetOffset(int index) => 34 + 47 * index;

    public void UpdateOffset(int index)
    {
        var position = _rectTransform.localPosition;
        Debug.Log(_rectTransform.offsetMin);
        _rectTransform.localPosition = new Vector3(GetOffset(index), position.y, position.z);
    }
}
