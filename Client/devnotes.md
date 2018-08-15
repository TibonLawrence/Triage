# Development notes
## Branch yak-shaving
### json-server
After getting the basic app up and running, I followed directions from [Elm Tutorial](https://www.elm-tutorial.org/en/) on [setting up a fake backend](https://www.elm-tutorial.org/en/04-starting/02-backend.html) with [json-server](https://github.com/typicode/json-server). The tutorial pinned v0.9.5, but I used the latest, which is 0.14.0. For some reason, I can't get the root screen of json-server to show anymore, whether or not I use the old version.

### Bootstrap
I know Bootstrap is tired, but I know it, so I'm using [elm-bootstrap](http://elm-bootstrap.info/). The library uses only the css, but `bootstrap-css` has individual css files that I don't want to pick through. I pulled down the regular Bootstrap package.

One concern I have is that the [dropdowns](http://elm-bootstrap.info/dropdown) require some coding and state management.

### Font Awesome
I'll probably want some icons, so I added Font Awesome.

### Department Filtering
Got it to work with departments, but have to find a way to cancel the filter with Nothing. Considering adding All to departments, but I'm not sure that's the model I want. It seems to work, so I'll use it for now.

### Dropdowns
I got a mockup of the UI going with hardcoded data and controls that do nothing. Because each dropdown needs to have its own subscription, I'm going have to write code for that. Bill Peregoy has an article on [dynamically creating subscriptions](https://becoming-functional.com/dynamically-creating-elm-subscriptions-3b41e2dc0a30) that I'll be using as a model. I didn't find anything else with a quick search.

State of UI elements doesn't belong in the regular model. Where can I assign this information?

### Moving to Bulma
It looks like dynamically creating dropdowns is impossible with Elm-Bootstrap. The Dropdown's toggleMsg property takes only a Msg without a payload, and I was unable to make a message with a payload.

Elm-MDL is dropped, and Elm-MDC is not yet released to the Elm package manager.

[Elm-Bulma](https://github.com/surprisetalk/elm-bulma/) leverages the [Bulma](https://bulma.io) CSS library, which looks clean and simple. The [dropdown component](https://github.com/surprisetalk/elm-bulma/blob/master/src/Bulma/Components.elm) looks better than the Elm-Bootstrap version. It uses regular `Html Msg` and its semantics look like drop-ins for Elm Html. I think it might be a win....

But it wasn't. I had some trouble with the API, but the maintaner sent me an email, so I might try again.

### Back to Bootsrap and using a Dict?
It occurred to me that I might be able to leverage partial application and a Dict to store dropdown state. For now, though, I'm using a list `(CaseNumber, Dropdown.State)` tuples. It works.

## To do
- petitioner and respondent dropdowns
- interpreter filters
- model status
- add notes to each case
- add collapsible div for notes
- add collapsible div for events
- add delete to collapsible div instead of dropdown box
- model case information
- model elements
- json encoders and decoders
- consume cms data
- consume triage data
- consume triage websockets
- CRUD for users
- CRUD for dispos
- CRUD for events
- export to CSV
- aggregate reports of dispos by month
- filter events by day
- navigation