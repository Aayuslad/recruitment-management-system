using Microsoft.AspNetCore.Mvc;

using Server.Core.Results;

namespace Server.Core.Extensions
{
    public static class ResultExtensions
    {
        public static IActionResult ToActionResult(this Result result, ControllerBase controller)
        {
            return result.IsSuccess
                ? controller.StatusCode(result.StatusCode, new { message = "Success" })
                : controller.StatusCode(result.StatusCode, new { error = result.ErrorMessage });
        }

        public static IActionResult ToActionResult<T>(this Result<T> result, ControllerBase controller)
        {
            return result.IsSuccess
                ? controller.StatusCode(result.StatusCode, result.Value)
                : controller.StatusCode(result.StatusCode, new { error = result.ErrorMessage });
        }
    }
}