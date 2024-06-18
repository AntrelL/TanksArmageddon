using UnityEngine;

namespace IJunior.UI
{
    public class VersionText : BasicText
    {
        public new void Initialize()
        {
            base.Initialize();

            SetText(Application.version);
        }
    }
}