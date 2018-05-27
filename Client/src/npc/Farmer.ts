import { ServerEventHandler } from '../event-handlers/ServerEventHandler'
import { FarmMenu } from '../menu/work/FarmMenu'

namespace Farmer {  
    let menu: FarmMenu = new FarmMenu()
 
    ServerEventHandler.getInstance().on('ShowFarmMenu', showMenu)
    ServerEventHandler.getInstance().on('HideFarmMenu', hideMenu)

    function showMenu(args: any[]) {
        menu.show(args)
    }

    function hideMenu() {
        menu.hide()
    }
}