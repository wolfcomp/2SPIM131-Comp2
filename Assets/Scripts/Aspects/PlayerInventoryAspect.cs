using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public readonly partial struct PlayerInventoryAspect : IAspect
{
    public readonly RefRO<Player> Player;
    public readonly RefRO<InventoryComponent> InventoryComponent;
}
