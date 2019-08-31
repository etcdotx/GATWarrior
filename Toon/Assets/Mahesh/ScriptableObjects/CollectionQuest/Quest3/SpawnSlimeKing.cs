using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SpawnSlimeKing : MonoBehaviour
{
    public static SpawnSlimeKing instance;
    public PlayableDirector playableDirector;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    public void StartEvent() {
        TimelineActivator.instance.StartTimeline(playableDirector);
    }
}
