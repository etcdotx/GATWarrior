using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour {

    public Player player;
    public GameObject character;
    public Vector3 characterScale;
	// Use this for initialization
	void Awake ()
    {
        GameStatus.isTalking = false;
        GameStatus.ResumeGame();
        try
        {
            player = GameObject.Find("Player").GetComponent<Player>();
            player.LoadPlayer();
        } catch { }
    }
}
