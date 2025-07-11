using System.Threading;
using UnityEngine;

namespace Source.Scripts.Release.Utils
{
    public class WaitDestroy : CustomYieldInstruction
    {
        private readonly CancellationToken _token;

        public WaitDestroy(MonoBehaviour mono)
        {
            if (mono == null)
            {
                _token = new CancellationToken(true);
                return;
            }

            _token = mono.destroyCancellationToken;
        }

        public override bool keepWaiting => _token.IsCancellationRequested == false;
    }
}