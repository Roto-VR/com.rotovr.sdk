
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace com.rotovr.sdk.sample
{
    public class SampleUnityMainThreadDispatcher  : MonoBehaviour, IUnityMainThreadDispatcher
    {
        private static readonly Queue<Action> _executionQueue = new Queue<Action>();

        public void Update()
        {
            lock (_executionQueue)
            {
                while (_executionQueue.Count > 0)
                {
                    _executionQueue.Dequeue().Invoke();
                }
            }
        }

        /// <summary>
        /// Locks the queue and adds the IEnumerator to the queue
        /// </summary>
        /// <param name="action">IEnumerator function that will be executed from the main thread.</param>
        public void Enqueue(IEnumerator action)
        {
            lock (_executionQueue)
            {
                _executionQueue.Enqueue(() => { StartCoroutine(action); });
            }
        }

        /// <summary>
        /// Locks the queue and adds the Action to the queue
        /// </summary>
        /// <param name="action">function that will be executed from the main thread.</param>
        public void Enqueue(Action action)
        {
            Enqueue(ActionWrapper(action));
        }

        /// <summary>
        /// Locks the queue and adds the Action to the queue, returning a Task which is completed when the action completes
        /// </summary>
        /// <param name="action">function that will be executed from the main thread.</param>
        /// <returns>A Task that can be awaited until the action completes</returns>
        public Task EnqueueAsync(Action action)
        {
            var tcs = new TaskCompletionSource<bool>();

            void WrappedAction()
            {
                try
                {
                    action();
                    tcs.TrySetResult(true);
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            }

            Enqueue(ActionWrapper(WrappedAction));
            return tcs.Task;
        }


        IEnumerator ActionWrapper(Action a)
        {
            a();
            yield return null;
        }


        private static SampleUnityMainThreadDispatcher _instance = null;

        public static bool Exists()
        {
            return _instance != null;
        }

        public static SampleUnityMainThreadDispatcher Instance()
        {
            if (!Exists())
            {
                GameObject go = new GameObject("MainThreadDispatcher");
                _instance = go.AddComponent<SampleUnityMainThreadDispatcher>();
                DontDestroyOnLoad(go);
            }

            return _instance;
        }


        void OnDestroy()
        {
            _instance = null;
        }
    }
}
