using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameManager : MonoBehaviour {

	//Makes new instance of a GameManager if there is not one present
	void Awake () {
		if(!GameManager.instance)
        {
            //GameManager Overmind = new GameManager();
        }
	}
	
	
}
