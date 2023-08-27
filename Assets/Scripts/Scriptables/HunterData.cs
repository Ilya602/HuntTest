using UnityEngine;

namespace Assets.Scripts.Scriptables
{
    [CreateAssetMenu(fileName = "New Merge Object Data")]
    public class HunterData : ScriptableObject
    {
        [SerializeField] private int damage;
        public int Damage => damage;

        [SerializeField] private float moveSpeed;
        public float MoveSpeed => moveSpeed;

        [SerializeField] private float maxJumpForce;
        public float MaxJumpForce => maxJumpForce;
    }
}