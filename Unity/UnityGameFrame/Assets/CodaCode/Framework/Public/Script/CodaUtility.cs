using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Coda
{
    public static class CodaUtility
    {
        public delegate void VoidDelegate();
        public delegate void VoidDelegate<T>(T t);
        public delegate void VoidDelegate<T, V>(T t, V v);
        public delegate void VoidDelegate<T, V, Y>(T t, V v, Y y);

        public delegate bool BoolDelegate();
        public delegate bool BoolDelegate<T>(T t);
        public delegate bool BoolDelegate<T, V>(T t, V v);
        public delegate bool BoolDelegate<T, V, Y>(T t, V v, Y y);

        public delegate int IntDelegate();
        public delegate int IntDelegate<T>(T t);
        public delegate int IntDelegate<T, V>(T t, V v);
        public delegate int IntDelegate<T, V, Y>(T t, V v, Y y);

        public delegate float FloatDelegate();
        public delegate float FloatDelegate<T>(T t);
        public delegate float FloatDelegate<T, V>(T t, V v);
        public delegate float FloatDelegate<T, V, Y>(T t, V v, Y y);

        public delegate Vector3 Vector3Delegate();
        public delegate Vector3 Vector3Delegate<T>(T t);
        public delegate Vector3 Vector3Delegate<T, V>(T t, V v);
        public delegate Vector3 Vector3Delegate<T, V, U>(T t, V v, U u);


        /// <summary>
        /// Check the sign of [f1] is the same as [f2] or not, if [f1] or [f2] is zero, return true.
        /// </summary>
        public static bool SignSameCheck(float f1, float f2)
        {
            if (f1 == 0 || f2 == 0) return true;

            return Mathf.Sign(f1) == Mathf.Sign(f2);
        }


        /// <summary>
        /// Copy a list for different reference.
        /// </summary>
        public static List<T> Copy<T>(this List<T> copyList, BoolDelegate<T> filterFunc = null)
        {
            if (copyList == null)
                return null;

            List<T> result = new List<T>();
            for (int i = 0; i < copyList.Count; i++)
            {
                if (filterFunc != null && filterFunc(copyList[i])) continue;

                result.Add(copyList[i]);
            }
            return result;
        }
    }
}
