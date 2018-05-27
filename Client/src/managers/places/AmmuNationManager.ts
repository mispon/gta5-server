import { AmmuNationMenu } from "../../menu/places/AmmuNationMenu";
import { WeaponGood } from '../../models/WeaponGood'
import { ServerEventHandler } from '../../event-handlers/ServerEventHandler'

namespace AmmuNationManager {
    let menu: AmmuNationMenu = new AmmuNationMenu

    ServerEventHandler.getInstance().on('ShowAmmuNationMenu', showMenu)
    ServerEventHandler.getInstance().on('HideAmmuNationMenu', hideMenu)

    function showMenu(args: any[]) {
        let weapons = JSON.parse(args[0]) as WeaponGood[]
        let ammo = JSON.parse(args[1]) as WeaponGood[]
        let district = args[2] as number
        menu.fillMenu(weapons, ammo, district)
        menu.show()
    }

    function hideMenu() {
        menu.hide()
    }
}