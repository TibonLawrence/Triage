module Webapp

open Giraffe
open GetHandlersV1
open PostHandlersV1
open AuthHandlers
open DeleteHandlers
                                                    
// ---------------------------------
// Web app
// ---------------------------------

let webApp: HttpHandler =
    //mustBeAuthenticated >=> choose [
    choose [
        GET >=>
            choose [
                route "/" >=> htmlFile @"index.html"
                subRoute "/api"
                    (choose [
                        subRoute "/v1"
                            (choose [
                                routef "/notesAndEvents/%s" getNotesAndEventsHandler
                                routef "/events/%s" getEventsHandler
                                routef "/notes/%s" getNotesHandler
                            ])
                        ])
            ]
        POST >=>
            choose [
                route "/api/v1/event" >=> postEventHandler
                route "/api/v1/note" >=> postNoteHandler
            ]
        //mustBeAuthenticated >=>
            //DELETE >=>
            //    choose [
            //        routef "/api/v2/event/%i" deleteEventHandler
            //        routef "/api/v2/note/%i" deleteNoteHandler
            //    ]
        setStatusCode 404 >=> text "Not Found" ] 

