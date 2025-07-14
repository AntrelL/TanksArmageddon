using System.Collections;
using Source.Scripts.Release.Airdrop;
using UnityEngine;

namespace Source.Scripts.Release.UI.ControllerParts
{
    public class AirdropFeedbackUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _airdropNotifierCanvasGroup;
        [SerializeField] private float _fadeDuration = 1.0f;
        [SerializeField] private float _visibleDuration = 1.0f;
        [SerializeField] private AirdropSpawner _airdropSpawner;

        private void OnEnable()
        {
            _airdropSpawner.Spawned += OnSpawned;
        }

        private void OnDisable()
        {
            _airdropSpawner.Spawned -= OnSpawned;
        }

        private IEnumerator FadeRoutine()
        {
            yield return Fade(0f, 1f, _fadeDuration);

            yield return new WaitForSeconds(_visibleDuration);

            yield return Fade(1f, 0f, _fadeDuration);
        }

        private IEnumerator Fade(float startValue, float targetValue, float duration)
        {
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float alpha = Mathf.Lerp(startValue, targetValue, elapsedTime / duration);
                SetAlpha(alpha);
                elapsedTime += Time.deltaTime;

                yield return null;
            }

            SetAlpha(targetValue);
        }

        private void SetAlpha(float alpha)
        {
            _airdropNotifierCanvasGroup.alpha = alpha;
        }

        private void OnSpawned(AirdropBox airdrop)
        {
            StartCoroutine(FadeRoutine());
        }
    }
}
