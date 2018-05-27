import { ServerEventHandler } from '../../event-handlers/ServerEventHandler'
import { ShopMenu } from '../../menu/places/ShopMenu'

namespace FillingManager {
    let shopMenu: ShopMenu = new ShopMenu()

    ServerEventHandler.getInstance().on('ShowShopMenu', showMenu)
    ServerEventHandler.getInstance().on('HideShopMenu', hideMenu)

    function showMenu(args: any[]) {
        let district = args[0] as number
        shopMenu.fillMenu(district)
        shopMenu.show()
    }

    function hideMenu() {        
        shopMenu.hide()
    }
}