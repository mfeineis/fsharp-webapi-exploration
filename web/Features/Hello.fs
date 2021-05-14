namespace Exploration.Web.Features

open Microsoft.AspNetCore.Mvc

open Exploration
open Exploration.Web

[<ApiController>]
[<Route("api/[controller]")>]
type HelloController () =
    inherit ControllerBase()

    [<HttpGet>]
    member this.Get() =
        WebApiAdapter.toHttpResult this (Success (Ok "Hello, World!"))

