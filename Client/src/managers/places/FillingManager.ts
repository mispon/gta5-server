import { ServerEventHandler } from '../../event-handlers/ServerEventHandler'
import { FillingMenu } from '../../menu/places/FillingMenu'

namespace FillingManager {
    let menu: FillingMenu = new FillingMenu()

    ServerEventHandler.getInstance().on('ShowFillingMenu', showMenu)
    ServerEventHandler.getInstance().on('HideFillingMenu', hideMenu)

    function showMenu(args: any[]) {
        let stationId = args[0] as number
        let district = args[1] as number
        menu.fillMenu(stationId, district)
        menu.show()
    }

    function hideMenu() {        
        menu.hide()
    }
}