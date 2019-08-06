﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

namespace Microsoft.Identity.Client
{
    /// <summary>
    /// This exception class is to inform developers that UI interaction is required for authentication to
    /// succeed. It's thrown when calling <see cref="ClientApplicationBase.AcquireTokenSilent(System.Collections.Generic.IEnumerable{string}, IAccount)"/> or one
    /// of its overrides, and when the token does not exists in the cache, or the user needs to provide more content, or perform multiple factor authentication based
    /// on Azure AD policies, etc..
    /// For more details, see https://aka.ms/msal-net-exceptions
    /// </summary>
    public class MsalUiRequiredException : MsalServiceException
    {
        private readonly UiRequiredExceptionClassification _classification;

        /// <summary>
        /// Initializes a new instance of the exception class with a specified
        /// error code and error message.
        /// </summary>
        /// <param name="errorCode">
        /// The error code returned by the service or generated by the client. This is the code you can rely on
        /// for exception handling.
        /// </param>
        /// <param name="errorMessage">The error message that explains the reason for the exception.</param>
        public MsalUiRequiredException(string errorCode, string errorMessage) :
            this(errorCode, errorMessage, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the exception class with a specified
        /// error code, error message and inner exception indicating the root cause.
        /// </summary>
        /// <param name="errorCode">
        /// The error code returned by the service or generated by the client. This is the code you can rely on
        /// for exception handling.
        /// </param>
        /// <param name="errorMessage">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">Represents the root cause of the exception.</param>
        public MsalUiRequiredException(string errorCode, string errorMessage, Exception innerException) :
            this(errorCode, errorMessage, innerException, UiRequiredExceptionClassification.None)
        {
        }

        /// <summary>
        /// Initializes a new instance of the exception class with a specified
        /// error code, error message and inner exception indicating the root cause.
        /// </summary>
        /// <param name="errorCode">
        /// The error code returned by the service or generated by the client. This is the code you can rely on
        /// for exception handling.
        /// </param>
        /// <param name="errorMessage">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">Represents the root cause of the exception.</param>
        /// <param name="classification">A higher level description for this exception, that allows handling code to 
        /// understand what type of action it needs to take to resolve the issue. </param>
        public MsalUiRequiredException(string errorCode, string errorMessage, Exception innerException, UiRequiredExceptionClassification classification) :
            base(errorCode, errorMessage, innerException)
        {
            _classification = classification;
        }

        /// <summary>
        /// Classification of the conditional access error, enabling you to do more actions or inform the user depending on your scenario. 
        /// See https://aka.ms/msal-net-UiRequiredException for more details.
        /// </summary>
        public UiRequiredExceptionClassification Classification
        {
            get
            {
                if (string.Equals(base.SubError, MsalError.BasicAction, StringComparison.OrdinalIgnoreCase))
                {
                    return UiRequiredExceptionClassification.BasicAction;
                }

                if (string.Equals(base.SubError, MsalError.AdditionalAction, StringComparison.OrdinalIgnoreCase))
                {
                    return UiRequiredExceptionClassification.AdditionalAction;
                }

                if (string.Equals(base.SubError, MsalError.MessageOnly, StringComparison.OrdinalIgnoreCase))
                {
                    return UiRequiredExceptionClassification.MessageOnly;
                }

                if (string.Equals(base.SubError, MsalError.ConsentRequired, StringComparison.OrdinalIgnoreCase))
                {
                    return UiRequiredExceptionClassification.ConsentRequired;
                }

                if (string.Equals(base.SubError, MsalError.UserPasswordExpired, StringComparison.OrdinalIgnoreCase))
                {
                    return UiRequiredExceptionClassification.UserPasswordExpired;
                }

                return _classification;
            }
        }
    }
}
