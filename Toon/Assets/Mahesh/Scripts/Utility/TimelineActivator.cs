using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineActivator : MonoBehaviour
{
    public GameObject timelineObject;
    public TimelineName timelineName;
    public enum TimelineName {
        startGameTimeline
    }

    void Start()
    {
        if (timelineName == TimelineName.startGameTimeline)
        {
            if (GameDataBase.instance.newGame)
                timelineObject.SetActive(true);
            else
                timelineObject.SetActive(false);
        }
    }

    void Update()
    {
        
    }
}
