using IJunior.TypedScenes;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Source.Scripts.Release.UI
{
    public class StartButtonHandler : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            TrainingScene.Load();
        }
    }
}