/// <reference path='../../types-gt-mp/index.d.ts' />

import { Menu } from './Menu'
import { InventoryItem } from '../models/InventoryItem'
import addCloseItem from '../utils/CloseItem'

export class InventoryMenu extends Menu {
    private countToUse: number = 1
    private weaponsTypes: number[] = [8, 9]
    private thingsTypes: number[] = [4, 5, 6, 7]

    constructor() {
        super('Инвентарь')
        InventoryMenu.setMenuBanner(this.menu)
    }

    public fillMenu(items: InventoryItem[], weight: number) {
        this.menu.Clear()
        let color = weight > 100 ? '~r~' : '~b~'
        let desc = `Вес: ${color}${weight} / 100`
        let consumMenu = API.addSubMenu(this.menu, 'Расходуемое', desc, true)
        InventoryMenu.setMenuBanner(consumMenu)
        let weaponMenu = API.addSubMenu(this.menu, 'Оружие', desc, true)
        InventoryMenu.setMenuBanner(weaponMenu)
        let thingsMenu = API.addSubMenu(this.menu, 'Вещи', desc, true)
        InventoryMenu.setMenuBanner(thingsMenu)
        items.forEach(item => {
            if (this.thingsTypes.indexOf(item.Type) != -1) {
                this.addItem(item, thingsMenu)
            }
            else if (this.weaponsTypes.indexOf(item.Type) != -1) {
                this.addItem(item, weaponMenu)
            }
            else {
                this.addListItem(item, consumMenu)
            }
        })
        addCloseItem(consumMenu)
        addCloseItem(weaponMenu)
        addCloseItem(thingsMenu)
        addCloseItem(this.menu, 'Закрыть')
    }

    private addItem(item: InventoryItem, menu: NativeUI.UIMenu) {
        let menuItem = API.createMenuItem(item.Name, `Количество: ${item.Count}`)
        menu.AddItem(menuItem)
    }

    private addListItem(item: InventoryItem, menu: NativeUI.UIMenu) {
        let countToUse = new List(String)
        for (let i = 1; i <= item.Count; i++) {
            countToUse.Add(`${i} / ${item.Count}`)                
        }
        let useItem = API.createListItem(item.Name, `Количество: ${item.Count}`, countToUse, 0)
        useItem.OnListChanged.connect((sender, index) => this.countToUse = index + 1)
        useItem.Activated.connect(() => API.triggerServerEvent('UseInventoryItem', JSON.stringify(item), this.countToUse))
        menu.AddItem(useItem)
    }

    private static setMenuBanner(menu: NativeUI.UIMenu) {
        API.setMenuBannerSprite(menu, 'shopui_title_gr_gunmod', 'shopui_title_gr_gunmod');
    }

    protected fillMenuByDefault() {}
}