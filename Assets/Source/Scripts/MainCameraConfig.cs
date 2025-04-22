using Essentia;

using SerializeField = UnityEngine.SerializeField;
using UnityCamera = UnityEngine.Camera;

namespace TanksArmageddon
{
    public class MainCameraConfig : ScriptConfig, MainCameraConfig.IAccessPoint
    {
        [SerializeField] private UnityCamera _camera;

        public Camera Camera => _camera;

        public interface IAccessPoint : IScriptConfigAccessPoint
        {
            public Camera Camera { get; }
        }
    }
}
