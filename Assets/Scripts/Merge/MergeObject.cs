using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Scriptables;
using Assets.Scripts.General;

namespace Assets.Scripts.Merge
{
    [RequireComponent(typeof(DragController))]
    public class MergeObject : MonoBehaviour
    {
        [SerializeField] private GameObject[] models;
        private GameObject currModel;

        private MergeLevelData data;
        private Cell lastCell;
        private Cell newCell;
        public Cell NewCell => newCell;

        [SerializeField] private int evoLevel = 0;
        public int EvoLevel => evoLevel;


        public void Initialize(MergeLevelData data, Cell lastCell, int evoLevel)
        {
            this.data = data;
            this.lastCell = lastCell;
            this.evoLevel = evoLevel;

            currModel = models[0];

            Evolution(evoLevel);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Cell>(out Cell cell))
            {
                if (!cell.IsLocked)
                {
                    if (cell.RelObject)
                    {
                        int cellCreatureLevel = cell.RelObject.EvoLevel;

                        if (cellCreatureLevel == this.evoLevel && cellCreatureLevel < data.MaxEvoLevel)
                            newCell = cell;
                    }

                    else
                        newCell = cell;
                }
            }
        }

        public void SetNewCell(params object[] args)
        {
            if (newCell.RelObject && newCell.RelObject != this)
            {
                int cellCreatureLevel = newCell.RelObject.EvoLevel;

                if (cellCreatureLevel == this.evoLevel)
                {
                    this.evoLevel++;

                    Evolution(newCell.RelObject, this.evoLevel);
                }
            }

            if (newCell != lastCell)
            {
                lastCell?.Clear();
                lastCell = newCell;
            }

            newCell.Take(this);
        }

        private void Evolution(int newEvoLevel)
        {
            currModel.SetActive(false);
            currModel = models[newEvoLevel];
            currModel.SetActive(true);
        }

        private void Evolution(MergeObject anotherObject, int newEvoLevel)
        {
            EventManager.Invoke("update-objects-list", anotherObject, this);

            Destroy(anotherObject.gameObject);
            Evolution(newEvoLevel);
        }
    }
}