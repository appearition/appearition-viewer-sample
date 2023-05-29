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
    public class TelstraPocEndPoint : EndPoint
    {
        public override string displayName
        {
            get { return "TelstraPOC Sandbox"; }
        }

        public override string endPointURL
        {
            get { return "https://telstrasandboxapi.appearition.com"; }
        }

        public override string portalURL
        {
            get { return "https://telstrasandbox.appearition.com/"; }
        }
    }
}