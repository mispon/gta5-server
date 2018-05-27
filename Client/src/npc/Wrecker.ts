import { ServerEventHandler } from '../event-handlers/ServerEventHandler'
import { ParkingFineMenu } from '../menu/work/ParkingFineMenu';

namespace Wrecker {       
    let menu: ParkingFineMenu = new ParkingFineMenu()

    ServerEventHandler.getInstance().on('ShowParkingFineMenu', showMenu)
    ServerEventHandler.getInstance().on('HideParkingFineMenu', hideMenu)

    function showMenu(args: any[]) {
        let vehicles = JSON.parse(args[0])
        menu.fillMenu(vehicles)
        menu.show()
    }

    function hideMenu() {
        menu.hide()
    }
}