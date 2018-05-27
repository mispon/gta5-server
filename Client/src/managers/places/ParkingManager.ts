import { ParkingMenu } from '../../menu/places/ParkingMenu'
import { ServerEventHandler } from '../../event-handlers/ServerEventHandler'

namespace CarShowroomManager {
    let parkingMenu: ParkingMenu = new ParkingMenu()

    ServerEventHandler.getInstance().on('ShowParkingMenu', showParkingMenu)
    ServerEventHandler.getInstance().on('HideParkingMenu', hideParkingMenu)

    function showParkingMenu(args: any[]) {
        let vehicles = JSON.parse(args[0])
        parkingMenu.fillMenu(vehicles)
        parkingMenu.show()
    }

    function hideParkingMenu() {
        parkingMenu.hide()
    }
}