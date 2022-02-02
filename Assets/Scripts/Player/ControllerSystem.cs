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
	public bool avd;
	public bool aim;
	public bool shoot;
	public bool jump;
	public bool fire;
	public bool sprint;

	[Header("Movement Settings")]
	public bool analogMovement;


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
	public void OnAvd(InputValue value)
	{
		AvdInput(value.isPressed);
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
	public void OnAim(InputValue value)
	{
		AimInput(value.isPressed);
	}
	public void OnShoot(InputValue value)
	{
		ShootInput(value.isPressed);
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
	public void AvdInput(bool newJumpState)
	{
		avd = newJumpState;
	}
	public void AttackInput(bool newJumpState)
	{
		attack = newJumpState;
	}
	public void FireInput(bool newFireState)
	{
		fire = newFireState;
	}
	public void AimInput(bool newSprintState)
	{
		aim = newSprintState;
	}
	public void ShootInput(bool newSprintState)
	{
		shoot = newSprintState;
	}
	public void SprintInput(bool newSprintState)
	{
		sprint = newSprintState;
	}
}
