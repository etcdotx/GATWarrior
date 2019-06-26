using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace GrassBending {
    public static class GrassBendingManager {
        private class ProxyBehaviour : MonoBehaviour {
            public System.Action OnUpdate { get; set; }
            private void Update() => OnUpdate?.Invoke();
        }

        private const int m_sourcesLimit = 16;
        private static readonly HashSet<IGrassBend> m_benders = new HashSet<IGrassBend>();
        private static readonly Vector4[] m_bendData = new Vector4[m_sourcesLimit];
        private static readonly int m_bendDataPropertyId = Shader.PropertyToID("_BendData");

        public static void AddBender(IGrassBend bender) {
            if (!m_benders.Add(bender)) return;

            //Debug.Log("x");
            var t_sortedBender = m_benders.OrderBy(b => b.m_priority).ToList();
            m_benders.Clear();  
            m_benders.UnionWith(t_sortedBender);
            //Debug.Log(m_benders.Add(bender));
        }

        public static void RemoveBender(IGrassBend bender) => m_benders.Remove(bender);

        private static void ProcessBender() {
            var t_sourceIndex = 0;
            foreach (var bender in m_benders) {
                if (t_sourceIndex >= m_sourcesLimit)  break; 
                m_bendData[t_sourceIndex] = new Vector4(bender.m_Position.x, bender.m_Position.y, bender.m_Position.z, bender.m_BendRadius);
                t_sourceIndex++;
            }

            for (int i = t_sourceIndex; i < m_bendData.Length; i++)
            {
                m_bendData[i] = Vector4.zero;
            }
            Shader.SetGlobalVectorArray(m_bendDataPropertyId, m_bendData);
            
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]

        private static void Initialize() {
            var t_objectName = nameof(GrassBendingManager);
            var t_gameObject = new GameObject(t_objectName)
            {
                hideFlags = HideFlags.HideAndDontSave
            };
            Object.DontDestroyOnLoad(t_gameObject);

            var t_proxyBehaviour = t_gameObject.AddComponent<ProxyBehaviour>();
            t_proxyBehaviour.OnUpdate = ProcessBender;
        }

    }
}