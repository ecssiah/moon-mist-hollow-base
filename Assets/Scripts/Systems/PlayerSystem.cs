using UnityEngine;
using UnityEngine.InputSystem;

namespace MMH
{
	public class PlayerSystem : MonoBehaviour
	{
		private float panSpeed = 8.0f;
		private float zoomSpeed = 8.0f;

		private Camera playerCamera;

		private PlayerInputActions playerInputActions;
		private InputAction pan;
		private InputAction zoom;
		private InputAction select;

		void Awake()
		{
			playerInputActions = new PlayerInputActions();
		}

		private void Start()
		{
			TimeSystem.OnTick += OnTick;
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
			Debug.Log("Select!");
		}

		private void OnTick(object sender, TimeSystem.OnTickEventArgs onTickEventArgs)
		{
			Debug.Log("Tick " + onTickEventArgs.tick);
		}
	}
}
