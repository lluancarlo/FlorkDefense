using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputHandler : MonoBehaviour
{
	public static InputHandler Instance;
	PlayerInput _playerInput;
	InputAction _clickAction, _clickPositionAction, _touchPositionAction;
	[SerializeField] GameManager _gameManager;

	void Awake()
	{
		if (Instance != null)
			return;
		Instance = this;
		_playerInput = GetComponent<PlayerInput>();
		_clickAction = _playerInput.actions[Click];
		_clickPositionAction = _playerInput.actions[ClickPosition];
		_touchPositionAction = _playerInput.actions[TouchPosition];
	}

	const string Click = nameof(Click);
	const string ClickPosition = nameof(ClickPosition);
	const string TouchPosition = nameof(TouchPosition);

	void OnEnable()
	{
		_clickAction.performed += clickPressed;
		_touchPositionAction.performed += touchPressed;
	}

	void OnDisable()
	{
		_clickAction.performed -= clickPressed;
		_touchPositionAction.performed -= touchPressed;
	}

	void clickPressed(InputAction.CallbackContext context)
	{
		if (!context.action.IsPressed())
			return;
		TrySelectGameObject(_clickPositionAction.ReadValue<Vector2>());
	}

	void touchPressed(InputAction.CallbackContext context)
	{
		if (!context.action.IsPressed())
			return;
		TrySelectGameObject(context.ReadValue<Vector2>());
	}

	void TrySelectGameObject(Vector2 position)
	{
		if (!PointerIsUIHit(position))
		{
			if (_gameManager.HasTowerSelected())
				_gameManager.OnDeselect();
		}
		
		Ray ray = Camera.main.ScreenPointToRay(position);
		RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, 32);
		if (hit.collider != null)
		{
			if (hit.collider.gameObject.TryGetComponent(out TowerSlot tower))
				_gameManager.OnSelect(tower);
		}
	}

	bool PointerIsUIHit(Vector2 position)
    {
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = position;
        List<RaycastResult> raycastResults = new List<RaycastResult>();

        // UI Elements must have `picking mode` set to `position` to be hit
        EventSystem.current.RaycastAll(pointer, raycastResults);

        foreach (RaycastResult result in raycastResults)
            if (result.distance == 0 && result.isValid)
                return true;
				return false;
    }
}
