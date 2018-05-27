/// <reference path='../../../types-gt-mp/index.d.ts' />

import { ServerEventHandler } from '../../event-handlers/ServerEventHandler'
import { ClanLeaderMenu } from '../../menu/clan/ClanLeaderMenu';

namespace ClanManager {
    let menu = new ClanLeaderMenu()

    ServerEventHandler.getInstance().on('ShowClanLeaderMenu', showMenu)
    ServerEventHandler.getInstance().on('HideClanLeaderMenu', hideMenu)

    function showMenu(args: any[]) {
        let clanId = args[0] as number
        let missionVotes = args[1] as number
        let authority = args[2] as number
        menu.fillMenu(clanId, missionVotes, authority)
        menu.show()
    }

    function hideMenu(args: any[]) {
        menu.hide()
    }
}