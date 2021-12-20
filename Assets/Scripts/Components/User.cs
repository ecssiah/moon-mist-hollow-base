using UnityEngine;
using UnityEngine.InputSystem;

namespace MMH
{
	public class User : MonoBehaviour
	{
		private RenderSettings _renderSettings;

		private float _panSpeed;
		private float _zoomSpeed;

		private Camera _camera;

		private PlayerInputActions _playerInputActions;

		private InputAction _pan;
		private InputAction _zoom;
		private InputAction _primary;
		private InputAction _secondary;

		void Awake()
		{
			_renderSettings = Resources.Load<RenderSettings>("Settings/RenderSettings");

			_panSpeed = _renderSettings.PanSpeed;
			_zoomSpeed = _renderSettings.ZoomSpeed;
			
			_camera = GameObject.Find("User").GetComponentInChildren<Camera>();
			_camera.transform.position = new Vector3(0, 0, -10);
			_camera.orthographicSize = _renderSettings.DefaultZoom;

			_playerInputActions = new PlayerInputActions();

			_pan = _playerInputActions.Player.Pan;
			_zoom = _playerInputActions.Player.Zoom;
			_primary = _playerInputActions.Player.Primary;
			_secondary = _playerInputActions.Player.Secondary;
		}

		void OnEnable()
		{
			_pan.Enable();
			_zoom.Enable();
			_primary.Enable();
			_secondary.Enable();

			_primary.performed += PrimaryAction;
			_secondary.performed += SecondaryAction;
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

			_camera.orthographicSize = Mathf.Clamp(
				_camera.orthographicSize, 
				_renderSettings.MinZoom, 
				_renderSettings.MaxZoom
			);
		}

		void OnDisable()
		{
			_pan.Disable();
			_zoom.Disable();
			_primary.Disable();
			_secondary.Disable();
		}

		private void PrimaryAction(InputAction.CallbackContext callbackContext)
		{
			Debug.Log($"Primary Action Fired");
		}

		private void SecondaryAction(InputAction.CallbackContext callbackContext)
		{
			Debug.Log($"Secondary Action Fired");
		}
	}
}
