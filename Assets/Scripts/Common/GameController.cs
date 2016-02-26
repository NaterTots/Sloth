using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// The Game Controller is a static object that maintains the other static objects for the life of the application.
/// </summary>
public class GameController : MonoBehaviour
{
    // Any controllers that are created in the design view should be stored in this list
    public List<GameObject> controllers = new List<GameObject>();
    private static bool wasInitialized = false;
    // Use this for initialization
    void Awake()
    {
        if (!wasInitialized)
        {
            DontDestroyOnLoad(this);

            wasInitialized = true;
            foreach (GameObject controller in controllers)
            {
                Debug.Log(controller.name);

                GameObject newController = (GameObject)Instantiate(controller, new Vector3(0, 0, 0), Quaternion.identity);
                newController.transform.parent = this.transform;
                IController newControllerScript = (IController)newController.GetComponent(typeof(IController));

                Resolver.Instance.AddController(newControllerScript);
            }
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }
}