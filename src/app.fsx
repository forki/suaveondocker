#if BOOTSTRAP
System.Environment.CurrentDirectory <- __SOURCE_DIRECTORY__
if not (System.IO.File.Exists "paket.exe") then let url = "https://github.com/fsprojects/Paket/releases/download/0.27.2/paket.exe" in use wc = new System.Net.WebClient() in let tmp = System.IO.Path.GetTempFileName() in wc.DownloadFile(url, tmp); System.IO.File.Move(tmp,System.IO.Path.GetFileName url);;
#r "paket.exe"
Paket.Dependencies.Install (System.IO.File.ReadAllText "paket.dependencies")
#endif

//---------------------------------------------------------------------

#I "packages/Suave/lib/net40"
#r "packages/Suave/lib/net40/Suave.dll"

open System
open System.Net
open Suave                 // always open suave
open Suave.Http
open Suave.Http.Applicatives
open Suave.Http.Successful // for OK-result
open Suave.Web             // for config
open Suave.Types     

printfn "initializing script..."

let species = 
  [("Plant (tree)", "Baishan Fir"); ("Insect (butterfly)", "");
   ("Reptile", "Leaf scaled sea-snake");
   ("Insect (damselfly)", "Amani flatwing"); ("Bird", "Araripe manakin");
   ("Insect", "(earwig)"); ("Fish", "Aci Göl toothcarp");
   ("Mammal (bat)", "Bulmer’s fruit bat"); ("Bird", "White bellied heron");
   ("Bird", "Great Indian bustard");
   ("Reptile (tortoise)", "Ploughshare tortoise Angonoka");
   ("Amphibian (toad)", "Rio Pescado stubfoot toad");
   ("Bird", "Madagascar pochard"); ("Fish", "Galapagos damsel fish");
   ("Fish", "Giant yellow croaker");
   ("Reptile (turtle)", "Common batagur Four-toed terrapin");
   ("Plant", "(liverwort)"); ("Mammal", "Hirola (antelope)");
   ("Insect (bee)", "Franklin’s bumblebee");
   ("Mammal (primate)", "Northern muriqui woolly spider monkey");
   ("Mammal", "Pygmy three-toed sloth");
   ("Plant (freshwater)", "(water-starwort)");
   ("Reptile", "Tarzan’s chameleon");
   ("Mammal (rodent)", "Santa Catarina’s guinea pig");
   ("Mammal (primate)", "Roloway guenon (monkey)");
   ("Mammal (bat)", "Seychelles sheath-tailed bat");
   ("Fungi", "Willow blister");
   ("Mammal (shrew)", "Nelson’s small-eared shrew");
   ("Reptile", "Jamaican iguana Jamaican rock iguana");
   ("Plant (orchid)", "Cayman Islands ghost orchid");
   ("Mammal (rhino)", "Sumatran rhino"); ("Bird", "Amsterdam albatross");
   ("Plant", "Wild yam"); ("Plant (tree)", ""); ("Plant (tree)", "");
   ("Amphibian (frog)", "Hula painted frog"); ("Plant", "");
   ("Plant (tree)", ""); ("Amphibian (frog)", "La Hotte glanded frog");
   ("Amphibian (frog)", "Macaya breast-spot frog");
   ("Plant", "Chilenito (cactus)"); ("Plant (tree)", "Coral tree");
   ("Plant (tree)", ""); ("Bird", "Spoon-billed sandpiper"); ("Plant", "");
   ("Bird", "Northern bald ibis");
   ("Plant", "(flowering plant in legume family)");
   ("Mollusc", "(type of gastropod)");
   ("Amphibian (frog)", "Table mountain ghost frog");
   ("Mollusc", "(type of land snail)"); ("Bird", "Liben lark");
   ("Plant (small tree)", ""); ("Fish", "Sakhalin taimen");
   ("Crustacean", "Singapore freshwater crab");
   ("Plant",
    "Belin vetchling (flowering plant related to Lathyrus odoratus)");
   ("Amphibian (frog)", "Archey’s frog");
   ("Amphibian (frog)", "Dusky gopher frog"); ("Bird", "Edwards’s pheasant");
   ("Plant", "(type of Magnolia tree)");
   ("Mollusc", "(type of freshwater mussel)"); ("Mollusc", "(snail)");
   ("Mammal (bat)", "Cuban greater funnel eared bat");
   ("Plant", "Attenborough’s pitcher plant");
   ("Mammal (primate)", "Hainan gibbon"); ("Amphibian", "Luristan newt");
   ("Insect (damselfly)", "Mulanje red damsel (damselfly)");
   ("Fish", "Pangasid catfish"); ("Insect (butterfly)", "(butterfly)");
   ("Mammal (cetacean)", "Vaquita (porpoise)");
   ("Plant (tree)", "Type of spruce tree"); ("Plant (tree)", "Qiaojia pine");
   ("Spider","Gooty tarantula, metallic tarantula, peacock parachute spider)");
   ("Bird", "Fatuhiva monarch"); ("Fish", "Common sawfish");
   ("Mammal (primate)", "Greater bamboo lemur");
   ("Mammal (primate)", "Silky sifaka");
   ("Reptile (tortoise)", "Geometric tortoise"); ("Mammal", "Saola");
   ("Plant", ""); ("Insect", "Beydaglari bush-cricket");
   ("Reptile (turtle)", "Red River giant softshell turtle");
   ("Mammal (rhino)", "Javan rhino");
   ("Mammal (primate)", "Tonkin snub-nosed monkey");
   ("Plant (orchid)", "West Australian underground orchid");
   ("Mammal (shrew)", "Boni giant sengi");
   ("Insect (damselfly)", "Cebu frill-wing (damselfly)"); ("Plant", "");
   ("Mammal", "Durrell’s vontsira (type of mongoose)");
   ("Mammal (rodent)", "Red crested tree rat");
   ("Fish", "Red-finned Blue-eye"); ("Fish (shark)", "Angel shark");
   ("Bird", "Chinese crested tern"); ("Fish", "Estuarine pipefish");
   ("Plant", "Suicide Palm Dimaka");
   ("Amphibian (frog)", "Bullock’s false toad");
   ("Mammal (rodent)", "Okinawa spiny rat"); ("Fish", "Somphongs’s rasbora");
   ("Fish", ""); ("Plant", "Forest coconut");
   ("Mammal", "Attenborough’s echidna")]

let speciesSorted = 
    species 
      |> Seq.countBy fst 
      |> Seq.sortBy (snd >> (~-))
      |> Seq.toList


let config =     
    let ip = IPAddress.Parse "0.0.0.0"
    { defaultConfig with 
        logger = Logging.Loggers.saneDefaultsFor Logging.LogLevel.Verbose
        bindings=[ HttpBinding.mk HTTP ip 8080us ] }

let text = 
    [ yield "<html><body><ul>"
      for (category,count) in speciesSorted do
         yield sprintf "<li>Category <b>%s</b>: <b>%d</b></li>" category count 
      yield "</ul></body></html>" ]
    |> String.concat "\n"

let angularHeader = """<head>
<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.2.0/css/bootstrap.min.css">
<script src="https://ajax.googleapis.com/ajax/libs/angularjs/1.2.26/angular.min.js"></script>
</head>"""

let animalsText = 
    [ yield """<html>"""
      yield angularHeader
      yield """ <body>"""
      yield """ <h1>Endangered Animals</h1>"""
      yield """  <table class="table table-striped">"""
      yield """   <thead><tr><th>Category</th><th>Count</th></tr></thead>"""
      yield """   <tbody>"""
      for (category,count) in speciesSorted do
         yield sprintf "<tr><td>%s</td><td>%d</td></tr>" category count 
      yield """   </tbody>"""
      yield """  </table>"""
      yield """ </body>""" 
      yield """</html>""" ]
    |> String.concat "\n"

let thingsText n = 
    [ yield """<html>"""
      yield angularHeader
      yield """ <body>"""
      yield """ <h1>Endangered Animals</h1>"""
      yield """  <table class="table table-striped">"""
      yield """   <thead><tr><th>Thing</th><th>Value</th></tr></thead>"""
      yield """   <tbody>"""
      for i in 1 .. n do
         yield sprintf "<tr><td>Thing %d</td><td>%d</td></tr>" i i  
      yield """   </tbody>"""
      yield """  </table>"""
      yield """ </body>""" 
      yield """</html>""" ]
    |> String.concat "\n"

let homePage = 
    [ yield """<html>"""
      yield angularHeader 
      yield """ <body>"""
      yield """ <h1>Sample Web App</h1>"""
      yield """  <table class="table table-striped">"""
      yield """   <thead><tr><th>Page</th><th>Link</th></tr></thead>"""
      yield """   <tbody>"""
      yield """      <tr><td>Endangered Animals</td><td><a href="/animals">Link to animals</a></td></tr>""" 
      yield """      <tr><td>Things</td><td><a href="/things/10">Link to things (10)</a></td></tr>""" 
      yield """      <tr><td>Things</td><td><a href="/things/100">Link to things (100)</a></td></tr>""" 
      yield """      <tr><td>API JSON</td><td><a href="/api/json/100">Link to result (100)</a></td></tr>"""
      yield """      <tr><td>API XML</td><td><a href="/api/xml/100">Link to result (100)</a></td></tr>"""
      yield """      <tr><td>API JSON</td><td><a href="/api/json/10">Link to result (10)</a></td></tr>"""
      yield """      <tr><td>API XML</td><td><a href="/api/xml/10">Link to result (10)</a></td></tr>"""
      yield """      <tr><td>Goodbye</td><td><a href="/goodbye">Link</a></td></tr>"""
      yield """   </tbody>"""
      yield """  </table>"""
      yield """ </body>""" 
      yield """</html>""" ]
    |> String.concat "\n"

printfn "starting web server..."

let jsonText n = 
    """
{"menu": {
  "id": "file",
  "value": "File",
  "popup": {
    "result": [
""" + String.concat "\n"
      [ for i in 1 .. n -> sprintf """{"value": "%d"},""" i ] + """
    ]
  }
}}""" 

let xmlText n = 
    """
<menu id="file" value="File">
  <popup>
""" + String.concat "\n"
      [ for i in 1 .. n -> sprintf """<menuitem value="%d" />""" i ] + """
    <menuitem value="Open" />
    <menuitem value="Close"  />
  </popup>
</menu>""" 

let xmlMime = Writers.setMimeType "application/xml"
let jsonMime = Writers.setMimeType "application/json"
let app = 
  choose
    [ GET >>= choose
                [ path "/" >>= OK homePage
                  path "/animals" >>= OK animalsText
                  pathScan "/things/%d" (fun n -> OK (thingsText n))
                  path "/api/json" >>= jsonMime >>= OK (jsonText 100)
                  pathScan "/api/json/%d" (fun n -> jsonMime >>= OK (jsonText n))
                  path "/api/xml" >>= xmlMime >>= OK (xmlText 100)
                  pathScan "/api/xml/%d" (fun n -> xmlMime >>= OK (xmlText n))
                  path "/goodbye" >>= OK "Good bye GET" ]
      POST >>= choose
                [ path "/hello" >>= OK "Hello POST"
                  path "/goodbye" >>= OK "Good bye POST" ] ]
    

startWebServer config app
printfn "exiting server..."


