using UnityEngine;
using UnityEngine.InputSystem;

namespace MMH
{
	public class PlayerSystem : GameSystem<PlayerSystem>
	{
		private float _panSpeed;
		private float _zoomSpeed;

		private Camera _playerCamera;

		private PlayerInputActions _playerInputActions;

		private InputAction _pan;
		private InputAction _zoom;
		private InputAction _select;

		protected override void Awake()
		{
			base.Awake();

			_playerInputActions = new PlayerInputActions();
		}

		private void Start()
		{
			_panSpeed = ManagerSystem.Settings.PanSpeed;
			_zoomSpeed = ManagerSystem.Settings.ZoomSpeed;
		}

		void OnEnable()
		{
			_playerCamera = GetComponentInChildren<Camera>();

			_pan = _playerInputActions.Player.Pan;
			_pan.Enable();

			_zoom = _playerInputActions.Player.Zoom;
			_zoom.Enable();

			_select = _playerInputActions.Player.Select;
			_select.performed += Select;
			_select.Enable();
		}

		void OnDisable()
		{
			_pan.Disable();
			_zoom.Disable();
			_select.Disable();
		}

		void Update()
		{
			Vector2 panValue = _pan.ReadValue<Vector2>();
			Vector3 panDisplacement = _panSpeed * panValue;

			transform.position = Vector3.Lerp(
				transform.position,
				transform.position + panDisplacement,
				Time.deltaTime
			);

			float zoomValue = _zoom.ReadValue<float>();
			float zoomDisplacement = _zoomSpeed * zoomValue;

			_playerCamera.orthographicSize = Mathf.Lerp(
				_playerCamera.orthographicSize,
				_playerCamera.orthographicSize + zoomDisplacement,
				Time.deltaTime
			);

			_playerCamera.orthographicSize = Mathf.Clamp(_playerCamera.orthographicSize, 2f, 20f);
		}

		private void Select(InputAction.CallbackContext obj)
		{

		}
	}
}
