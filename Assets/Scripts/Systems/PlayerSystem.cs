using UnityEngine;
using UnityEngine.InputSystem;

namespace MMH
{
	public class PlayerSystem : MonoBehaviour
	{
		private static PlayerSystem _instance;
		public static PlayerSystem Instance { get { return _instance; } }

		private float panSpeed = 8.0f;
		private float zoomSpeed = 8.0f;

		private Camera playerCamera;

		private PlayerInputActions playerInputActions;

		private InputAction pan;
		private InputAction zoom;
		private InputAction select;

		void Awake()
		{
			if (_instance != null && _instance != this)
			{
				Destroy(this.gameObject);
			}
			else
			{
				_instance = this;
			}

			TimeSystem.OnTick += OnTick;

			playerInputActions = new PlayerInputActions();
		}

		private void Start()
		{
		}

		void OnEnable()
		{
			playerCamera = GetComponentInChildren<Camera>();

			pan = playerInputActions.Player.Pan;
			pan.Enable();

			zoom = playerInputActions.Player.Zoom;
			zoom.Enable();

			select = playerInputActions.Player.Select;
			select.performed += Select;
			select.Enable();
		}

		void OnDisable()
		{
			TimeSystem.OnTick -= OnTick;

			pan.Disable();
			zoom.Disable();
			select.Disable();
		}

		void Update()
		{
			Vector2 panValue = pan.ReadValue<Vector2>();
			Vector3 panDisplacement = panSpeed * panValue;

			transform.position = Vector3.Lerp(
				transform.position,
				transform.position + panDisplacement,
				Time.deltaTime
			);

			float zoomValue = zoom.ReadValue<float>();
			float zoomDisplacement = zoomSpeed * zoomValue;

			playerCamera.orthographicSize = Mathf.Lerp(
				playerCamera.orthographicSize,
				playerCamera.orthographicSize + zoomDisplacement,
				Time.deltaTime
			);

			playerCamera.orthographicSize = Mathf.Clamp(playerCamera.orthographicSize, 2f, 20f);
		}

		private void Select(InputAction.CallbackContext obj)
		{

		}

		private void OnTick(object sender, TimeSystem.OnTickArgs onTickArgs)
		{
		}
	}
}
