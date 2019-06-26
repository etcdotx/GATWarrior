using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrassBending {
    [RequireComponent(typeof(Renderer)), DisallowMultipleComponent]
    public class BendWhenVisible : MonoBehaviourGrassBend {
        private void OnBecameVisible()
        {
            GrassBendingManager.AddBender(this);
        }

        private void OnBecameInvisible()
        {
            GrassBendingManager.RemoveBender(this);
        }
    }
}
