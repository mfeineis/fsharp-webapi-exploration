namespace Exploration.Web

open System.Net
open Microsoft.AspNetCore.Mvc

[<RequireQualifiedAccess>]
module WebApiAdapter =
    // See https://blog.ploeh.dk/2015/03/19/posting-json-to-an-f-web-api/
    // See https://blog.ploeh.dk/2017/03/30/a-reusable-apicontroller-adapter/

    open Exploration

    let toHttpResult (controller: ControllerBase) result : IActionResult =
        match result with
        | Success (Ok resp) -> controller.Ok resp :> _
        | Success (Created resp) -> controller.Content resp :> _
        // controller.Content (location, resp) :> _
        // CreatedNegotiatedContentResult (location, resp, controller) :> _
        | Failure RouteFailure -> // NotFoundResult controller :> _
            controller.NotFound() :> _
        | Failure (ValidationFailure msg) -> controller.BadRequest msg :> _
        // BadRequestErrorMessageResult (msg, controller) :> _
        | Failure (IntegrationFailure msg) ->
            controller.StatusCode(
                LanguagePrimitives.EnumToValue
                    HttpStatusCode.InternalServerError,
                msg
            )
            :> _
        // controller.Problem (detail = msg, statusCode = HttpStatusCode.InternalServerError) :> _
        // let resp =
        //     controller.Request.CreateErrorResponse (
        //         HttpStatusCode.InternalServerError,
        //         msg)
        // ResponseMessageResult resp :> _
        | Failure StabilityFailure ->
            let res = controller.Content("")

            res.StatusCode <-
                LanguagePrimitives.EnumToValue HttpStatusCode.ServiceUnavailable
            // TODO: Retry Header
            res :> _
// let resp =
//     new HttpResponseMessage (HttpStatusCode.ServiceUnavailable)
// resp.Headers.RetryAfter <-
//     RetryConditionHeaderValue (TimeSpan.FromMinutes 5.)
// ResponseMessageResult resp :> _
