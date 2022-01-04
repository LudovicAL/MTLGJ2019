using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MouseControls", menuName = "Controls/MouseControls")]
public class MouseControls : Controls {

    //Controls type
    private const controlsType CONTROLSTYPE = controlsType.MOUSE;

    [Header("Joysticks equivalent")]
    public string horizontalAxis = "Mouse X";
    public string verticalAxis = "Mouse Y";
    public float horizontalAxisSensitivity = 1.0f;
    public float verticalAxisSensitivity = 1.0f;
    public KeyCode click = KeyCode.Mouse0;

    [Header("Buttons equivalents")]
    public KeyCode buttonA = KeyCode.Mouse0;
    public KeyCode buttonB = KeyCode.Mouse1;
    public KeyCode buttonX = KeyCode.Mouse0;
    public KeyCode buttonY = KeyCode.Mouse1;

    [Header("Bumpers equivalents")]
    public KeyCode lBumper = KeyCode.Mouse0;
    public KeyCode rBumper = KeyCode.Mouse1;

    [Header("Other equivalents")]
    public KeyCode buttonStart = KeyCode.Mouse2;
    public KeyCode buttonBack = KeyCode.Mouse3;

    private float getHorizontalAxis() {
        return Mathf.Clamp(Input.GetAxis(horizontalAxis) * horizontalAxisSensitivity, -1.0f, 1.0f);
    }

    private float getVerticalAxis() {
        return Mathf.Clamp(Input.GetAxis(verticalAxis) * verticalAxisSensitivity, -1.0f, 1.0f);
    }

    //Getters
    public override controlsType GetControlsType() {
        return CONTROLSTYPE;
    }
    public override float GetDHorizontal() {
        return getHorizontalAxis();
    }
    public override float GetDVertical() {
        return getVerticalAxis();
    }
    public override float GetLHorizontal() {
        return getHorizontalAxis();
    }
    public override float GetLVertical() {
        return getVerticalAxis();
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
        return getHorizontalAxis();
    }
    public override float GetRVertical() {
        return getVerticalAxis();
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
		return Input.GetKey(buttonBack);
	}
	public override bool GetButtonBackUp() {
		return Input.GetKeyUp(buttonBack);
	}
}