import { InventoryMenu } from '../../menu/InventoryMenu'
import { InventoryItem } from '../../models/InventoryItem'
import { ServerEventHandler } from '../../event-handlers/ServerEventHandler'

namespace InventoryManager {
    let menu: InventoryMenu = new InventoryMenu()

    API.onKeyDown.connect((entity: any, args: System.Windows.Forms.KeyEventArgs) => {
        let player = API.getLocalPlayer()
        if (API.hasEntitySyncedData(player, 'DisableHotkeys')) {        
            return
        }
        if (args.KeyCode == Keys.I) {
            if (menu.isOpen) {
                menu.hide()
            } else {
                API.triggerServerEvent('ShowInventory')
            }            
        }
    })

    ServerEventHandler.getInstance().on('ShowInventory', showInventory)

    function showInventory(args: any[]) {
        let items = JSON.parse(args[0]) as InventoryItem[]
        let weight = args[1] as number
        menu.fillMenu(items, weight)
        menu.show()
    }
}