using System;
using UnityEngine;

namespace RainyPlace
{
    public struct Contract
    {
        public Contract(string text, ContractSeverity severity = ContractSeverity.Error)
        {
            Text = text;
            Severity = severity;
        }

        public string Text { get; private set; }

        public ContractSeverity Severity { get; private set; }

        public bool CheckViolation(bool violationCondition, string prefix = null, string postfix = null) => 
            CheckViolation(violationCondition, text => prefix + text + postfix);

        public bool CheckViolation(bool violationCondition, Func<string, string> textProcessor)
        {
            if (violationCondition)
                GetLogMethod()(textProcessor?.Invoke(Text) ?? Text);

            return violationCondition;
        }

        private Action<string> GetLogMethod() => Severity switch
        { 
            ContractSeverity.Info => Debug.Log,
            ContractSeverity.Warning => Debug.LogWarning,
            ContractSeverity.Error => Debug.LogError,
            _ => Debug.Log,
        };
    }
}
