using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class PInput : MonoBehaviour {

    public int playernum;
    public bool controller;
    public KeyCode up;
    public KeyCode down;
    public KeyCode right;
    public KeyCode left;
    public KeyCode jump;
    public KeyCode sword;
    public KeyCode special;
    public KeyCode taunt;
    public KeyCode gun;
    public PController pc;
  


    // Use this for initialization
    void Start () {
        pc = gameObject.GetComponent<PController>();
        if (GameController.playerids[playernum].controllernum == 0)
            controller = false;
        else
        {
            controller = true;
            Xbox360 xbox = GetComponent<Xbox360>();
            xbox.controllernum = GameController.playerids[playernum].controllernum;
        }
    }
	
	// Update is called once per frame
	public void InputUpdate () {
        if (controller == false)
            Keyboard();
        if (controller == true)
            Controller();

    }

    void Controller()
    {
        Xbox360 xbox = gameObject.GetComponent<Xbox360>();
		if (xbox.state.IsConnected == false)
			return;
        GamePadThumbSticks.StickValue stickl = xbox.GetStick_L();
        ControllerDigital("Jump", "A");
        ControllerDigital("Sword", "X");
        ControllerDigital("Gun", "B");
        
        ControllerDigital("Special", "Y");
        In("Up", stickl.Y>0.5f || xbox.GetButton("DPad_Up"));
        In("Down", stickl.Y < -0.5f || xbox.GetButton("DPad_Down"));
        In("Right", stickl.X > 0.2f || xbox.GetButton("DPad_Right"));
        In("Left", stickl.X < -0.2f || xbox.GetButton("DPad_Left"));
    }

    void In(string input, bool i)
    {
        pc.inputs[input].down = i;
    }

    void Trigger(string input, bool r)
    {
        Xbox360 xbox = gameObject.GetComponent<Xbox360>();
        pc.inputs[input].down = xbox.GetTrigger_R() > 0.4;
        pc.inputs[input].isPressed = xbox.GetTriggerTap_R();
        pc.inputs[input].isReleased = xbox.GetTrigger_R() > 0.4;
    }


    void ControllerDigital(string input, string button)
    {
        Xbox360 xbox = gameObject.GetComponent<Xbox360>();
        pc.inputs[input].down = xbox.GetButton(button);
        pc.inputs[input].isPressed = xbox.GetButtonDown(button);
        pc.inputs[input].isReleased = xbox.GetButtonReleased (button);
    }

    void InKey(string inputmap,KeyCode key)
    {
        pc.inputs[inputmap].down = Input.GetKey(key);
        pc.inputs[inputmap].isPressed = Input.GetKeyDown(key);
        pc.inputs[inputmap].isReleased   = Input.GetKeyUp(key);
    }

    void Update()
    {
        if (controller == false)
            Keyboard();
        if (controller == true)
            Controller();
    }

    void Keyboard()
    {
        InKey("Up", up);
        InKey("Down", down);
        InKey("Left", left);
        InKey("Right", right);
        InKey("Jump", jump);
        InKey("Sword", sword);
        InKey("Special", special);
        InKey("Taunt", taunt);
        InKey("Gun", gun);
    }
}
