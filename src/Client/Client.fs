module Client

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Notation
open WebSharper.JQuery


module Bulma =

    let DrawerShown = Var.Create false

    let IsSiderbarOpen = Var.Create false

    let OpenedMenu = Var.Create ""

    let MobileOpen = Var.Create false

    [<JavaScriptExport>]
    let ToggleDrawer() = DrawerShown.Update not

    let HookDrawer() =
        DrawerShown.View |> View.Sink (fun shown ->
            JS.Document.QuerySelectorAll(".drawer-backdrop, .lhs-drawer").ForEach(
                (fun (node, _, _, _) ->
                    let node = node :?> Dom.Element
                    "shown"
                    |> if shown then node.ClassList.Add else node.ClassList.Remove
                ),
                JS.Undefined
            )
        )

    [<JavaScriptExport>]
    let NavBarToggle(id: string) = 
        JQuery.Of(id).ToggleClass "is-active"

    [<JavaScriptExport>]
    let InitNavbar(id : string) = 
        let navBar = JS.Document.QuerySelector(id)

        navBar.SetAttribute("scrolled", "false")

        let updateScroll (nav:Dom.Element)  = 
            
            let scrollValue = JS.Window.ScrollY
            
            if scrollValue >= nav.ClientHeight then
                navBar.SetAttribute("scrolled", "true")
            else
                navBar.SetAttribute("scrolled", "false")
            navBar.SetAttribute("searchExpanded", "false")
            ()

        //navBar.("scroll", 
        //        fun e _ -> (updateScroll e)
        //    ).Ignore

        JS.Undefined


    [<JavaScriptExport>]
    let OpenMobileMenu() = MobileOpen.Update not

    [<JavaScriptExport>]
    let OpenSidebar() = IsSiderbarOpen.Set true
        
    [<JavaScriptExport>]
    let CloseSidebar() = IsSiderbarOpen.Set false

    [<JavaScriptExport>]
    let OpenSidebarMenu( targetMenu )  =                 
        if OpenedMenu.Get() = targetMenu then OpenedMenu.Set ""
        else OpenedMenu.Set targetMenu
          

    let initPageLoader() =
        JS.Window.AddEventListener("load", fun () -> ())



[<SPAEntryPoint>]
let Main() =
    Bulma.HookDrawer()
    Bulma.InitNavbar("navbar-main")
    Bulma.InitNavbar("navbar-clone")
    

[<assembly:JavaScript>]
do ()
