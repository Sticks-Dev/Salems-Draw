using UnityEngine;

namespace Salems_Draw
{
    public static class Vector3Extensions
    {
        public static Vector3 With(this Vector3 vector, float x = 0, float y = 0, float z = 0)
        {
            if (x != 0)
                vector.x = x;
            if (y != 0)
                vector.y = y;
            if (z != 0)
                vector.z = z;
            return vector;
        }
    }
}
