using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrassBending
{
    [DisallowMultipleComponent]
    public class BendWhenEnable : MonoBehaviourGrassBend
    {
        private void OnEnable()
        {
            GrassBendingManager.AddBender(this);
        }

        private void OnDisable()
        {
            GrassBendingManager.RemoveBender(this);
        }
    }
}

