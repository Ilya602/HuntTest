using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.General;

namespace Assets.Scripts.Hunt
{
    public class TargetUI : MonoBehaviour
    {
        [SerializeField] private Target targetComp;
        [SerializeField] private Image healthBar;


        public void Initialize()
        {
            EventManager.Subscribe("update-target-ui", UpdateUI);
        }

        private void UpdateUI(params object[] args)
        {
            float barValue = Mathf.InverseLerp(0, targetComp.Data.Health, targetComp.Health);

            healthBar.fillAmount = barValue;
        }
    }
}