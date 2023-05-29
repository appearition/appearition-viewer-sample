// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: "MakeObjectAlwaysLastInHierarchyB.cs" 
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using UnityEngine;

namespace Appearition.Example
{
    /// <summary>
    /// Utility class to ensure that an object is always the bottom child in the hierarchy.
    /// </summary>
    //[ExecuteInEditMode]
    public class MakeObjectAlwaysLastInHierarchyB : MonoBehaviour
    {
        void LateUpdate()
        {
            if (transform.parent != null && transform.parent.GetChild(transform.parent.childCount - 1).GetHashCode() != transform.GetHashCode())
            {
                Transform parent = transform.parent;
                transform.SetParent(null, true);
                transform.SetParent(parent);
            }
        }
    }
}