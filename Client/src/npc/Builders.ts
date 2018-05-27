import { ServerEventHandler } from '../event-handlers/ServerEventHandler'
import { BuilderMenu } from '../menu/work/BuilderMenu'

namespace Builders {  
    let menu: BuilderMenu = new BuilderMenu()
 
    ServerEventHandler.getInstance().on('ShowBuilderMenu', showMenu)
    ServerEventHandler.getInstance().on('HideBuilderMenu', hideMenu)

    function showMenu(args: any[]) {
        menu.show(args)
    }

    function hideMenu() {
        menu.hide()
    }
}