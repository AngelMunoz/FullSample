namespace FullSample

module Counter =
    open Elmish
    open Avalonia.Controls
    open Avalonia.FuncUI.Components.Hosts
    open Avalonia.FuncUI.DSL
    open Avalonia.FuncUI.Elmish
    open Avalonia.Layout
    open XTargets.Elmish

    type State =
        { count: int }

    let init = { count = 0 }, Cmd.none

    let view (state: Lens<State>) =
        let updateIncrement (state: Lens<State>) = { state.Get with count = state.Get.count + 1 } |> state.Set

        let updateDecrement (state: Lens<State>) = { state.Get with count = state.Get.count - 1 } |> state.Set

        let updateReset (state: Lens<State>) = { state.Get with count = 0 } |> state.Set

        DockPanel.create
            [ DockPanel.children
                [ StackPanel.create
                    [ StackPanel.dock Dock.Bottom
                      StackPanel.margin 5.0
                      StackPanel.spacing 5.0
                      StackPanel.children
                          [ Button.create
                              [ Button.onClick (fun _ -> updateIncrement state)
                                Button.content "+"
                                Button.classes [ "plus" ] ]
                            Button.create
                                [ Button.onClick (fun _ -> updateDecrement state)
                                  Button.content "-"
                                  Button.classes [ "minus" ] ]
                            Button.create
                                [ Button.onClick (fun _ -> updateReset state)
                                  Button.content "reset" ] ] ]

                  TextBlock.create
                      [ TextBlock.dock Dock.Top
                        TextBlock.fontSize 48.0
                        TextBlock.verticalAlignment VerticalAlignment.Center
                        TextBlock.horizontalAlignment HorizontalAlignment.Center
                        TextBlock.text (string state.Get.count) ] ] ]

    type Host() as this =
        inherit HostControl()
        do

            let view state dispatch =
                let getter = fun () -> state
                view (Lens.init getter dispatch)

            let update msg state =
                match msg with
                | Message(update, cmd) -> update state, cmd

            Elmish.Program.mkProgram (fun () -> init) update view
            |> Program.withHost this
            |> Program.run
