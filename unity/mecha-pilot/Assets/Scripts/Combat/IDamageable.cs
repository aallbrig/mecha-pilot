using System;
using UnityEngine;

namespace Combat
{
    [Serializable]
    public class DamageResult : IDamageResult
    {
        public DamageResult(float damageAmount, GameObject receivingGameObject)
        {
            DamageAmount = damageAmount;
            ReceivingGameObject = receivingGameObject;
        }

        public float DamageAmount { get; set; }

        public GameObject ReceivingGameObject { get; set; }
    }

    public interface IDamageResult
    {
        public float DamageAmount { get; set; }

        public GameObject ReceivingGameObject { get; set; }
    }

    public interface IDamageable
    {
        public event Action<DamageResult> Damaged;

        public void Damage(float amount);
    }
}
