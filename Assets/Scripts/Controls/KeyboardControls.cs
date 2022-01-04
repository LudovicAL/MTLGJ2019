using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KeyboardControls", menuName = "Controls/KeyboardControls")]
public class KeyboardControls : Controls {

    //Controls type
    private const controlsType CONTROLSTYPE = controlsType.KEYBOARD;

    [Header("D-Pad and Joysticks equivalents")]
    public KeyCode up = KeyCode.UpArrow;
    public KeyCode down = KeyCode.DownArrow;
    public KeyCode left = KeyCode.LeftArrow;
    public KeyCode right = KeyCode.RightArrow;
    public KeyCode click = KeyCode.L;

    [Header("Buttons equivalents")]
    public KeyCode buttonA = KeyCode.U;
    public KeyCode buttonB = KeyCode.I;
    public KeyCode buttonX = KeyCode.O;
    public KeyCode buttonY = KeyCode.P;

    [Header("Bumpers equivalents")]
    public KeyCode lBumper = KeyCode.N;
    public KeyCode rBumper = KeyCode.M;

    [Header("Other equivalents")]
    public KeyCode buttonStart = KeyCode.J;
    public KeyCode buttonBack = KeyCode.K;
    
    //Returns the combined value of two keyboard buttons as if they were a joystick axis
    private float getKeysAsAxis(KeyCode positive, KeyCode negative) {
        float axis = (Input.GetKey(positive)) ? 1.0f : 0.0f;
        axis += (Input.GetKey(negative)) ? -1.0f : 0.0f;
        return axis;
    }
    
    //Getters
    public override controlsType GetControlsType() {
        return CONTROLSTYPE;
    }
    public override float GetDHorizontal() {
        return getKeysAsAxis(right, left);
    }
    public override float GetDVertical() {
        return getKeysAsAxis(up, down);
    }
    public override float GetLHorizontal() {
        return getKeysAsAxis(right, left);
    }
    public override float GetLVertical() {
        return getKeysAsAxis(up, down);
    }
    public override bool GetLClickDown() {
        return Input.GetKeyDown(click);
    }
	public override bool GetLClick() {
		return Input.GetKey(click);
	}
	public override bool GetLClickUp() {
		return Input.GetKeyUp(click);
	}
	public override float GetRHorizontal() {
        return getKeysAsAxis(right, left);
    }
    public override float GetRVertical() {
        return getKeysAsAxis(up, down);
    }
    public override bool GetRClickDown() {
        return Input.GetKeyDown(click);
    }
	public override bool GetRClick() {
		return Input.GetKey(click);
	}
	public override bool GetRClickUp() {
		return Input.GetKeyUp(click);
	}
	public override bool GetButtonADown() {
        return Input.GetKeyDown(buttonA);
    }
	public override bool GetButtonA() {
		return Input.GetKey(buttonA);
	}
	public override bool GetButtonAUp() {
		return Input.GetKeyUp(buttonA);
	}
	public override bool GetButtonBDown() {
        return Input.GetKeyDown(buttonB);
    }
	public override bool GetButtonB() {
		return Input.GetKey(buttonB);
	}
	public override bool GetButtonBUp() {
		return Input.GetKeyUp(buttonB);
	}
	public override bool GetButtonXDown() {
        return Input.GetKeyDown(buttonX);
    }
	public override bool GetButtonX() {
		return Input.GetKey(buttonX);
	}
	public override bool GetButtonXUp() {
		return Input.GetKeyUp(buttonX);
	}
	public override bool GetButtonYDown() {
        return Input.GetKeyDown(buttonY);
    }
	public override bool GetButtonY() {
		return Input.GetKey(buttonY);
	}
	public override bool GetButtonYUp() {
		return Input.GetKeyUp(buttonY);
	}
	public override bool GetLBumperDown() {
        return Input.GetKeyDown(lBumper);
    }
	public override bool GetLBumper() {
		return Input.GetKey(lBumper);
	}
	public override bool GetLBumperUp() {
		return Input.GetKeyUp(lBumper);
	}
	public override bool GetRBumperDown() {
        return Input.GetKeyDown(rBumper);
    }
	public override bool GetRBumper() {
		return Input.GetKey(rBumper);
	}
	public override bool GetRBumperUp() {
		return Input.GetKeyUp(rBumper);
	}
	public override bool GetButtonStartDown() {
        return Input.GetKeyDown(buttonStart);
    }
	public override bool GetButtonStart() {
		return Input.GetKey(buttonStart);
	}
	public override bool GetButtonStartUp() {
		return Input.GetKeyUp(buttonStart);
	}
	public override bool GetButtonBackDown() {
        return Input.GetKeyDown(buttonBack);
    }
	public override bool GetButtonBack() {
		return Input.GetKeyDown(buttonBack);
	}
	public override bool GetButtonBackUp() {
		return Input.GetKeyUp(buttonBack);
	}
}