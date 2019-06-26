using System;
using UnityEngine;

namespace GrassBending
{
    public abstract class MonoBehaviourGrassBend : MonoBehaviour, IGrassBend, IEquatable<MonoBehaviourGrassBend> {
        public Vector3 m_Position => transform.position;
        public float m_BendRadius  { get => t_bendRadius; set => t_bendRadius = value; }
        public int m_priority { get => t_priority; set => t_priority = value; }

        [Tooltip("Radius Bend Affection"), Range(0.1f, 10f)]
        [SerializeField] private float t_bendRadius = 1f;
        [Tooltip("Priorit Bender")]
        [SerializeField] private int t_priority = 0;

        public bool Equals(MonoBehaviourGrassBend other) {
            if (other is null) return false;
            return other.GetInstanceID() == GetInstanceID();
        }
    }
}
