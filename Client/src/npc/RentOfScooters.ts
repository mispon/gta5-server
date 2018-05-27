import { ServerEventHandler } from '../event-handlers/ServerEventHandler'
import { RentOfScootersMenu } from '../menu/places/RentOfScootersMenu'

namespace RentOfScooters {       
    let menu: RentOfScootersMenu = new RentOfScootersMenu()  

    ServerEventHandler.getInstance().on('ShowScootersMenu', showMenu)
    ServerEventHandler.getInstance().on('HideScootersMenu', hideMenu)

    function showMenu(args: any[]): void {
        menu.show(args)
    }

    function hideMenu(): void {
        menu.hide()
    }
}