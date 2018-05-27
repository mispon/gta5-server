import { ServerEventHandler } from '../event-handlers/ServerEventHandler'
import { BusDriverMenu } from '../menu/work/BusDriverMenu'

namespace BusDrivers {  
    let menu: BusDriverMenu = new BusDriverMenu()
    
    ServerEventHandler.getInstance().on('ShowOneilMenu', showMenu)
    ServerEventHandler.getInstance().on('HideOneilMenu', hideMenu)

    function showMenu(args: any[]) {
        menu.show(args)
    }

    function hideMenu() {
        menu.hide()
    }
}