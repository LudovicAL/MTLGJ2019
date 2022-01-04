using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controls: ScriptableObject {

    //Player id
    public string controlsName;

    //Controls type
    public enum controlsType {
        XBOX,
        KEYBOARD,
        MOUSE
    }

    //Getters
    public abstract controlsType GetControlsType();
    public abstract float GetDHorizontal();
    public abstract float GetDVertical();
    public abstract float GetLHorizontal();
    public abstract float GetLVertical();
    public abstract bool GetLClickDown();
	public abstract bool GetLClick();
	public abstract bool GetLClickUp();
	public abstract float GetRHorizontal();
    public abstract float GetRVertical();
    public abstract bool GetRClickDown();
	public abstract bool GetRClick();
	public abstract bool GetRClickUp();
	public abstract bool GetButtonADown();
	public abstract bool GetButtonA();
	public abstract bool GetButtonAUp();
	public abstract bool GetButtonBDown();
	public abstract bool GetButtonB();
	public abstract bool GetButtonBUp();
	public abstract bool GetButtonXDown();
	public abstract bool GetButtonX();
	public abstract bool GetButtonXUp();
	public abstract bool GetButtonYDown();
	public abstract bool GetButtonY();
	public abstract bool GetButtonYUp();
	public abstract bool GetLBumperDown();
	public abstract bool GetLBumper();
	public abstract bool GetLBumperUp();
	public abstract bool GetRBumperDown();
	public abstract bool GetRBumper();
	public abstract bool GetRBumperUp();
	public abstract bool GetButtonStartDown();
	public abstract bool GetButtonStart();
	public abstract bool GetButtonStartUp();
	public abstract bool GetButtonBackDown();
	public abstract bool GetButtonBack();
	public abstract bool GetButtonBackUp();
}
