import { ServerEventHandler } from '../../event-handlers/ServerEventHandler'
import { NurseMenu } from '../../menu/places/NurseMenu'

namespace HospitalManager {
    let menu: NurseMenu = new NurseMenu()

    ServerEventHandler.getInstance().on('ShowNurseMenu', showMenu)
    ServerEventHandler.getInstance().on('HideNurseMenu', hideMenu)

    function showMenu(args: any[]) {        
        menu.show(args)
    }

    function hideMenu() {        
        menu.hide()
    }
}