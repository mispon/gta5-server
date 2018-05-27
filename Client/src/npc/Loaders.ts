import { ServerEventHandler } from '../event-handlers/ServerEventHandler'
import { LoaderMenu } from '../menu/work/LoaderMenu'

namespace Loaders {  
    let menu: LoaderMenu = new LoaderMenu()
    
    ServerEventHandler.getInstance().on('ShowLoaderMenu', showMenu)
    ServerEventHandler.getInstance().on('HideLoaderMenu', hideMenu)

    function showMenu(args: any[]) {
        menu.show(args)
    }

    function hideMenu() {
        menu.hide()
    }
}