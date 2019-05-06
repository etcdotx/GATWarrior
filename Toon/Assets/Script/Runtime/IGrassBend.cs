using UnityEngine;

namespace GrassBending
{
    public interface IGrassBend {
        //Bender World Position
        Vector3 m_Position { get; }
        //Radius Of the bend affection
        float m_BendRadius { get; }
        //Priority of the bender
        int m_priority { get; }
    }
}

