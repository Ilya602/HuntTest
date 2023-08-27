using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Scriptables;
using Assets.Scripts.General;

namespace Assets.Scripts.Merge
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private Material unlockedMat;

        private MeshRenderer meshRend;
        private MergeLevelData data;
        private MergeController mergeCont;
        private MergeObject relObject;
        public MergeObject RelObject => relObject;

        [SerializeField] private bool isLocked = true;
        public bool IsLocked => isLocked;


        public void Initialize(MergeLevelData data, MergeController mergeCont, bool isLocked)
        {
            meshRend = GetComponent<MeshRenderer>();

            this.data = data;
            this.mergeCont = mergeCont;

            if (!isLocked)
            {
                this.isLocked = isLocked;
                meshRend.material = unlockedMat;
            }
        }

        private void OnMouseDown()
        {
            Unlock();
        }

        public void Unlock()
        {
            if (IsLocked)
            {
                if (mergeCont.CurrFood >= data.CellPrice)
                {
                    mergeCont.CurrFood -= data.CellPrice;
                    EventManager.Invoke("update-merge-level-ui", mergeCont.CurrFood);

                    isLocked = false;

                    meshRend.material = unlockedMat;
                }

                else
                    Debug.Log("Not Enough Food!");
            }
        }

        public void Take(MergeObject newMergeObj)
        {
            if (newMergeObj != null)
            {
                relObject = newMergeObj;
                relObject.transform.SetParent(transform);
                relObject.transform.localPosition = Vector3.zero;
            }
        }

        public void Clear()
        {
            relObject = null;
        }
    }
}