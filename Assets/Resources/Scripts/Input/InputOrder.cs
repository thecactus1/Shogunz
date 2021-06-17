using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputOrder : MonoBehaviour {

    public int order;
    public string gamemode;
    public List <InputGet> inputorder;

	// Use this for initialization
	void Start () {
        Debug.Log(1);
        //GameController.Players();
        Debug.Log(2);
        order = 0;
        inputorder = new List<InputGet>();
        inputorder.Add(transform.Find("Slot1").GetComponent<InputGet>());
        if(transform.Find("Slot2").GetComponent<InputGet>())
        inputorder.Add(transform.Find("Slot2").GetComponent<InputGet>());
        if(transform.Find("Slot3").GetComponent<InputGet>())
        inputorder.Add(transform.Find("Slot3").GetComponent<InputGet>());
        if (transform.Find("Slot4").GetComponent<InputGet>())
            inputorder.Add(transform.Find("Slot4").GetComponent<InputGet>());
        Debug.Log(3);

    }
	
	// Update is called once per frame
	void Update () {
        GameController.Players();
        Debug.Log(4);
        if (GameController.playerids[4].controllernum == -1)
            order = 3;
        if (GameController.playerids[3].controllernum == -1)
            order = 2;
        if (GameController.playerids[2].controllernum == -1)
            order = 1;
        if (GameController.playerids[1].controllernum == -1)
            order = 0;
        Debug.Log(5);
        if (order<GameController.GameMode[gamemode].playermax)
        inputorder[order].ControllerCheck();
	}
}
