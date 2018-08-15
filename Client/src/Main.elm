module Main exposing (..)

import Bootstrap.Button as Button
import Bootstrap.Dropdown as Dropdown
import Bootstrap.Form.Input as Input
import Bootstrap.Navbar as Navbar
import Bootstrap.Table as Table
import Bootstrap.Utilities.Spacing as Spacing
import Html exposing (Html, text, div, span, p, ul, li, h1)
import Html.Attributes exposing (class, id, src, type_, placeholder, href, attribute)
import Html.Events exposing (onClick)


---- MODEL ----


type alias Model =
    { departmentDropdownState : Dropdown.State
    , departmentFilter : Department
    , departments : List Department
    , interpreterDropdownState : Dropdown.State
    , matters : List Matter
    , mattersDropdownStates : MatterDropdownStates
    , navbarState : Navbar.State
    , statusDropdownState : Dropdown.State
    }


type Department
    = All
    | F201
    | F301
    | F401
    | F402
    | F501
    | F502


type alias Matter =
    { caseNumber : CaseNumber
    , department : Department
    , interpreter : Language
    , petitioner : FullName
    , respondent : FullName
    }


type alias Language =
    String


type alias CaseNumber =
    String


type alias FullName =
    String


type alias MatterDropdownStates =
    List ( CaseNumber, Dropdown.State )


init : ( Model, Cmd Msg )
init =
    let
        ( navbarState, navbarCmd ) =
            Navbar.initialState NavbarMsg

        matters =
            [ { department = F201, interpreter = "None", caseNumber = "RIF1700174", petitioner = "Beatrice Phelps", respondent = "Shannon Terry" }
            , { department = F201, interpreter = "None", caseNumber = "RIF1800505", petitioner = "Bethany Oliver", respondent = "Al Bailey" }
            , { department = F201, interpreter = "None", caseNumber = "RIF1700371", petitioner = "Harry Mendoza", respondent = "Lynette Perry" }
            , { department = F301, interpreter = "None", caseNumber = "RIF1800524", petitioner = "Teresa Stevens", respondent = "Daniel Bishara" }
            , { department = F301, interpreter = "None", caseNumber = "RIF1700867", petitioner = "Kelly Clayton", respondent = "Julian Roberson" }
            , { department = F401, interpreter = "None", caseNumber = "RIF1700273", petitioner = "Leona Jackson", respondent = "Hazel Wood" }
            , { department = F401, interpreter = "None", caseNumber = "RIF1800578", petitioner = "Ramona Hudson", respondent = "Terry Arnold" }
            , { department = F401, interpreter = "None", caseNumber = "RIF1800475", petitioner = "Faith Reyes", respondent = "Herman Barber" }
            , { department = F402, interpreter = "None", caseNumber = "RIF1700123", petitioner = "Terence Chavez", respondent = "Jeannette Mitchell" }
            , { department = F402, interpreter = "None", caseNumber = "RIF1800836", petitioner = "Tracy Fields", respondent = "Kent Osborne" }
            , { department = F402, interpreter = "None", caseNumber = "RIF1800183", petitioner = "Daniel Sherman", respondent = "Pam Moss" }
            , { department = F501, interpreter = "None", caseNumber = "RIF1700678", petitioner = "Javier Roy", respondent = "Clifford Sutton" }
            , { department = F501, interpreter = "None", caseNumber = "RIF1600977", petitioner = "Colleen Padilla", respondent = "Israel Miles" }
            , { department = F501, interpreter = "None", caseNumber = "RIF1700081", petitioner = "Cynthia Collier", respondent = "Jesse Moody" }
            , { department = F502, interpreter = "None", caseNumber = "RIF1600118", petitioner = "Robert Ruiz", respondent = "Margaret Fields" }
            , { department = F502, interpreter = "None", caseNumber = "RIF1800229", petitioner = "Jodi Flores", respondent = "Wendell Moody" }
            , { department = F502, interpreter = "None", caseNumber = "RIF1800181", petitioner = "Moses Moss", respondent = "Carrie Matthews" }
            ]

        mattersDropdownStates =
            List.map (\m -> ( m.caseNumber, Dropdown.initialState )) matters
    in
        ( { departmentDropdownState = Dropdown.initialState
          , departmentFilter = All
          , departments = [ All, F201, F301, F401, F402, F501, F502 ]
          , interpreterDropdownState = Dropdown.initialState
          , matters = matters
          , mattersDropdownStates = mattersDropdownStates
          , navbarState = navbarState
          , statusDropdownState = Dropdown.initialState
          }
        , navbarCmd
        )



---- UPDATE ----


type Msg
    = NoOp
    | ToggleDropdown String Dropdown.State
    | DepartmentDropdownToggle Dropdown.State
    | InterpreterDropdownToggle Dropdown.State
    | StatusDropdownToggle Dropdown.State
    | FilterDepartment Department
    | FilterInterpreter String
    | FilterStatus String
    | NavbarMsg Navbar.State


update : Msg -> Model -> ( Model, Cmd Msg )
update msg model =
    case msg of
        NoOp ->
            ( model, Cmd.none )

        ToggleDropdown caseNumber state ->
            let
                mattersDropdownStates =
                    model.mattersDropdownStates
                        |> List.map
                            (\( cn, s ) ->
                                if cn == caseNumber then
                                    ( caseNumber, state )
                                else
                                    ( cn, s )
                            )
            in
                ( { model | mattersDropdownStates = mattersDropdownStates }, Cmd.none )

        DepartmentDropdownToggle state ->
            ( { model | departmentDropdownState = state }
            , Cmd.none
            )

        InterpreterDropdownToggle state ->
            ( { model | interpreterDropdownState = state }
            , Cmd.none
            )

        StatusDropdownToggle state ->
            ( { model | statusDropdownState = state }
            , Cmd.none
            )

        FilterDepartment department ->
            ( { model | departmentFilter = department }
            , Cmd.none
            )

        FilterInterpreter language ->
            let
                _ =
                    Debug.log "filter language" language
            in
                ( model, Cmd.none )

        FilterStatus status ->
            let
                _ =
                    Debug.log "filter status" status
            in
                ( model, Cmd.none )

        NavbarMsg state ->
            ( { model | navbarState = state }, Cmd.none )



---- VIEW ----


view : Model -> Html Msg
view model =
    div []
        [ viewNavbar model
        , div [ attribute "role" "main", class "container" ]
            [ div [ class "starter-template" ]
                [ h1 [] [ text "Triage Elm" ]
                , p [ class "lead" ]
                    [ text "Making Triage Functional"
                    ]
                ]
            , viewActionTable model
            ]
        ]


viewNavbar : Model -> Html Msg
viewNavbar model =
    Navbar.config NavbarMsg
        |> Navbar.withAnimation
        |> Navbar.dark
        |> Navbar.brand [ href "#" ] [ text "Triage" ]
        |> Navbar.items
            [ Navbar.itemLink [ href "#home" ] [ text "Home" ]
            , Navbar.itemLink [ href "#link" ] [ text "Link" ]
            , Navbar.dropdown
                { id = "navbarDropdown"
                , toggle = Navbar.dropdownToggle [] [ text "Nav" ]
                , items =
                    [ Navbar.dropdownHeader [ text "Heading" ]
                    , Navbar.dropdownItem
                        [ href "#drop1" ]
                        [ text "Drop item 1" ]
                    , Navbar.dropdownItem
                        [ href "#drop2" ]
                        [ text "Drop item 2" ]
                    , Navbar.dropdownDivider
                    , Navbar.dropdownItem
                        [ href "#drop3" ]
                        [ text "Drop item 3" ]
                    ]
                }
            ]
        |> Navbar.customItems
            [ Navbar.formItem []
                [ Input.text [ Input.attrs [ placeholder "search for something" ] ]
                , Button.button
                    [ Button.outlineLight
                    , Button.attrs [ Spacing.ml2Sm ]
                    ]
                    [ text "Search" ]
                ]
            ]
        |> Navbar.view model.navbarState


viewActionTable : Model -> Html Msg
viewActionTable model =
    let
        matters =
            case model.departmentFilter of
                All ->
                    model.matters

                department ->
                    List.filter (\m -> m.department == department) model.matters
    in
        Table.table
            { options = [ Table.hover ]
            , thead =
                Table.simpleThead
                    [ Table.th [] [ departmentDropdown model ]
                    , Table.th [] [ interpreterDropdown model ]
                    , Table.th [] [ text "Case Number" ]
                    , Table.th [] [ text "Petitioner" ]
                    , Table.th [] [ text "Respondent" ]
                    , Table.th [] [ statusDropdown model ]
                    ]
            , tbody =
                Table.tbody [] (List.map (viewActionRow model) model.matters)
            }


viewActionRow : Model -> Matter -> Table.Row Msg
viewActionRow model matter =
    Table.tr []
        [ Table.td [] [ text (toString matter.department) ]
        , Table.td [] [ text matter.interpreter ]
        , Table.td [] [ text matter.caseNumber ]
        , Table.td [] [ text matter.petitioner ]
        , Table.td [] [ text matter.respondent ]
        , Table.td [] [ actionDropdown model matter ]
        ]


actionDropdownState : Model -> Matter -> Dropdown.State
actionDropdownState model matter =
    model.mattersDropdownStates
        |> List.filter (\( caseNumber, state ) -> caseNumber == matter.caseNumber)
        |> List.head
        |> Maybe.map Tuple.second
        |> Maybe.withDefault Dropdown.initialState


actionDropdown : Model -> Matter -> Html Msg
actionDropdown model matter =
    Dropdown.dropdown
        (actionDropdownState model matter)
        { options = []
        , toggleMsg = ToggleDropdown matter.caseNumber
        , toggleButton =
            Dropdown.toggle [ Button.light ] [ text "Choose Action" ]
        , items =
            [ Dropdown.buttonItem [ onClick (NoOp) ] [ text "Action1" ]
            , Dropdown.buttonItem [ onClick (NoOp) ] [ text "Action2" ]
            , Dropdown.buttonItem [ onClick (NoOp) ] [ text "Action3" ]
            , Dropdown.buttonItem [ onClick (NoOp) ] [ text "Action4" ]
            ]
        }


departmentDropdown : Model -> Html Msg
departmentDropdown model =
    let
        label =
            "Department: " ++ toString (model.departmentFilter)
    in
        Dropdown.dropdown
            model.departmentDropdownState
            { options = []
            , toggleMsg = DepartmentDropdownToggle
            , toggleButton = Dropdown.toggle [ Button.light ] [ text label ]
            , items =
                List.map (\d -> Dropdown.buttonItem [ onClick (FilterDepartment d) ] [ text (toString d) ]) model.departments
            }


interpreterDropdown : Model -> Html Msg
interpreterDropdown model =
    Dropdown.dropdown
        model.interpreterDropdownState
        { options = []
        , toggleMsg = InterpreterDropdownToggle
        , toggleButton = Dropdown.toggle [ Button.light ] [ text "Interpreter" ]
        , items =
            [ Dropdown.buttonItem [ onClick (FilterInterpreter "Spanish") ] [ text "Spanish" ]
            , Dropdown.buttonItem [ onClick (FilterInterpreter "None") ] [ text "None" ]
            ]
        }


statusDropdown : Model -> Html Msg
statusDropdown model =
    -- needs dropdownstate, buttontext, buttoncolor, menuitems
    Dropdown.dropdown
        model.statusDropdownState
        { options = []
        , toggleMsg = StatusDropdownToggle
        , toggleButton =
            Dropdown.toggle [ Button.light ] [ text "Filter" ]
        , items =
            [ Dropdown.buttonItem [ onClick (FilterStatus "Action1") ] [ text "Action1" ]
            , Dropdown.buttonItem [ onClick (FilterStatus "Action2") ] [ text "Action2" ]
            , Dropdown.buttonItem [ onClick (FilterStatus "Action3") ] [ text "Action3" ]
            , Dropdown.buttonItem [ onClick (FilterStatus "Action4") ] [ text "Action4" ]
            ]
        }



---- PROGRAM ----


main : Program Never Model Msg
main =
    Html.program
        { view = view
        , init = init
        , update = update
        , subscriptions = subscriptions
        }


mattersDropdownSubscriptions : Model -> List (Sub Msg)
mattersDropdownSubscriptions model =
    model.mattersDropdownStates
        |> List.map (\( caseNumber, state ) -> Dropdown.subscriptions state (ToggleDropdown caseNumber))


subscriptions : Model -> Sub Msg
subscriptions model =
    Sub.batch
        (mattersDropdownSubscriptions model
            ++ [ Navbar.subscriptions model.navbarState NavbarMsg
               , Dropdown.subscriptions model.departmentDropdownState DepartmentDropdownToggle
               , Dropdown.subscriptions model.interpreterDropdownState InterpreterDropdownToggle
               , Dropdown.subscriptions model.statusDropdownState StatusDropdownToggle
               ]
        )
