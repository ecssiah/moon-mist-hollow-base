using UnityEngine;

namespace MMH
{
	[CreateAssetMenu(fileName = "New Render Settings", menuName = "Render Settings")]
	public class RenderSettings : ScriptableObject
	{
		[Header("Camera")]
		public float PanSpeed = 8f;
		public float ZoomSpeed = 8f;

		public float MaxZoom = 20f;
		public float MinZoom = 2f;

		public float DefaultZoom = 6f;
	}
}
