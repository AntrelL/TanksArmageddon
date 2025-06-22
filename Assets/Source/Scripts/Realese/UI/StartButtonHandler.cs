using UnityEngine;
using UnityEngine.EventSystems;
using IJunior.TypedScenes;

namespace TanksArmageddon
{
    public class StartButtonHandler : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            TrainingScene.Load();
        }
    }
}