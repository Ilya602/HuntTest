using UnityEngine;
using Assets.Scripts.Merge;

namespace Assets.Scripts.Scriptables
{
    [CreateAssetMenu(fileName = "New Merge Level Data")]
    public class MergeLevelData : ScriptableObject
    {
        public int currFood;

        [SerializeField] private int creaturePrice;
        public int CreaturePrice => creaturePrice;

        [SerializeField] private int cellPrice;
        public int CellPrice => cellPrice;

        [SerializeField] private int maxEvoLevel = 3;
        public int MaxEvoLevel => maxEvoLevel;

        [SerializeField, Space] private MergeObject objectPref;
        public MergeObject ObjectPref => objectPref;
    }
}
