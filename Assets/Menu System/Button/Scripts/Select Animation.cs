using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace MenuSystem
{
    public class SelectAnimation
    {
        public void ApplyEffect(Button button)
        {
            RectTransform buttonTransform = button.GetComponent<RectTransform>();

            buttonTransform.DOScale(Vector3.one * 2, 0.3f)
            .SetUpdate(true);
        }

        public void RemoveEffect(Button button)
        {
            RectTransform buttonTransform = button.GetComponent<RectTransform>();

            buttonTransform.DOScale(Vector3.one, 0.3f)
            .SetUpdate(true);
        }
    }
}