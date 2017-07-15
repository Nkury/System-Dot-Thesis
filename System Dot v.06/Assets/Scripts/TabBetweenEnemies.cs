using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TabBetweenEnemies : MonoBehaviour {

    public static List<EnemyTerminal> onScreenObjects;

    int index = 0;
    int lastIndex = -1;
    int objectCount = 0;

	// Use this for initialization
	void Start () {
        onScreenObjects = new List<EnemyTerminal>(GameObject.FindObjectsOfType<EnemyTerminal>());
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            objectCount = 0;

            while (!inCameraView(onScreenObjects[index].gameObject) && objectCount < onScreenObjects.Count)
            {
                index = (index + 1) % onScreenObjects.Count;
                objectCount++;
            }      

            if (objectCount < onScreenObjects.Count)
            {
                // if this is the first object being tabbed to
                if (lastIndex == -1)
                {
                    lastIndex = index;
                }

                Debug.Log("last object tabbed to: " + onScreenObjects[lastIndex].gameObject.name);
                onScreenObjects[lastIndex].OnMouseExit();

                lastIndex = index;

                Debug.Log("object being tabbed to: " + onScreenObjects[lastIndex].gameObject.name);
                 onScreenObjects[index].OnMouseOver();

                index = (index + 1) % onScreenObjects.Count;
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            onScreenObjects[index].OnMouseDown();
        }
	}

    bool inCameraView(GameObject target)
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(target.transform.position);
        bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

        return onScreen;
    }

    public static void removeEnemy(EnemyTerminal obj)
    {
        onScreenObjects.Remove(obj);
    }
}
