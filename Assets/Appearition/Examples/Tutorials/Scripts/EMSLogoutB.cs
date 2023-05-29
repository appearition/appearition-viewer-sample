// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: "EMSLogoutB.cs" 
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using UnityEngine;
using System;
using Appearition.AccountAndAuthentication;

namespace Appearition.Example
{
    /// <summary>
    /// Simple class handling logging out from the EMS, hence removing access to authenticated-level features.
    /// </summary>
    public class EMSLogoutB : MonoBehaviour
    {
        public void Logout(Action<bool> callback)
        {
            AccountHandler.Logout(false);
        }
    }
}