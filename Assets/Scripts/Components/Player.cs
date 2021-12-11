using UnityEngine;
using UnityEngine.InputSystem;

namespace MMH
{
	public class Player : MonoBehaviour
	{
		private Camera _camera;

		private PlayerInputActions _playerInputActions;

		private float _panSpeed;
		private float _zoomSpeed;

		private InputAction _pan;
		private InputAction _zoom;
		private InputAction _select;

		void Awake()
		{
			_camera = GameObject.Find("Player").GetComponentInChildren<Camera>();
			_camera.transform.position = new Vector3(0, 0, -10);
			_camera.orthographicSize = 6f;

			_playerInputActions = new PlayerInputActions();

			_panSpeed = 8.0f;
			_zoomSpeed = 8.0f;

			_pan = _playerInputActions.Player.Pan;
			_zoom = _playerInputActions.Player.Zoom;
			_select = _playerInputActions.Player.Select;
			
			_select.performed += Select;
		}

		void OnEnable()
		{
			_pan.Enable();
			_zoom.Enable();
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

			_camera.transform.position = Vector3.Lerp(
				_camera.transform.position,
				_camera.transform.position + panDisplacement,
				Time.deltaTime
			);

			float zoomValue = _zoom.ReadValue<float>();
			float zoomDisplacement = _zoomSpeed * zoomValue;

			_camera.orthographicSize = Mathf.Lerp(
				_camera.orthographicSize,
				_camera.orthographicSize + zoomDisplacement,
				Time.deltaTime
			);

			_camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize, 2f, 20f);
		}

		private void Select(InputAction.CallbackContext obj)
		{

		}
	}
}
