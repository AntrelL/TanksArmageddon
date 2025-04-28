using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TanksArmageddon
{
    public class Medkit : Script
    {
        [SerializeField] private int _healAmount = 100;

        private void OnTriggerEnter(Collider other)
        {
            if (!IsInitialized) return;

            var healable = other.GetComponent<IHealable>();

            if (healable != null)
            {
                healable.Heal(_healAmount);
                gameObject.SetActive(false);
            }
        }
    }
}
