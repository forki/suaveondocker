#r "packages/Suave/lib/net40/Suave.dll"

open Suave                 // always open suave
open Suave.Types
open Suave.Http.Successful // for OK-result
open Suave.Web             // for config
open System
open System.Net

let config =
  let ip = IPAddress.Parse "0.0.0.0"
  { defaultConfig with bindings = [ HttpBinding.mk HTTP ip 8080us ]}

startWebServer config (OK "Hello World!")
