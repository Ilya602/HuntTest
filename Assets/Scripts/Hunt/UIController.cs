using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Assets.Scripts.General;

namespace Assets.Scripts.Hunt
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI foodCountText;


        public void Initialize(int currFoodCount)
        {
            foodCountText.text = currFoodCount.ToString();

            EventManager.Subscribe("update-hunt-level-ui", UpdateUI);
        }

        public void UpdateUI(params object[] args)
        {
            int currFood = (int)args[0];

            foodCountText.text = currFood.ToString();
        }
    }
}
