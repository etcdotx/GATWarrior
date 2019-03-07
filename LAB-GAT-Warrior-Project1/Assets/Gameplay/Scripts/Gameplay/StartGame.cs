using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour {

    public Player Player;
    public GameObject Character;
	// Use this for initialization
	void Awake ()
    {
        Player = GameObject.Find("Player").GetComponent<Player>();
        Player.LoadPlayer();
    }
}
