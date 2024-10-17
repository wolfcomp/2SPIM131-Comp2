using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class HealthBehaviour : MonoBehaviour
{
    [SerializeField]
    private List<HeartBehaviour> _hearts;

    [SerializeField]
    private HeartBehaviour _heartPrefab;

    [SerializeField]
    private GameObject _listObject;
    
    void Start()
    {
        var entitySystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<HealthSystem>();
        entitySystem.AttachUI(this);
    }

    public void Initialize(byte maxHealth)
    {
        var containers = maxHealth / 2;
        if (_hearts.Count == containers) return;
        for (var i = 0; i < containers; i++)
        {
            var container = Instantiate(_heartPrefab, _listObject.transform);
            container.transform.localPosition = new Vector3(i * 48 + 24, 0, 0);
            _hearts.Add(container);
        }
    }

    public void UpdateHealth(byte newHealth)
    {
        for (var i = 0; i < _hearts.Count; i++)
        {
            var modified = newHealth - 2 * i;
            if (modified < 0)
                modified = 0;
            _hearts[i].UpdateHealth((byte)modified);
        }
    }
}
