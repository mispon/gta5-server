import {VehicleActionsMenu} from '../../menu/VehicleActionsMenu'
import {ServerEventHandler} from '../../event-handlers/ServerEventHandler'

namespace VehicleActionsManager {
    let menu: VehicleActionsMenu = new VehicleActionsMenu()

    ServerEventHandler.getInstance().on('ShowVehicleActionMenu', showMenu)
    ServerEventHandler.getInstance().on('HideVehicleActionMenu', hideMenu)

    function showMenu(args: any[]) {
        menu.fillMenu(args)
        menu.show()
    }

    function hideMenu() {
        menu.hide()
    }
}