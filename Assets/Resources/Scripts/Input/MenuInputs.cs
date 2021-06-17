using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class MenuInputs : MonoBehaviour
{

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
    public Dictionary<string, inputlist> inputs;

    public inputlist j, l, r, u, d, sw, g, sp, t;

    inputlist Createinput(inputlist i)
    {
        i = new inputlist();
        i.down = false;
        i.isPressed = false;
        i.isReleased = false;
        return i;
    }

    void SetUpInputs()
    {
        inputs = new Dictionary<string, inputlist>();
        inputs["Up"] = Createinput(u);
        inputs["Down"] = Createinput(d);
        inputs["Left"] = Createinput(l);
        inputs["Right"] = Createinput(r);
        inputs["Jump"] = Createinput(j);
        inputs["Sword"] = Createinput(sw);
        inputs["Special"] = Createinput(sp);
        inputs["Taunt"] = Createinput(t);
        inputs["Gun"] = Createinput(g);
    }

    // Use this for initialization
    void Start()
    {
        up = KeyCode.UpArrow;
        down = KeyCode.DownArrow;
        left = KeyCode.LeftArrow;
        right = KeyCode.RightArrow;
        jump = KeyCode.Z;
        sword = KeyCode.X;
        gun = KeyCode.C;
        taunt = KeyCode.LeftShift;
        special = KeyCode.Space;
        SetUpInputs();

    }

    // Update is called once per frame
    public void InputUpdate()
    {
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
        Trigger("Gun", true);
        ControllerDigital("Special", "Y");
        In("Up", stickl.Y > 0.4f || xbox.GetButton("DPad_Up"));
        In("Down", stickl.Y < -0.4f || xbox.GetButton("DPad_Down"));
        In("Right", stickl.X > 0.2f || xbox.GetButton("DPad_Right"));
        In("Left", stickl.X < -0.2f || xbox.GetButton("DPad_Left"));
    }

    void In(string input, bool i)
    {
        inputs[input].down = i;
    }

    void Trigger(string input, bool r)
    {
        Xbox360 xbox = gameObject.GetComponent<Xbox360>();
        inputs[input].down = xbox.GetTrigger_R() > 0.4;
        inputs[input].isPressed = xbox.GetTriggerTap_R();
        inputs[input].isReleased = xbox.GetTrigger_R() > 0.4;
    }


    void ControllerDigital(string input, string button)
    {
        Xbox360 xbox = gameObject.GetComponent<Xbox360>();
        inputs[input].down = xbox.GetButton(button);
        inputs[input].isPressed = xbox.GetButtonDown(button);
        inputs[input].isReleased = xbox.GetButtonReleased(button);
    }

    void InKey(string inputmap, KeyCode key)
    {
        inputs[inputmap].down = Input.GetKey(key);
        inputs[inputmap].isPressed = Input.GetKeyDown(key);
        inputs[inputmap].isReleased = Input.GetKeyUp(key);
    }

    void Update()
    {
        if (GameController.playerids[playernum].controllernum == 0)
            controller = false;
        else
        {
            controller = true;
            Xbox360 xbox = GetComponent<Xbox360>();
            xbox.controllernum = GameController.playerids[playernum].controllernum;
        }
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
