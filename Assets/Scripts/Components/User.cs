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

		private UserInputActions _userInputActions;

		private InputAction _panAction;
		private InputAction _zoomAction;
		private InputAction _primaryAction;
		private InputAction _secondaryAction;

		void Awake()
		{
			_renderSettings = Resources.Load<RenderSettings>("Settings/RenderSettings");

			_panSpeed = _renderSettings.PanSpeed;
			_zoomSpeed = _renderSettings.ZoomSpeed;
			
			_camera = GameObject.Find("User").GetComponentInChildren<Camera>();
			_camera.transform.position = new Vector3(0, 0, -10);
			_camera.orthographicSize = _renderSettings.DefaultZoom;

			_userInputActions = new UserInputActions();

			_panAction = _userInputActions.Player.Pan;
			_zoomAction = _userInputActions.Player.Zoom;
			_primaryAction = _userInputActions.Player.Primary;
			_secondaryAction = _userInputActions.Player.Secondary;
		}

		void OnEnable()
		{
			_panAction.Enable();
			_zoomAction.Enable();
			_primaryAction.Enable();
			_secondaryAction.Enable();

			_primaryAction.performed += PrimaryAction;
			_secondaryAction.performed += SecondaryAction;
		}

		void Update()
		{
			UpdatePan();
			UpdateZoom();
		}

		private void UpdatePan()
		{
			Vector2 panValue = _panAction.ReadValue<Vector2>();
			Vector3 panDisplacement = _panSpeed * panValue;

			_camera.transform.position = Vector3.Lerp(
				_camera.transform.position,
				_camera.transform.position + panDisplacement,
				Time.deltaTime
			);
		}

		private void UpdateZoom()
		{
			float zoomValue = _zoomAction.ReadValue<float>();
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
			_panAction.Disable();
			_zoomAction.Disable();
			_primaryAction.Disable();
			_secondaryAction.Disable();
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
