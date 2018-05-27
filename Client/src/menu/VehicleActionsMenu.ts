/// <reference path='../../types-gt-mp/index.d.ts' />

import { Menu } from './Menu'
import {InventoryItem} from '../models/InventoryItem'
import addCloseItem from '../utils/CloseItem'

export class VehicleActionsMenu extends Menu {
    constructor() {
        super('Транспорт')
        VehicleActionsMenu.setMenuBanner(this.menu)
    }

    public fillMenu(args: any[]) {
        this.menu.Clear()
        this.addTurnItem()
        this.addLockItem()
        if (args.length == 1 && <boolean> args[0]) {
            this.addVansTrunkItem()
        }
        if (args.length == 4) {
            let inventory = JSON.parse(args[0]) as InventoryItem[]
            let trunk = JSON.parse(args[1]) as InventoryItem[]
            let weight = args[2] as number
            let carrying = args[3] as number
            this.addTrunkMenu(inventory, trunk, weight, carrying)
        }
        addCloseItem(this.menu, 'Закрыть')
    }    

    private addTurnItem() {
        let lockItem = API.createMenuItem('Ключ зажигания', 'Завести или заглушить двигатель')
        lockItem.Activated.connect(() => API.triggerServerEvent('TurnEngine'))
        this.menu.AddItem(lockItem)
    }

    private addLockItem() {
        let lockItem = API.createMenuItem('Дверной замок', 'Закрыть или открыть транспорт')
        lockItem.Activated.connect(() => API.triggerServerEvent('LockVehicle'))
        this.menu.AddItem(lockItem)
    }

    private addTrunkMenu(inventory: InventoryItem[], trunk: InventoryItem[], weight: number, carrying: number) {
        let color = weight > carrying ? '~r~' : '~b~'
        let trunkMenu = API.addSubMenu(this.menu, 'Багажник', `Загруженность: ${color}${weight} / ${carrying}`, true)
        this.addPutSubMenu(trunkMenu, inventory)
        this.addTakeSubMenu(trunkMenu, trunk)
        addCloseItem(trunkMenu, 'Назад')
        trunkMenu.OnMenuClose.connect(() => API.triggerServerEvent('ChangeDoorState', 5, false))
    }

    private addPutSubMenu(menu: NativeUI.UIMenu, inventory: InventoryItem[]) {
        let putMenu = API.addSubMenu(menu, 'Положить', 'Положить вещи из инвентаря в багажник', true)
        inventory.sort(e => e.Type).forEach(item => {
            let putItem = API.createMenuItem(item.Name, `Количество: ~b~${item.Count}`)
            putItem.Activated.connect(() => {
                let count = item.Count > 1 ? parseInt(API.getUserInput('', 3)) : 1
                if (isNaN(count)) {
                    API.sendColoredNotification('Некорректный ввод', 0, 6)
                    return
                }
                API.triggerServerEvent('PutInTrunk', JSON.stringify(item), count)
            })
            putMenu.AddItem(putItem)
        })
        addCloseItem(putMenu, 'Назад')
    }

    private addTakeSubMenu(menu: NativeUI.UIMenu, trunk: InventoryItem[]) {
        let takeMenu = API.addSubMenu(menu, 'Забрать', 'Забрать вещи из багажника в инвентарь', true)
        trunk.sort(e => e.Type).forEach(item => {
            let takeItem = API.createMenuItem(item.Name, `Количество: ~b~${item.Count}`)
            takeItem.Activated.connect(() => {
                let count = item.Count > 1 ? parseInt(API.getUserInput('', 3)) : 1
                if (isNaN(count)) {
                    API.sendColoredNotification('Некорректный ввод', 0, 6)
                    return
                }
                API.triggerServerEvent('TakeFromTrunk', JSON.stringify(item), count)
            })
            takeMenu.AddItem(takeItem)
        })
        addCloseItem(takeMenu, 'Назад')
    }

    private addVansTrunkItem() {
        let vansTrunkItem = API.createMenuItem('Багажник', 'Закрыть или открыть багажник')
        vansTrunkItem.Activated.connect(() => API.triggerServerEvent('TriggerVansTrunk'))
        this.menu.AddItem(vansTrunkItem)
    }

    private static setMenuBanner(menu: NativeUI.UIMenu) {
        API.setMenuBannerSprite(menu, 'shopui_title_ie_modgarage', 'shopui_title_ie_modgarage')
    }

    protected fillMenuByDefault() {
        this.menu.OnItemSelect.connect((sender, item, index) => {
            if (item.Text == 'Багажник') {
                API.triggerServerEvent('ChangeDoorState', 5, true)
            }
        })
    }
}