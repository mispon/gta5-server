/// <reference path='../../../types-gt-mp/index.d.ts' />

import { Menu } from '../Menu'
import { WeaponGood } from '../../models/WeaponGood'
import addCloseItem from '../../utils/CloseItem'

export class AmmuNationMenu extends Menu {
    constructor() {
        super(' ', 'Каталог оружия:')
        AmmuNationMenu.setMenuBanner(this.menu)
    }

    public fillMenu(weapons: WeaponGood[], ammo: WeaponGood[], district: number) {
        this.menu.Clear()
        this.addWeaponMenu(weapons, district)
        this.addAmmoMenu(ammo, district)
        addCloseItem(this.menu, 'Закрыть')
    }

    private addWeaponMenu(weapons: WeaponGood[], district: number) {
        let weaponMenu = API.addSubMenu(this.menu, 'Оружие', 'Ассортимент оружия', true)
        AmmuNationMenu.setMenuBanner(weaponMenu)
        weapons.forEach(weapon => {
            let weaponItem = API.createMenuItem(`${weapon.Name}`, `Цена: ~b~${weapon.Price}$`)
            weaponItem.Activated.connect(() => {
                API.triggerServerEvent('BuyWeapon', JSON.stringify(weapon), district)
                weaponMenu.GoBack()
            })
            weaponMenu.AddItem(weaponItem)
        })
        addCloseItem(weaponMenu)
    }

    private addAmmoMenu(ammo: WeaponGood[], district: number) {
        let ammoMenu = API.addSubMenu(this.menu, 'Патроны', 'Ассортимент боезапасов', true)
        AmmuNationMenu.setMenuBanner(ammoMenu)
        ammo.forEach(ammoGood => {
            let ammoItem = API.createMenuItem(`${ammoGood.Name}`, `Цена: ~b~${ammoGood.Price}$`)
            ammoItem.Activated.connect(() => {
                let ammoCount = parseInt(API.getUserInput('', 3))
                if (isNaN(ammoCount)) {
                    API.sendNotification('~r~Некорректное количество')
                    return
                }
                API.triggerServerEvent('BuyAmmo', JSON.stringify(ammoGood), ammoCount, district)
                ammoMenu.GoBack()
            })
            ammoMenu.AddItem(ammoItem)
        })
        addCloseItem(ammoMenu)
    }

    private static setMenuBanner(menu: NativeUI.UIMenu) {
        API.setMenuBannerSprite(menu, 'shopui_title_gunclub', 'shopui_title_gunclub');
    }

    protected fillMenuByDefault() {}
}