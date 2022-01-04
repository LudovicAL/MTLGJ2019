using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "XBoxControls", menuName = "Controls/XBoxControls")]
public class XBoxControls : Controls {

    //Controls type
    private const controlsType CONTROLSTYPE = controlsType.XBOX;

    //D-Pad
    private string dHorizontal = "dHorizontal";
    private string dVertical = "dVertical";

    //Left joystick
    private string lHorizontal = "lHorizontal";
    private string lVertical = "lVertical";
    private string lClick = "lClick";

    //Right Joystick
    private string rHorizontal = "rHorizontal";
    private string rVertical = "rVertical";
    private string rClick = "rClick";

    //Buttons
    private string buttonA = "buttonA";
    private string buttonB = "buttonB";
    private string buttonX = "buttonX";
    private string buttonY = "buttonY";

    //Bumpers
    private string lBumper = "lBumper";
    private string rBumper = "rBumper";

    //Others
    private string buttonStart = "buttonStart";
    private string buttonBack = "buttonBack";
    
    //Getters
    public override controlsType GetControlsType() {
        return CONTROLSTYPE;
    }
    public override float GetDHorizontal() {
        return Input.GetAxis(base.controlsName + dHorizontal);
    }
    public override float GetDVertical() {
        return Input.GetAxis(base.controlsName + dVertical);
    }
    public override float GetLHorizontal() {
        return Input.GetAxis(base.controlsName + lHorizontal);
    }
    public override float GetLVertical() {
        return Input.GetAxis(base.controlsName + lVertical);
    }
    public override bool GetLClickDown() {
        return Input.GetButtonDown(base.controlsName + lClick);
    }
	public override bool GetLClick() {
		return Input.GetButton(base.controlsName + lClick);
	}
	public override bool GetLClickUp() {
		return Input.GetButtonUp(base.controlsName + lClick);
	}
	public override float GetRHorizontal() {
        return Input.GetAxis(base.controlsName + rHorizontal);
    }
    public override float GetRVertical() {
        return Input.GetAxis(base.controlsName + rVertical);
    }
    public override bool GetRClickDown() {
        return Input.GetButtonDown(base.controlsName + rClick);
    }
	public override bool GetRClick() {
		return Input.GetButton(base.controlsName + rClick);
	}
	public override bool GetRClickUp() {
		return Input.GetButtonUp(base.controlsName + rClick);
	}
	public override bool GetButtonADown() {
        return Input.GetButtonDown(base.controlsName + buttonA);
    }
	public override bool GetButtonA() {
		return Input.GetButton(base.controlsName + buttonA);
	}
	public override bool GetButtonAUp() {
		return Input.GetButtonUp(base.controlsName + buttonA);
	}
	public override bool GetButtonBDown() {
        return Input.GetButtonDown(base.controlsName + buttonB);
    }
	public override bool GetButtonB() {
		return Input.GetButton(base.controlsName + buttonB);
	}
	public override bool GetButtonBUp() {
		return Input.GetButtonUp(base.controlsName + buttonB);
	}
	public override bool GetButtonXDown() {
        return Input.GetButtonDown(base.controlsName + buttonX);
    }
	public override bool GetButtonX() {
		return Input.GetButton(base.controlsName + buttonX);
	}
	public override bool GetButtonXUp() {
		return Input.GetButtonUp(base.controlsName + buttonX);
	}
	public override bool GetButtonYDown() {
        return Input.GetButtonDown(base.controlsName + buttonY);
    }
	public override bool GetButtonY() {
		return Input.GetButton(base.controlsName + buttonY);
	}
	public override bool GetButtonYUp() {
		return Input.GetButtonUp(base.controlsName + buttonY);
	}
	public override bool GetLBumperDown() {
        return Input.GetButtonDown(base.controlsName + lBumper);
    }
	public override bool GetLBumper() {
		return Input.GetButton(base.controlsName + lBumper);
	}
	public override bool GetLBumperUp() {
		return Input.GetButtonUp(base.controlsName + lBumper);
	}
	public override bool GetRBumperDown() {
        return Input.GetButtonDown(base.controlsName + rBumper);
    }
	public override bool GetRBumper() {
		return Input.GetButton(base.controlsName + rBumper);
	}
	public override bool GetRBumperUp() {
		return Input.GetButtonUp(base.controlsName + rBumper);
	}
	public override bool GetButtonStartDown() {
        return Input.GetButtonDown(base.controlsName + buttonStart);
    }
	public override bool GetButtonStart() {
		return Input.GetButton(base.controlsName + buttonStart);
	}
	public override bool GetButtonStartUp() {
		return Input.GetButtonUp(base.controlsName + buttonStart);
	}
	public override bool GetButtonBackDown() {
        return Input.GetButtonDown(base.controlsName + buttonBack);
    }
	public override bool GetButtonBack() {
		return Input.GetButton(base.controlsName + buttonBack);
	}
	public override bool GetButtonBackUp() {
		return Input.GetButtonUp(base.controlsName + buttonBack);
	}
}