using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Unity.Entities;

public partial class HealthSystem : SystemBase
{
    [CanBeNull]
    private HealthBehaviour _healthBehaviour;

    protected override void OnCreate()
    {
        RequireForUpdate<Player>();
    }

    protected override void OnUpdate()
    {
        var player = SystemAPI.GetSingletonEntity<Player>();
        var component = SystemAPI.GetComponentRO<PlayerComponent>(player);
        if (!component.IsValid || _healthBehaviour == null) return;
        _healthBehaviour.Initialize(component.ValueRO.MaxHealth);
        _healthBehaviour.UpdateHealth(component.ValueRO.Health);
    }

    public void AttachUI(HealthBehaviour behaviour)
    {
        _healthBehaviour = behaviour;
    }
}
