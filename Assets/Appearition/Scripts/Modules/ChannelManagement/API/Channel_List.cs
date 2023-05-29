// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: Channel_List.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using Appearition.API;

namespace Appearition.ChannelManagement.API
{
    /// <summary>
    /// Class holder for the Channel / List
    /// </summary>
    [System.Serializable]
    public class Channel_List : BaseApiGet
    {
        public override int ApiVersion => 2;
        
        //Variables
        public ApiData Data;

        //SubClasses
        [System.Serializable]
        public class ApiData
        {
            //Variables
            public Channel[] channels;
        }
    }
}