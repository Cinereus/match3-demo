using System;
using UnityEngine;

namespace Code.Runtime.Logic.Match3
{
    public class Match3Slot : MonoBehaviour
    {
        public Guid slotId { get; } = Guid.NewGuid();
        public Vector3 targetPoint => gameObject.transform.position;
    }
}