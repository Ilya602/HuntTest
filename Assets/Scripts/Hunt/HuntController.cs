using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Assets.Scripts.General;
using Assets.Scripts.Scriptables;
using PathCreation;
using PathCreation.Examples;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Hunt
{
    public class HuntController : MonoBehaviour
    {
        [SerializeField] private UIController uiCont;
        [SerializeField] private InputController inputCont;
        [SerializeField] private PathCreator path;
        [SerializeField] private GeneratePathExample pathGenerator;
        [SerializeField] private CinemachineVirtualCamera cameraCont;
        [SerializeField] private HuntLevelData data;
        public HuntLevelData Data => data;
        private Queue<Hunter> hunters;
        private Target activeTarget;
        private Hunter activeHunter;
        private LineRenderer lineRend;

        private Vector3 startingPosition;
        private Vector3 startingVelocity;

        private float jumpForce;
        private float jumpOffsetX;
        public int CurrFood { get; set; }


        void Start()
        {
            QualitySettings.vSyncCount = 1;
            Application.targetFrameRate = 100;

            pathGenerator.Initialize();
            uiCont.Initialize(CurrFood);
            Initialize();
        }

        private void Initialize()
        {
            SpawnTarget();

            hunters = new Queue<Hunter>();

            GeneralData general = SaveManager.Load();

            if (general != null)
            {
                CurrFood = general.FoodValue;

                foreach (var data in general.ObjectsData)
                {
                    SpawnHunter(data.EvoLevel);
                }
            }

            else
            {
                CurrFood = data.currFood;
            }

            EventManager.Subscribe("hunter-jump", HunterJump);
            EventManager.Invoke("update-hunt-level-ui", CurrFood);

            lineRend = GetComponent<LineRenderer>();

            path.bezierPath.GlobalNormalsAngle = 90;

            SetNewActiveHunter();
        }

        private void SpawnTarget()
        {
            activeTarget = Instantiate(data.TargetsPrefs[Random.Range(0, data.TargetsPrefs.Length)], Vector3.zero, Quaternion.identity);
            activeTarget.Initialize(path);

            cameraCont.LookAt = activeTarget.transform;
        }

        private void SpawnHunter(int evoLevel)
        {
            Hunter hunter = Instantiate(data.HunterPref, Vector3.zero, Quaternion.identity);
            hunter.Initialize(evoLevel, path, this);
            hunters.Enqueue(hunter);
        }

        public void SetNewActiveHunter()
        {
            if (hunters.Count > 0)
            {
                activeHunter = hunters.Dequeue();

                cameraCont.Follow = activeHunter.transform;
            }

            else
                SceneManager.LoadScene(0); // Restart
        }

        private void HunterJump(params object[] args)
        {
            if (!activeHunter.IsJumped)
            {
                activeHunter.IsJumped = true;
                lineRend.enabled = false;

                activeHunter.Jump(startingVelocity);

                jumpForce = 0f;
                jumpOffsetX = 0f;
            }
        }

        public void CalculateJumpTrajectory()
        {
            lineRend.enabled = true;

            List<Vector3> points = new List<Vector3>();
            startingPosition = activeHunter.transform.position;
            startingVelocity = ((Vector3.up + activeHunter.transform.forward) * jumpForce) + activeHunter.transform.right * jumpOffsetX;

            for (float t = 0; t < data.NumPoints; t += data.TimeBetweenPoints)
            {
                Vector3 newPoint = startingPosition + t * startingVelocity;
                newPoint.y = startingPosition.y + startingVelocity.y * t + Physics.gravity.y / 2f * t * t;
                points.Add(newPoint);

                if (Physics.OverlapSphere(newPoint, 2f, data.TrajectoryMask).Length > 0)
                {
                    lineRend.positionCount = points.Count;
                    break;
                }
            }

            lineRend.positionCount = data.NumPoints;
            lineRend.SetPositions(points.ToArray());
        }

        void Update()
        {
            if (!activeHunter.IsJumped)
            {
                if (inputCont.IsSwipingVert())
                {
                    jumpForce += inputCont.InputY;
                    jumpForce = Mathf.Clamp(jumpForce, 0f, activeHunter.Data.MaxJumpForce);
                }

                if (inputCont.IsSwipingHor())
                {
                    jumpOffsetX += inputCont.InputX;
                    jumpOffsetX = Mathf.Clamp(jumpOffsetX, -2f, 2f);
                }

                CalculateJumpTrajectory();
            }
        }
    }
}
