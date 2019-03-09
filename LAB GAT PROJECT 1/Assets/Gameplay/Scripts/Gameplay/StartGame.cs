using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour {

    public Player player;
    public int spawnPosition;
	// Use this for initialization
	void Awake ()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        player.loadPlayer(spawnPosition);
    }
}
