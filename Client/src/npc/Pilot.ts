import { ServerEventHandler } from '../event-handlers/ServerEventHandler'
import { PilotsMenu } from '../menu/work/PilotsMenu'
import { DeliveryContract } from '../models/DeliveryContract'

namespace Trucker {
    let menu: PilotsMenu = new PilotsMenu()

    ServerEventHandler.getInstance().on('ShowPilotMenu', showMenu)
    ServerEventHandler.getInstance().on('HidePilotMenu', hideMenu)

    function showMenu(args: any[]): void {
        let contracts = JSON.parse(args[0]) as DeliveryContract[]
        menu.fillMenu(contracts)
        menu.show()
    }

    function hideMenu(): void {
        menu.hide()
    }
}