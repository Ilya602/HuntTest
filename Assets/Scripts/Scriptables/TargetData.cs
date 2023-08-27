using UnityEngine;

namespace Assets.Scripts.Scriptables
{
    [CreateAssetMenu(fileName = "New Target Data")]
    public class TargetData : ScriptableObject
    {
        [SerializeField] private int moveSpeed;
        public int MoveSpeed => moveSpeed;

        [SerializeField] private int health;
        public int Health => health;
    }
}