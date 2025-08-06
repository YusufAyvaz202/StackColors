using UnityEngine;
using UnityEngine.UI;

namespace UI
{
   public class BonusUI : MonoBehaviour
   {
      [SerializeField] private Slider _bonusSlider;
   
      public void UpdateBonusSlider(float bonusValue)
      {
         _bonusSlider.value = bonusValue / 5f;
      }
   }
}
