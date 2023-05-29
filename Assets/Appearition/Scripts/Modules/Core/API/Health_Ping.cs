// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: Health_Ping.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

namespace Appearition.API
{
    /// <summary>
    /// Simple API which is used to test the connection to the EMS using a tenant and end point.
    /// https://api.appearition.com/TenantName/api/Health/Ping/0 , where 0 is Channel ID 
    /// </summary>
    [System.Serializable]
    public class Health_Ping : BaseApiGet
    {
        public override int ApiVersion
        {
            get { return 1; }
        }

        public override bool BypassInternetCheck => true;

        public string Data { get; set; }
        
    }
}