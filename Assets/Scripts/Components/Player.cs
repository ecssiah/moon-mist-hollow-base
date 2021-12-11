using UnityEngine;
using UnityEngine.InputSystem;

namespace MMH
{
	public class Player : MonoBehaviour
	{
		private Camera _camera;

		private float _panSpeed;
		private float _zoomSpeed;

		private PlayerInputActions _playerInputActions;

		private InputAction _pan;
		private InputAction _zoom;
		private InputAction _primary;
		private InputAction _secondary;

		void Awake()
		{
			_camera = GameObject.Find("Player").GetComponentInChildren<Camera>();
			_camera.transform.position = new Vector3(0, 0, -10);
			_camera.orthographicSize = 6f;

			_panSpeed = 8.0f;
			_zoomSpeed = 8.0f;
			
			_playerInputActions = new PlayerInputActions();

			_pan = _playerInputActions.Player.Pan;
			_zoom = _playerInputActions.Player.Zoom;
			_primary = _playerInputActions.Player.Primary;
			_secondary = _playerInputActions.Player.Secondary;
			
			_primary.performed += PrimaryAction;
			_secondary.performed += SecondaryAction;
		}

		void OnEnable()
		{
			_pan.Enable();
			_zoom.Enable();
			_primary.Enable();
			_secondary.Enable();
		}

		void OnDisable()
		{
			_pan.Disable();
			_zoom.Disable();
			_primary.Disable();
			_secondary.Disable();
		}

		void Update()
		{
			UpdatePan();
			UpdateZoom();
		}

		private void UpdatePan()
		{
			Vector2 panValue = _pan.ReadValue<Vector2>();
			Vector3 panDisplacement = _panSpeed * panValue;

			_camera.transform.position = Vector3.Lerp(
				_camera.transform.position,
				_camera.transform.position + panDisplacement,
				Time.deltaTime
			);
		}

		private void UpdateZoom()
		{
			float zoomValue = _zoom.ReadValue<float>();
			float zoomDisplacement = _zoomSpeed * zoomValue;

			_camera.orthographicSize = Mathf.Lerp(
				_camera.orthographicSize,
				_camera.orthographicSize + zoomDisplacement,
				Time.deltaTime
			);

			_camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize, 2f, 20f);
		}

		private void PrimaryAction(InputAction.CallbackContext callbackContext)
		{
			Debug.Log($"Primary Action: {callbackContext}");
		}

		private void SecondaryAction(InputAction.CallbackContext callbackContext)
		{
			Debug.Log($"Secondary Action: {callbackContext}");
		}
	}
}
