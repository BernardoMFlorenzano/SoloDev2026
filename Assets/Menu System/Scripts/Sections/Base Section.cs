using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MenuSystem
{
    public class BaseSection : MonoBehaviour
    {
        [SerializeField] protected RectTransform sectionPanel;
        [SerializeField] protected GameObject firstSelectedObject;
        protected GameObject previouslySelectedObject;

        protected virtual void Awake()
        {
            sectionPanel.gameObject.SetActive(false);
        }

        //Animação padrão de ativação das Seções
        //Pode ser personalizada no override de cada seção
        public virtual void Activate(GameObject previouslySelectedObject)
        {
            this.previouslySelectedObject = previouslySelectedObject;

            sectionPanel.localScale = Vector3.one * 0.8f;

            sectionPanel.gameObject.SetActive(true);

            sectionPanel.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    EventSystem.current.SetSelectedGameObject(firstSelectedObject);
                });
        }

        //Animação padrão de desativação das Seções
        //Pode ser personalizada no override de cada seção
        public virtual void Deactivate()
        {
            sectionPanel.DOScale(Vector3.one * 0.8f, 0.1f).SetEase(Ease.Linear)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                sectionPanel.gameObject.SetActive(false);

                if (previouslySelectedObject != null)
                    EventSystem.current.SetSelectedGameObject(previouslySelectedObject);
            });
        }

        private void OnDisable()
        {
            sectionPanel.DOKill();
        }
    }
}