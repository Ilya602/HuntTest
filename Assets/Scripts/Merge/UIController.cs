using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Assets.Scripts.General;

namespace Assets.Scripts.Merge
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI foodCountText;
        [SerializeField] private TextMeshProUGUI priceValueText;


        public void Initialize(int currFoodCount, int priceValue)
        {
            foodCountText.text = currFoodCount.ToString();
            priceValueText.text = priceValue.ToString();

            EventManager.Subscribe("update-merge-level-ui", UpdateUI);
        }

        public void UpdateUI(params object[] args)
        {
            int currFood = (int)args[0];

            foodCountText.text = currFood.ToString();
        }
    }
}
