import { ParachuteMenu } from '../../menu/places/ParachuteMenu'
import { ServerEventHandler } from '../../event-handlers/ServerEventHandler'

namespace ParachuteManager {
    let menu: ParachuteMenu = new ParachuteMenu()

    ServerEventHandler.getInstance().on('ShowParachuteMenu', showMenu)
    ServerEventHandler.getInstance().on('HideParachuteMenu', hideMenu)

    function showMenu() {
        menu.show()
    }

    function hideMenu() {
        menu.hide()
    }
}