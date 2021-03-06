using UnityEngine;

namespace MMH
{
	[CreateAssetMenu(fileName = "RenderSettings", menuName = "Render Settings")]
	public class RenderSettings : ScriptableObject
	{
		[Header("Camera")]
		public float PanSpeed = 8f;
		public float ZoomSpeed = 8f;

		[Space()]
		public float DefaultZoom = 6f;

		[Space()]
		public float MinZoom = 2f;
		public float MaxZoom = 20f;

		[Header("Entities")]
		public float EntitySpacing = 0.00001f;
	}
}
