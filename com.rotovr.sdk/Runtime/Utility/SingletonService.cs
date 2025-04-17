#if !NO_UNITY
using UnityEngine;

namespace com.rotovr.sdk
{
    static class SingletonService
    {
        static Transform s_ServicesObjectTransform;
        public static Transform Parent
        {
            get
            {
                if (s_ServicesObjectTransform == null)
                {
                    s_ServicesObjectTransform = new GameObject("RotoVR").transform;
                    Object.DontDestroyOnLoad(s_ServicesObjectTransform);
                }

                return s_ServicesObjectTransform;
            }
        }
    }
}
#endif