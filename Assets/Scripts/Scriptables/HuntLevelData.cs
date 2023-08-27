using UnityEngine;
using Assets.Scripts.Hunt;

namespace Assets.Scripts.Scriptables
{
    [CreateAssetMenu(fileName = "New Hunt Level Data")]
    public class HuntLevelData : ScriptableObject
    {
        public int currFood;

        [SerializeField, Header("Milliseconds")] private int switchHunterTime;
        public int SwitchHunterTime => switchHunterTime;

        [SerializeField, Space, Header("Prefabs")] private Hunter hunterPref;
        public Hunter HunterPref => hunterPref;

        [SerializeField] private Target[] targetsPrefs;
        public Target[] TargetsPrefs => targetsPrefs;

        [SerializeField, Space, Header("Jump Projection")] private int numPoints = 10;
        public int NumPoints => numPoints;

        [SerializeField] private float timeBetweenPoints = 0.1f;
        public float TimeBetweenPoints => timeBetweenPoints;

        [SerializeField] private LayerMask trajectoryMask;
        public LayerMask TrajectoryMask => trajectoryMask;
    }
}
