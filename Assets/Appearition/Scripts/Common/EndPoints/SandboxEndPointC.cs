// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: USAEndPointC.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Appearition.Internal.EndPoints
{
    public class SandboxEndPoint : EndPoint
    {
        public override string displayName
        {
            get { return "Appearition Sandbox"; }
        }

        public override string endPointURL
        {
            get { return "https://sandboxapi.appearition.com"; }
        }

        public override string portalURL
        {
            get { return "https://sandbox.appearition.com/"; }
        }
    }
}