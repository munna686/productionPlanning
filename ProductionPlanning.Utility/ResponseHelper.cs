using Microsoft.AspNetCore.Mvc;
using ProductionPlanning.Core.DTOs;

namespace ProductionPlanning.Utility
{
    public static class ResponseHelper
    {
        public static IActionResult CustomApiResponse(this ControllerBase controller, ServiceResponse response)
        => response.Success ? controller.Ok(response) : controller.BadRequest(response);
    }
}
