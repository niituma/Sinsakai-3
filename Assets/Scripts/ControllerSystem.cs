using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerSystem : MonoBehaviour
{
	[Header("Character Input Values")]
	public Vector2 move;
	public bool change;
	public bool attack;
	public bool lockon;
	public bool jump;
	public bool fire;
	public bool sprint;

	[Header("Movement Settings")]
	public bool analogMovement;

#if !UNITY_IOS || !UNITY_ANDROID
	[Header("Mouse Cursor Settings")]
	public bool cursorLocked = true;
	public bool cursorInputForLook = true;
#endif

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
	public void OnMove(InputValue value)
	{
		MoveInput(value.Get<Vector2>());
	}

	public void OnJump(InputValue value)
	{
		JumpInput(value.isPressed);
	}
	public void OnChange(InputValue value)
	{
		ChangeInput(value.isPressed);
	}
	public void OnLockOn(InputValue value)
	{
		LockOnInput(value.isPressed);
	}
	public void OnAttack(InputValue value)
	{
		AttackInput(value.isPressed);
	}
	public void OnFire(InputValue value)
	{
		FireInput(value.isPressed);
	}

	public void OnSprint(InputValue value)
	{
		SprintInput(value.isPressed);
	}
#else
	// old input sys if we do decide to have it (most likely wont)...
#endif


	public void MoveInput(Vector2 newMoveDirection)
	{
		move = newMoveDirection;
	}

	public void JumpInput(bool newJumpState)
	{
		jump = newJumpState;
	}
	public void ChangeInput(bool newJumpState)
	{
		change = newJumpState;
	}
	public void LockOnInput(bool newJumpState)
	{
		lockon = newJumpState;
	}
	public void AttackInput(bool newJumpState)
	{
		attack = newJumpState;
	}
	public void FireInput(bool newFireState)
	{
		fire = newFireState;
	}

	public void SprintInput(bool newSprintState)
	{
		sprint = newSprintState;
	}

#if !UNITY_IOS || !UNITY_ANDROID

	private void OnApplicationFocus(bool hasFocus)
	{
		SetCursorState(cursorLocked);
	}

	private void SetCursorState(bool newState)
	{
		Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
	}

#endif


}
