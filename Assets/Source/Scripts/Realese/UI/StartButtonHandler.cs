using IJunior.TypedScenes;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Source.Scripts.Realese.UI
{
    public class StartButtonHandler : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            TrainingScene.Load();
        }
    }
}