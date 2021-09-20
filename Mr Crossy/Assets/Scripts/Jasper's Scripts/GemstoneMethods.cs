
using UnityEngine;

namespace Gemstone
{
	public static class Emerald
	{

        /// <summary>
        /// Gets a random point in a circle on a flat plane
        /// </summary>
        /// <param name="radius">Radius of allowed area.</param>
        /// <param name="centre">Centre of allowed area.</param>
        /// <returns>Random point within specified area as a Vector3.</returns>
        public static Vector3 GetPointInCircle(Vector3 radius, Vector3 centre)
        {
            Vector2 pointInCircle;
            pointInCircle = Random.insideUnitCircle * radius;

            Vector3 areaWithCentreOffset = new Vector3(pointInCircle.x + centre.x, 0, pointInCircle.y + centre.z);

            return areaWithCentreOffset;
        }

        public static float ToRadians(this float angleInDeg)
        {
            float radians = angleInDeg * Mathf.Deg2Rad;

            return radians;
        }

        
    }
}