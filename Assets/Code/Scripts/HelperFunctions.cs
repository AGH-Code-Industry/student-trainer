using UnityEngine;

public static class HelperFunctions
{
    private const int MAX_WHILE_TRIES = 20;

    /// <summary>
    /// Traverses the hierarhy tree upwards, looking for a component of the provided type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="startingObject">The object from which to begin the search</param>
    /// <returns>Reference to the component of the provided type, null if none were found</returns>
    public static T GetComponentUpTree<T>(GameObject startingObject) where T : class
    {
        T component = null;
        if (startingObject.TryGetComponent<T>(out component))
        {
            //Debug.Log($"Found component of type {typeof(T)} at the first try!");
            return component;
        }

        int tries = 0;
        Transform root = startingObject.transform.root;
        Transform parent = startingObject.transform.parent;
        while(parent != null)
        {
            if (parent.TryGetComponent<T>(out component))
            {
                //Debug.Log($"Found component of type {typeof(T)} at the {tries + 2} try.");
                return component;
            }

            parent = parent.parent;
            tries++;
            if(tries > MAX_WHILE_TRIES)
            {
                //Debug.LogError("GetComponentUpTree(): max number of tries (50) in the while loop reached!");
                return null;
            }
        }

        //Debug.Log($"Traversed the entire tree and didn't find the desired {typeof(T)} component.");
        return null;
    }
}