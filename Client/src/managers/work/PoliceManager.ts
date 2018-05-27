import { PoliceActionsMenu } from '../../menu/work/PoliceActionsMenu'
import { ServerEventHandler } from '../../event-handlers/ServerEventHandler'

namespace PoliceManager {
    let policeActions: PoliceActionsMenu = new PoliceActionsMenu()

    ServerEventHandler.getInstance().on('TriggerPoliceActionMenu', triggerMenu)
    ServerEventHandler.getInstance().on('HidePolicemanMenu', hideMenu)

    API.onKeyDown.connect((sender, args) => {
        let player = API.getLocalPlayer()
        if (!API.hasEntitySyncedData(player, 'HasPoliceActions') || API.hasEntitySyncedData(player, 'DisableHotkeys')) {
            return
        }
        if (args.KeyCode == Keys.E) {
            API.triggerServerEvent('ArrestPlayer')
        }
        if (args.KeyCode == Keys.X) {            
            API.triggerServerEvent('GetPoliceMenu', policeActions.isOpen)
        }
    })

    function triggerMenu(args: any[]) {
        policeActions.triggerVisible(args)
    }

    function hideMenu() {
        policeActions.hide()
    }
}
