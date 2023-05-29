// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: AppearitionRequest.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using Appearition.API;
using Appearition.Internal;

namespace Appearition
{
    public sealed class AppearitionRequest<T> : AppearitionBaseRequest<T> where T : BaseApi, new()
    {
    }
}