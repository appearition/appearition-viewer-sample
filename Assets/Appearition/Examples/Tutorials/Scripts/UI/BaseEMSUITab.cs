// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: "BaseEMSUITab.cs" 
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using UnityEngine;

namespace Appearition.Example
{
    public abstract class BaseEMSUITab : MonoBehaviour
    {
        //References
        public RectTransform containerRect;

        //Internal Variables
        public Vector2 retractedAndExpandedHeights = new Vector2(70, 70);
        float expansionProgress = 1;
        float targetExpansion = 1;
        float lerpSpeed = 8;
        public bool startExpanded = false;

        protected virtual void Awake()
        {
            expansionProgress = targetExpansion = (startExpanded ? 1 : 0);

            //Set default expansion state.
            if (containerRect != null)
            {
                //Set default value
                containerRect.sizeDelta = new Vector2(containerRect.sizeDelta.x, Mathf.Lerp(retractedAndExpandedHeights.x, retractedAndExpandedHeights.y, expansionProgress));
            }
        }

        public void OnTitleButtonPressed()
        {
            //Flip it!
            targetExpansion = Mathf.Abs(1 - targetExpansion);
        }

        protected void Update()
        {
            if (containerRect == null)
                return;

            //Change the retraction / expansion !
            if ((expansionProgress < 1 && targetExpansion >= 1) || (expansionProgress > 0 && targetExpansion <= 0))
            {
                //handle _progress !
                expansionProgress = Mathf.Clamp01(Mathf.Lerp(expansionProgress, targetExpansion, Time.deltaTime * lerpSpeed));
                containerRect.sizeDelta = new Vector2(containerRect.sizeDelta.x, Mathf.Lerp(retractedAndExpandedHeights.x, retractedAndExpandedHeights.y, expansionProgress));
            }
        }

        #region Utilities

        /// <summary>
        /// Opens a URL in the external browser. Most likely used by UI buttons.
        /// </summary>
        /// <param name="url"></param>
        public void OpenUrl(string url)
        {
            Application.OpenURL(url);
        }

        #endregion
    }
}