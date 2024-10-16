using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/Enemy")]

public class EnemySO : ScriptableObject
{
    public GameObject Prefab;
    public float Health;
    public float Damage;
    public float MoveSpeed;
}
