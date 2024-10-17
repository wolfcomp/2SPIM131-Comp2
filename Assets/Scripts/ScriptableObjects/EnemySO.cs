using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/Enemy")]
public class EnemySo : ScriptableObject
{
    public GameObject Prefab;
    public byte Health;
    public byte Damage;
    public float MoveSpeed;
}
