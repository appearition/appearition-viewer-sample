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
    public class USAEndPoint : EndPoint
    {
        public override string displayName
        {
            get { return "Appearition USA"; }
        }

        public override string endPointURL
        {
            get { return "https://apiusa.appearition.com"; }
        }

        public override string portalURL
        {
            get { return "https://www.loginusa.appearition.com"; }
        }
    }
}