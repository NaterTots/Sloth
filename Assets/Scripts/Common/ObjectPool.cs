using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPool : IEnumerable<KeyValuePair<int, GameObject>>
{
    public GameObject objectPrefab;
    public GameObject parentContainer;

    private struct PoolObject
    {
        internal GameObject gameObject;
        internal bool isActive;
        internal int id;
    }

    public int maxPoolSize;

    PoolObject[] activePoolObjects;
    PoolObject[] inactivePoolObjects;

    //These point to the next position available in the arrays
    private int activeIndex = 0;
    private int inactiveIndex = 0;

    /// <summary>
    /// Note: this is an expensive property.  Don't call it every frame or you're dead meat.
    /// </summary>
    public int ActiveCount
    {
        get
        {
            return UpdateActiveList();
        }
    }

    /// <summary>
    /// If using the Unity inspector or setting the maxPoolSize property beforehand, calling this method will suffice.
    /// </summary>
    public void Initialize()
    {
        activePoolObjects = new PoolObject[maxPoolSize];
        inactivePoolObjects = new PoolObject[maxPoolSize];
    }

    public void Initialize(int poolSize)
    {
        maxPoolSize = poolSize;

        Initialize();
    }

    public void LoadMaxInactive()
    {
        for (int i = 0; i < inactivePoolObjects.Length; i++)
	    {
            if (inactivePoolObjects[i].gameObject == null)
            {
                inactivePoolObjects[i].gameObject = (GameObject)MonoBehaviour.Instantiate(objectPrefab);
                inactivePoolObjects[i].gameObject.transform.parent = parentContainer.transform;
                inactivePoolObjects[i].gameObject.SetActive(false);
                inactivePoolObjects[i].isActive = false;
                inactivePoolObjects[i].id = inactivePoolObjects[i].gameObject.GetInstanceID();
            }
        }

        inactiveIndex = maxPoolSize;
    }

    public delegate void PrepGameObject(ref GameObject gameObject, object optionalParam = null);

    public GameObject InitNewObject(PrepGameObject prep = null, object optionalPrepParam = null)
    {
        //there's a two-phase check to see if there's space for a new object
        //First: check the Active list.  if there's a spot at the end, take it
        //Second: if the active list seems full, update it and move any inactive ones out of it.  Then do step 1 again

        int nextObjectIndex = maxPoolSize;

        //grab the next index
        if (activeIndex < maxPoolSize)
        {
            nextObjectIndex = activeIndex;
        }
        else
        {
            nextObjectIndex = UpdateActiveList();
        }


        if (nextObjectIndex < maxPoolSize)
        {
            ++activeIndex;

            //see if there's an inactive one ready for us
            if (inactiveIndex > 0)
            {
                //remember, this points to the *next* item in the array, so decrementing it will point it to the one we want
                --inactiveIndex;
                activePoolObjects[nextObjectIndex] = inactivePoolObjects[inactiveIndex];
            }
            else
            {
                activePoolObjects[nextObjectIndex].gameObject = UnityEngine.Object.Instantiate(objectPrefab);
                activePoolObjects[nextObjectIndex].gameObject.transform.parent = parentContainer.transform;
            }

            activePoolObjects[nextObjectIndex].isActive = true;
            activePoolObjects[nextObjectIndex].gameObject.SetActive(true);
            activePoolObjects[nextObjectIndex].id = activePoolObjects[nextObjectIndex].gameObject.GetInstanceID();

            if (prep != null)
            {
                prep(ref activePoolObjects[nextObjectIndex].gameObject, optionalPrepParam);
            }

            return activePoolObjects[nextObjectIndex].gameObject;
        }
        else
        {
            Debug.LogError("Pool is full!");
            return null;
        } 
    }

    public void DeactivateObject(GameObject obj)
    {

        for (int i = 0; i < activeIndex; i++)
        {
            if (activePoolObjects[i].id == obj.GetInstanceID())
            {
                activePoolObjects[i].isActive = false;
                activePoolObjects[i].gameObject.SetActive(false);
                inactivePoolObjects[inactiveIndex++] = activePoolObjects[i]; 
            }
        }
    }

    public void DeactivateAll()
    {
        for (int i = 0; i < activeIndex; i++)
        {
            activePoolObjects[i].isActive = false;
            activePoolObjects[i].gameObject.SetActive(false);
            inactivePoolObjects[inactiveIndex++] = activePoolObjects[i];
        }

        activeIndex = 0;
    }

    public bool HasActiveObjects()
    {
        return (activeIndex > 0);
    }

    public int UpdateActiveList()
    {
        int amountToTrickleDown = 0;
        for (int i = 0; i < activeIndex; i++)
        {
            if (activePoolObjects[i].isActive == false ||
                (activePoolObjects[i].gameObject != null &&
                activePoolObjects[i].gameObject.activeInHierarchy == false))
            {
                activePoolObjects[i].isActive = false;
                inactivePoolObjects[inactiveIndex++] = activePoolObjects[i];
                ++amountToTrickleDown;

            }
            else if (amountToTrickleDown > 0)
            {
                activePoolObjects[i - amountToTrickleDown] = activePoolObjects[i];
            }
        }

        activeIndex -= amountToTrickleDown;

        return activeIndex;
    }

    public GameObject this[int key]
    {
        get
        {
            for (int i = 0; i < activeIndex; i++)
            {
                //must check isActive because there's no telling when the activelist was last updated
                if (activePoolObjects[i].isActive && activePoolObjects[i].id == key)
                {
                    return activePoolObjects[i].gameObject;
                }
            }

            return null;
        }
    }

    public bool IsObjectActive(int key)
    {
        bool active = false;

        for (int i = 0; i < activeIndex; i++)
        {
            if (activePoolObjects[i].isActive && activePoolObjects[i].id == key)
            {
                active = true;
                break;
            }
        }

        return active;
    }

    #region IEnumerable

    public IEnumerator<KeyValuePair<int, GameObject>> GetEnumerator()
    {
        for (int i = 0; i < activeIndex; i++)
        {
            if (activePoolObjects[i].isActive)
            {
                yield return new KeyValuePair<int, GameObject>(
                    activePoolObjects[i].id,
                    activePoolObjects[i].gameObject);
            }
        }
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    #endregion IEnumerable
}

