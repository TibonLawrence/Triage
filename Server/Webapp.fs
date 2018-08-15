module Webapp

open Giraffe
open GetHandlersV1
open AuthHandlers
open DeleteHandlers
                                                    
// ---------------------------------
// Web app
// ---------------------------------

let webApp: HttpHandler =
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
        mustBeAuthenticated >=>
            POST >=>
                choose [
                    subRoute "/api"
                        (choose [
                            subRoute "/v2"
                                (choose [
                                    route "/event" >=> PostHandlersV2.postEventHandler
                                    route "/note" >=> PostHandlersV2.postNoteHandler
                                ])
                            ])
                        ]
                DELETE >=>
                    subRoute "/api"
                        (choose [
                            subRoute "/v2"
                                (choose [
                                    routef "/event/%i" deleteEventHandler
                                    routef "/note/%i" deleteNoteHandler
                                ])
                            ])
        setStatusCode 404 >=> text "Not Found" 
    ]

