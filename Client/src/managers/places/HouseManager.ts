import { HouseMenu } from '../../menu/places/HouseMenu'
import { HouseStorageMenu } from '../../menu/places/HouseStorageMenu'
import { InventoryItem } from '../../models/InventoryItem'
import { ServerEventHandler } from '../../event-handlers/ServerEventHandler'

namespace HouseManager {
    let houseMenu: HouseMenu = new HouseMenu()
    let houseStorageMenu: HouseStorageMenu = new HouseStorageMenu()

    ServerEventHandler.getInstance().on('ShowHouseMenu', showHouseMenu)
    ServerEventHandler.getInstance().on('HideHouseMenu', hideHouseHide)
    ServerEventHandler.getInstance().on('ShowHouseStorageMenu', showHouseStorageMenu)
    ServerEventHandler.getInstance().on('HideHouseStorageMenu', hideHouseStorageHide)

    function showHouseMenu(args: any[]) {
        houseMenu.show(args) 
    }

    function hideHouseHide() { 
        houseMenu.hide() 
    }

    function showHouseStorageMenu(args: any[]) {
        let items = JSON.parse(args[0]) as InventoryItem[]
        houseStorageMenu.fillMenu(items)
        houseStorageMenu.show() 
    }

    function hideHouseStorageHide() { 
        houseStorageMenu.hide() 
    }
}

