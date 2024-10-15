using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public float NextOffset() => _rectTransform.rect.x + _rectTransform.rect.width + 7;
}
