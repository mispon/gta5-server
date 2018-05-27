import { ServerEventHandler } from '../event-handlers/ServerEventHandler'
import { StreetFightsMenu } from '../menu/places/StreetFightsMenu'

namespace Fighters {       
    let menu: StreetFightsMenu = new StreetFightsMenu()      
    ServerEventHandler.getInstance().on('ShowFightMenu', showMenu)
    ServerEventHandler.getInstance().on('HideFightMenu', hideMenu)

    function showMenu(args: any[]): void {
        let members = args[0] as number
        menu.fillMenu(members)
        menu.show()
    }

    function hideMenu(): void {
        menu.hide()
    }
}