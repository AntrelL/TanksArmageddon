using Essentia;

using SerializeField = UnityEngine.SerializeField;
using UnityCanvas = UnityEngine.Canvas;

namespace TanksArmageddon
{
    public class ScreenConfig : ScriptConfig, ScreenConfig.IAccessPoint
    {
        [SerializeField] private UnityCanvas _canvas;

        public Canvas Canvas => _canvas;

        public interface IAccessPoint : IScriptConfigAccessPoint
        {
            public Canvas Canvas { get; }
        }
    }
}
