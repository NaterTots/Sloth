using UnityEngine;
using System.Collections;

//the interface that all controllers must implement
public interface IController
{
    //TODO: not sure yet what should be in here.  in the past i've had:
    //distinct name for the controller
    //an Order/Importance to denote in which order the controllers should take action
    //project-specific construction/destruction methods

    void Cleanup();
}