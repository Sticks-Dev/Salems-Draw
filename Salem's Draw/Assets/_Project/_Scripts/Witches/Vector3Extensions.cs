using UnityEngine;

namespace Salems_Draw
{
    public static class Vector3Extensions
    {
        public static Vector3 With(this Vector3 vector, float? x = null, float? y = null, float? z = null)
        {
            if (x != null)
                vector.x = (float)x;
            if (y != null)
                vector.y = (float)y;
            if (z != null)
                vector.z = (float)z;
            return vector;
        }
    }
}
