import { ServerEventHandler } from '../event-handlers/ServerEventHandler'
import { ForkliftMenu } from '../menu/work/ForkliftMenu'

namespace Forklifts {  
    let menu: ForkliftMenu = new ForkliftMenu()
 
    ServerEventHandler.getInstance().on('ShowForkliftMenu', showMenu)
    ServerEventHandler.getInstance().on('HideForkliftMenu', hideMenu)

    function showMenu(args: any[]) {
        menu.show(args)
    }

    function hideMenu() {
        menu.hide()
    }
}