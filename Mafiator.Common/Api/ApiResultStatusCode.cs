using System.ComponentModel.DataAnnotations;

namespace Mafiator.Common.Api;

/// <summary>
/// Api result status code.
/// </summary>
public enum ApiResultStatusCode
{
    /// <summary>
    /// Success.
    /// </summary>
    [Display(Name = "Succeeded")]
    Success = 0,

    /// <summary>
    /// Server error.
    /// </summary>
    [Display(Name = "Internal server error occurred")]
    ServerError = 1,

    /// <summary>
    /// Bad request.
    /// </summary>
    [Display(Name = "The submitted parameters are not valid")]
    BadRequest = 2,

    /// <summary>
    /// Not found.
    /// </summary>
    [Display(Name = "Result not found")]
    NotFound = 3,

    /// <summary>
    /// List empty.
    /// </summary>
    [Display(Name = "List is empty")]
    ListEmpty = 4,

    /// <summary>
    /// Logic error.
    /// </summary>
    [Display(Name = "An error occurred while processing")]
    LogicError = 5,

    /// <summary>
    /// Unauthorized.
    /// </summary>
    [Display(Name = "Authentication error occurred")]
    Unauthorized = 6,

    /// <summary>
    /// Forbidden.
    /// </summary>
    [Display(Name = "Access permission has not been issued")]
    Forbidden = 7,

    /// <summary>
    /// Conflict.
    /// </summary>
    [Display(Name = "Confliction has occurred")]
    Conflict = 8,
}