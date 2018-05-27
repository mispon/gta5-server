import { ServerEventHandler } from '../event-handlers/ServerEventHandler'
import { PoliceDepartmentMenu } from '../menu/work/PoliceDepartmentMenu'

namespace Policemans {       
    let menu: PoliceDepartmentMenu = new PoliceDepartmentMenu()      
    ServerEventHandler.getInstance().on('ShowSarahMenu', showMenu)
    ServerEventHandler.getInstance().on('HideSarahMenu', hideMenu)

    function showMenu(args: any[]): void {
        let hasQuest = args[0] as boolean
        menu.fillMenu(hasQuest)
        menu.show()
    }

    function hideMenu(): void {
        menu.hide()
    }
}