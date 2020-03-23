namespace FullSample

open XTargets.Elmish

module Shell =
    open Elmish
    open Avalonia.Controls
    open Avalonia.FuncUI.DSL
    open Avalonia.FuncUI
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Components.Hosts
    open Avalonia.FuncUI.Elmish

    type State =
        { noop: bool }

    let init = { noop = true }, Cmd.none

    let view (state: Lens<State>) =
        DockPanel.create
            [ DockPanel.children
                [ TabControl.create
                    [ TabControl.tabStripPlacement Dock.Top
                      TabControl.viewItems
                          [ TabItem.create
                              [ TabItem.header "Counter Sample"
                                TabItem.content (ViewBuilder.Create<Counter.Host>([])) ]
                            TabItem.create
                                [ TabItem.header "About"
                                  TabItem.content (ViewBuilder.Create<About.Host>([])) ] ] ] ] ]

    /// This is the main window of your application
    /// you can do all sort of useful things here like setting heights and widths
    /// as well as attaching your dev tools that can be super useful when developing with
    /// Avalonia
    type MainWindow() as this =
        inherit HostWindow()
        do
            base.Title <- "Full App"
            base.Width <- 800.0
            base.Height <- 600.0
            base.MinWidth <- 800.0
            base.MinHeight <- 600.0

            let view state dispatch =
                let getter = fun () -> state
                view (Lens.init getter dispatch)

            let update msg state =
                match msg with
                | Message(update, cmd) -> update state, cmd

            Elmish.Program.mkProgram (fun () -> init) update view
            |> Program.withHost this
#if DEBUG
            |> Program.withConsoleTrace
#endif
            |> Program.run
