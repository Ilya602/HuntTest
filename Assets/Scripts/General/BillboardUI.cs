using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.General
{
    public class BillboardUI : MonoBehaviour
    {
        void LateUpdate()
        {
            transform.LookAt(Camera.main.transform);
            transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
        }
    }
}
