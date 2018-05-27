import { ServerEventHandler } from '../event-handlers/ServerEventHandler'
import { TaxiDriverMenu } from '../menu/work/TaxiDriverMenu'

namespace TaxiDrivers {       
    let menu: TaxiDriverMenu = new TaxiDriverMenu()      
    ServerEventHandler.getInstance().on('ShowSiemonMenu', showMenu)
    ServerEventHandler.getInstance().on('HideSiemonMenu', hideMenu)

    function showMenu(): void {
        menu.show()
    }

    function hideMenu(): void {
        menu.hide()
    }
}