/// <reference path='../../../types-gt-mp/index.d.ts' />

import { ServerEventHandler } from '../../event-handlers/ServerEventHandler'
import { MainClanMenu } from '../../menu/clan/MainClanMenu'
import { VansGarageMenu } from '../../menu/clan/VansGarageMenu'

namespace ClanManager {
    let clanMenu = new MainClanMenu()
    let vansMenu = new VansGarageMenu()

    ServerEventHandler.getInstance().on('ShowClanMenu', showClanMenu)
    ServerEventHandler.getInstance().on('HideClanMenu', hideClanMenu)
    ServerEventHandler.getInstance().on('ShowClanVansMenu', showVansGarageMenu)
    ServerEventHandler.getInstance().on('HideClanVansMenu', hideVansGarageMenu)

    function showClanMenu(args: any[]) {
        let clanId = args[0] as number
        clanMenu.fillMenu(clanId)
        clanMenu.show()
    }

    function hideClanMenu(args: any[]) {
        clanMenu.hide()
    }

    function showVansGarageMenu(args: any[]) {
        let position = args[0] as Vector3
        let rotation = args[1] as Vector3
        vansMenu.fillMenu(position, rotation)
        vansMenu.show()
    }

    function hideVansGarageMenu() {
        vansMenu.hide()
    }
}