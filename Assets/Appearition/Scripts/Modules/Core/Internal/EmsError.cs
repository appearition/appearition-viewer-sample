// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: EmsError.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics;

namespace Appearition.Common
{
    /// <summary>
    /// Object containing any _errors or messages from the EMS regarding the EMS request it was used on.
    /// </summary>
    public struct EmsError
    {
        /// <summary>
        /// Contains all the _errors and messages from the EMS that occured during this request.
        /// </summary>
        public List<string> Errors;

        public int errorCode;

        /// <summary>
        /// Returns whether or not any _errors happened during the EMS request.
        /// </summary>
        /// <value><c>true</c> if has _errors; otherwise, <c>false</c>.</value>
        public bool HasErrors
        {
            get { return Errors != null && Errors.Count > 0; }
        }

        public EmsError(string newError, int newErrorCode = 0)
        {
            Errors = new List<string> {newError};
            errorCode = newErrorCode;
        }

        public EmsError(IEnumerable<string> newErrors, int newErrorCode = 0)
        {
            Errors = new List<string>();
            if (newErrors != null)
                Errors.AddRange(newErrors);
            errorCode = newErrorCode;
        }

        /// <summary>
        /// Resets this error container to a blank default state.
        /// </summary>
        public void ResetToDefault()
        {
            if (Errors == null)
                Errors = new List<string>();
            else
                Errors.Clear();
        }

        public override string ToString()
        {
            string outcome = ""; // "EMS Error:\n";

            for (int i = 0; i < Errors.Count; i++)
            {
                outcome += Errors[i];
                if (i + 1 > Errors.Count)
                    outcome += "\n";
            }

            //UnityEngine.Debug.LogError(outcome + ", " + Errors.Count);

            return outcome;
        }

        #region Error Code Utilities

        /// <summary>
        /// Whether the error occured without the EMS getting involved.
        /// </summary>
        public bool IsErrorLocal => errorCode == 0;
        /// <summary>
        /// Whether the error is caused by unexpected data outcome despite the request being successful.
        /// </summary>
        public bool IsMarkedAsSuccessful => errorCode == 200;
        /// <summary>
        /// Whether the error is caused by an internal server error. Do note that unexpected request content may cause this outcome.
        /// </summary>
        public bool IsServerError => errorCode == 500;
        /// <summary>
        /// Whether the error is caused by low authentication level. Often, this happens when a user token has been expired, and the user should be prompted to login again.
        /// </summary>
        public bool IsAuthenticationError => errorCode == 401;
        /// <summary>
        /// Whether the request hasn't found a valid end point.
        /// </summary>
        public bool IsRequestNotFound => errorCode == 404;
        
        #endregion
    }
}