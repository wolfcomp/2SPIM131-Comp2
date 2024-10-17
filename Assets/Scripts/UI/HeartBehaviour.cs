using UnityEngine;
using UnityEngine.UI;

public class HeartBehaviour : MonoBehaviour
{
    [SerializeField]
    private Sprite _full;
    [SerializeField]
    private Sprite _half;
    [SerializeField]
    private Sprite _empty;
    [SerializeField]
    private Image _heartImageContainer;

    public void UpdateHealth(byte type)
    {
        _heartImageContainer.sprite = type switch
        {
            0 => _empty,
            1 => _half,
            _ => _full
        };
    }
}
