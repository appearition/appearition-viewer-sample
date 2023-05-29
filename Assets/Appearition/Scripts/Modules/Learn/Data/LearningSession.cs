// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: LearningSession.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Appearition.Learn
{
    /// <summary>
    /// Contains all information that has been logged throughout a learning session.
    /// This container is made to be used on LearnHandler.Tracking( )'s submission.
    /// </summary>
    [System.Serializable]
    public class LearningSession
    {
        //Variables
        public int productId;
        public string sessionKey;
        public string username;
        public string startDateTime;
        public string endDateTime;
        public string deviceInformation;
        public List<LearningSessionNodePath> learningSessionNodePaths;

        public LearningSession()
        {
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="cc"></param>
        public LearningSession(LearningSession cc)
        {
            productId = cc.productId;
            sessionKey = cc.sessionKey;
            username = cc.username;
            startDateTime = cc.startDateTime;
            endDateTime = cc.endDateTime;
            deviceInformation = cc.deviceInformation;
            learningSessionNodePaths = new List<LearningSessionNodePath>(cc.learningSessionNodePaths);
        }
    }
}