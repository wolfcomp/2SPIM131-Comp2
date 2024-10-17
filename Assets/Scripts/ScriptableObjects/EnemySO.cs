using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/Enemy")]

public class EnemySO : ScriptableObject
{
    public GameObject Prefab;
    public byte Health;
    public byte Damage;
    public float MoveSpeed;
}
