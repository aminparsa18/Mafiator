using System.ComponentModel.DataAnnotations;

namespace Mafiator.Common.Api
{
    public enum ApiResultStatusCode
    {
        [Display(Name = "Succeeded")]
        Success = 0,

        [Display(Name = "Internal server error occurred")]
        ServerError = 1,

        [Display(Name = "The submitted parameters are not valid")]
        BadRequest = 2,

        [Display(Name = "Result not found")]
        NotFound = 3,

        [Display(Name = "List is empty")]
        ListEmpty = 4,

        [Display(Name = "An error occurred while processing")]
        LogicError = 5,

        [Display(Name = "Authentication error occurred")]
        UnAuthorized = 6,

        [Display(Name = "Access permission has not been issued")]
        Forbidden=7,

        [Display(Name = "Confliction has occurred")]
        Conflict=8,
    }
}
