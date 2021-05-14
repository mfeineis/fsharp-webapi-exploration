namespace Exploration

open System

type Success<'a> =
    | Ok of 'a
    | Created of 'a

type Failure<'a> =
    | RouteFailure
    | ValidationFailure of 'a
    | IntegrationFailure of 'a
    | StabilityFailure

type HandlerResult<'res, 'err> =
    | Success of Success<'res>
    | Failure of Failure<'err>


// [<CLIMutable>]
type WeatherForecast =
    { Date: DateTime
      TemperatureC: int
      Summary: string }

    member this.TemperatureF =
        32.0 + (float this.TemperatureC / 0.5556)
