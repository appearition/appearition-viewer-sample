// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: ArTarget.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

namespace Appearition.ArTargetImageAndMedia
{
    /// <summary>
    /// Container of an Appearition Asset JSON ApiData model.
    /// </summary>
    [System.Serializable]
    public class ArTarget : Asset
    {
        //Variables 
        public int arTargetId;
        public bool isPublished;
        public bool isLocked;
        public bool isInMarket;
        public bool isFromMarket;
        

        /// <summary>
        /// Whether or not the experience can be edited at all. If false, any edit API will return with an error.
        /// </summary>
        public bool canEditExperience;
        public bool canDeleteExperience;

        public ArTarget()
        {
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="cc">C.</param>
        public ArTarget(Asset cc) : base(cc)
        {
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="cc">C.</param>
        public ArTarget(ArTarget cc)
        {
            CopyValuesFrom(cc);
        }

        public void CopyValuesFrom(ArTarget cc) 
        {
            //Copy asset content
            base.CopyValuesFrom(cc);

            arTargetId = cc.arTargetId;
            isPublished = cc.isPublished;
            isLocked = cc.isLocked;
            isInMarket = cc.isInMarket;
            isFromMarket = cc.isFromMarket;
            canEditExperience = cc.canEditExperience;
            canEditExperience = cc.canEditExperience;
        }
    }
}