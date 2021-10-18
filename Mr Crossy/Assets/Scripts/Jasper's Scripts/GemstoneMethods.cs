
using UnityEngine;
using UnityEngine.AI;

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

        public static float GetPathLength(NavMeshPath path)
        {
            float pathLength = 0.0f;
            if (path.corners.Length > 1)
            {
                for (int i = 1; i < path.corners.Length; ++i)
                {
                    pathLength += Vector3.Distance(path.corners[i - 1], path.corners[i]);
                }
            }

            return pathLength;
        }
    }
}