using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts.Scriptables;
using Assets.Scripts.General;

namespace Assets.Scripts.Merge
{
    public class MergeController : MonoBehaviour
    {
        [SerializeField] private List<MergeObject> objects;
        [SerializeField] private Cell[] gridCells;
        [SerializeField] private UIController uiCont;
        [SerializeField] private MergeLevelData data;
        public int CurrFood { get; set; }


        void Start()
        {
            QualitySettings.vSyncCount = 1;
            Application.targetFrameRate = 100;

            Initialize();
            uiCont.Initialize(CurrFood, data.CreaturePrice);
        }

        private void Initialize()
        {
            // Load General Data

            GeneralData general = SaveManager.Load();

            if (general != null)
            {
                CurrFood = general.FoodValue;

                foreach (var data in general.ObjectsData)
                {
                    Cell relCell = gridCells[data.CellIndex];
                    SpawnObject(relCell, data.EvoLevel);
                }

                for (int i = 0; i < gridCells.Length; i++)
                {
                    var cellData = general.CellsData[i];

                    gridCells[i].Initialize(data, this, cellData.IsLocked);
                }
            }

            else
            {
                CurrFood = data.currFood;

                foreach (var cell in gridCells)
                    cell.Initialize(data, this, true);
            }

            EventManager.Subscribe("update-objects-list", UpdateObjectsList);
        }

#if UNITY_EDITOR
        void OnApplicationQuit()
        {
            SaveManager.Save(CurrFood, ref objects, ref gridCells);
        }

#elif UNITY_ANDROID
        void OnApplicationFocus(bool focus)
        {
            if (!focus)
                SaveManager.Save(CurrFood, ref objects, ref gridCells);
        }
#endif
        public void StartHuntLevel()
        {
            SaveManager.Save(CurrFood, ref objects, ref gridCells);
            SceneManager.LoadScene(1);
        }

        private void UpdateObjectsList(params object[] args)
        {
            MergeObject anotherObject = (MergeObject)args[0];

            objects.Remove(anotherObject);
        }

        private void SpawnObject(Cell avialableCell, int evoLevel)
        {
            MergeObject obj = Instantiate(data.ObjectPref, Vector3.zero, Quaternion.identity);
            obj.Initialize(data, avialableCell, evoLevel);
            avialableCell.Take(obj);

            objects.Add(obj);
        }

        public void BuyObject()
        {
            if (CurrFood >= data.CreaturePrice)
            {
                var avialableCell = GetFreeCell();

                if (avialableCell)
                {
                    CurrFood -= data.CreaturePrice;
                    EventManager.Invoke("update-merge-level-ui", CurrFood);

                    SpawnObject(avialableCell, 0);
                }

                else
                    Debug.Log("Not Enough Cells!");
            }

            else
                Debug.Log("Not Enough Food!");
        }

        private Cell GetFreeCell()
        {
            foreach (var cell in gridCells)
            {
                if (!cell.IsLocked && cell.RelObject == null)
                    return cell;
            }

            return null;
        }
    }
}