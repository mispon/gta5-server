import {PlayerActionsMenu} from '../../menu/PlayerActionsMenu'
import {ServerEventHandler} from '../../event-handlers/ServerEventHandler'

namespace PlayerActionManager {
    let menu: PlayerActionsMenu = new PlayerActionsMenu()

    ServerEventHandler.getInstance().on('ShowPlayerActionMenu', showMenu)
    ServerEventHandler.getInstance().on('HidePlayerActionMenu', hideMenu)

    function showMenu(args: any[]) {        
        let positions = JSON.parse(args[0]) as Vector3[]        
        menu.fillHouseMenu(positions)
        menu.show()
    }

    function hideMenu() {
        menu.hide()
    }
}