/// <reference path='../../../types-gt-mp/index.d.ts' />

import { TuningMenu } from '../../menu/places/TuningMenu';
import { TuningInfo } from '../../models/TuningInfo'
import { ServerEventHandler } from '../../event-handlers/ServerEventHandler'

namespace TuningManager {
    let menu: TuningMenu = new TuningMenu()

    ServerEventHandler.getInstance().on('ShowTuningMenu', showMenu)
    ServerEventHandler.getInstance().on('HideTuningMenu', hideMenu)

    API.onKeyDown.connect((sender, arg) => {
        let player = API.getLocalPlayer()
        if (!API.hasEntitySyncedData(player, 'InTuningGarage')) {
            return
        }
        if (arg.KeyCode == Keys.O) {
            if (menu.isOpen) {
                menu.hide()
            }
            menu.show()
        }
    })

    function showMenu(args: any[]) {
        let tuningInfo = JSON.parse(args[0]) as TuningInfo[];
        let hasNeon = args[1] as boolean
        menu.fillMenu(tuningInfo, hasNeon)
        menu.show()
    }

    function hideMenu() {
        menu.hide()
    }
}