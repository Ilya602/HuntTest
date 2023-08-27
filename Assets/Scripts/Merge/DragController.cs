using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Merge
{
    public class DragController : MonoBehaviour
    {
        private MergeObject mergeObjComp;
        private Vector3 mousePos;


        void Start()
        {
            mergeObjComp = GetComponent<MergeObject>();
        }

        private Vector3 GetMousePos()
        {
            return Camera.main.WorldToScreenPoint(transform.position);
        }

        private void OnMouseDown()
        {
            mousePos = Input.mousePosition - GetMousePos();
        }

        private void OnMouseDrag()
        {
            var worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePos);

            transform.position = new Vector3(worldPos.x, transform.position.y, worldPos.z);
        }

        private void OnMouseUp()
        {
            mergeObjComp.SetNewCell();
        }
    }
}