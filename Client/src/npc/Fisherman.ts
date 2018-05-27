import { ServerEventHandler } from '../event-handlers/ServerEventHandler'
import { FishermansMenu } from '../menu/work/FishermansMenu'

namespace Fishermans {  
    let menu: FishermansMenu = new FishermansMenu()
 
    ServerEventHandler.getInstance().on('ShowFishermansMenu', showMenu)
    ServerEventHandler.getInstance().on('HideFishermansMenu', hideMenu)

    function showMenu(args: any[]) {
        menu.show(args)
    }

    function hideMenu() {
        menu.hide()
    }
}