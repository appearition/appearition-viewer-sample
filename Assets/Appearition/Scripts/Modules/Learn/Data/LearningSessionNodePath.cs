// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: LearningSessionNodePath.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

namespace Appearition.Learn
{
    [System.Serializable]
    public class LearningSessionNodePath
    {
        //Variables
        public int nodeId;
        public string startDateTime;
        public string endDateTime;
        public LearningSessionNodePathActivity[] learningSessionNodePathActivities;
    }
}