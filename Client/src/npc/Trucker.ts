import { ServerEventHandler } from '../event-handlers/ServerEventHandler'
import { TruckersMenu } from '../menu/work/TruckersMenu'
import { DeliveryContract } from '../models/DeliveryContract'

namespace Trucker {
    let menu: TruckersMenu = new TruckersMenu()

    ServerEventHandler.getInstance().on('ShowTruckersMenu', showMenu)
    ServerEventHandler.getInstance().on('HideTruckersMenu', hideMenu)

    function showMenu(args: any[]): void {
        let contracts = JSON.parse(args[0]) as DeliveryContract[]
        menu.fillMenu(contracts)
        menu.show()
    }

    function hideMenu(): void {
        menu.hide()
    }
}