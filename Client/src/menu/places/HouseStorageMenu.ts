/// <reference path='../../../types-gt-mp/index.d.ts' />

import { Menu } from '../Menu'
import { InventoryItem } from '../../models/InventoryItem'
import addCloseItem from '../../utils/CloseItem'

export class HouseStorageMenu extends Menu {
    constructor() {
        super('Хранилище')
    }

    public fillMenu(items: InventoryItem[]) {
        this.menu.Clear()
        let putSubMenu = API.addSubMenu(this.menu, 'Положить вещи', 'Поместить вещи в хранилище', true)
        let takeSubMenu = API.addSubMenu(this.menu, 'Забрать вещи', 'Забрать вещи из хранилища', true)
        items.forEach(item => {
            if (item.Count > 0) {
                this.addToSubMenu(putSubMenu, item, true)
            }
            if (item.CountInHouse > 0) {
                this.addToSubMenu(takeSubMenu, item, false)
            }
        })
        addCloseItem(putSubMenu)
        addCloseItem(takeSubMenu)
        addCloseItem(this.menu, 'Закрыть')
    }

    private addToSubMenu(menu: NativeUI.UIMenu, item: InventoryItem, isPut: boolean) {
        let allCount = isPut ? item.Count : item.CountInHouse
        let desc = isPut ? 'Нажмите Enter, чтобы положить' : 'Нажмите Enter, чтобы забрать'
        let serverEvent = isPut ? 'PutItemToStorage' : 'TakeItemToStorage'
        let menuItem = API.createMenuItem(`${item.Name}, ${allCount}${this.getPostfix(item.Type)}`, desc)
        menuItem.Activated.connect(() => {
            let count = allCount
            if (count > 1) {
                let countInput = API.getUserInput('', 10)
                count = parseInt(countInput)
                if (isNaN(count)) {
                    API.sendNotification('~r~Введено некорректное количество')
                    return
                }
            }
            API.triggerServerEvent(serverEvent, JSON.stringify(item), count)            
        })
        menu.AddItem(menuItem)
    }

    private getPostfix(type: number): string {
        switch(type) {
            case 4:
                return '$'
            case 10:
                return ' л.'
            default:
                return ' шт.'
        }
    }

    protected fillMenuByDefault() {}
}
