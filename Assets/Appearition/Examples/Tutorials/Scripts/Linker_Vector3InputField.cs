// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: "Linker_Vector3InputField.cs" 
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace Appearition.Example
{
    /// <summary>
    /// Utility linker to handle 3 input fields used to type a Vector3, in one.
    /// </summary>
    public class Linker_Vector3InputField : MonoBehaviour
    {
        //References
        public InputField xIF;
        public InputField yIF;
        public InputField zIF;

        //Internal Variables
        public Vector3 GetValueVector3
        {
            get { return new Vector3((xIF == null ? 0f : float.Parse(xIF.text)), (yIF == null ? 0f : float.Parse(yIF.text)), (zIF == null ? 0f : float.Parse(zIF.text))); }
        }

        public string GetValueText
        {
            get { return (xIF == null ? "" : xIF.text) + "," + (yIF == null ? "" : yIF.text) + "," + (zIF == null ? "" : zIF.text); }
        }

        public void SetValue(float x, float y, float z)
        {
            if (xIF != null)
                xIF.text = x.ToString();
            if (yIF != null)
                yIF.text = y.ToString();
            if (zIF != null)
                zIF.text = z.ToString();
        }
    }
}