using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class ObjectPool : MonoBehaviour
    {
        public GameObject prefabToPool;
        public int poolCount = 30;
        public Transform parentTransform;
        private readonly List<GameObject> _pool = new();
        private void Start()
        {
            if (prefabToPool == null)
            {
                enabled = false;
                throw new NullReferenceException("prefab needs to be set before object pool works");
            }
            for (var i = 0; i < poolCount; i++)
            {
                var poolObject = CreatePoolObject();
                poolObject.name = $"{prefabToPool.name} {i}";
                _pool.Add(poolObject);
            }
        }
        private GameObject CreatePoolObject()
        {
            var poolObject = Instantiate(prefabToPool);
            poolObject.SetActive(false);
            poolObject.layer = gameObject.layer;
            if (parentTransform) poolObject.transform.parent = parentTransform;
            return poolObject;
        }
        public GameObject GetPoolObject()
        {
            foreach (var poolObject in _pool)
                if (poolObject.activeSelf == false)
                    return poolObject;
            var newPoolObject = CreatePoolObject();
            _pool.Add(newPoolObject);
            return newPoolObject;
        }
        public void RecyclePoolObject(GameObject maybePoolObject)
        {
            if (_pool.Contains(maybePoolObject))
                maybePoolObject.SetActive(false);
        }
    }
}
