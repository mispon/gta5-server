/// <reference path='../../types-gt-mp/index.d.ts' />

import { Menu } from './Menu'

export class AdminMenu extends Menu {
    constructor() {
        super('Меню админа', 'Доступные функции')
        API.setMenuBannerSprite(this.menu, 'shopui_title_gr_gunmod', 'shopui_title_gr_gunmod')       
    }

    protected fillMenuByDefault(): void {
        let positionItem = API.createMenuItem('Текущая позиция', 'Вывести в чат вашу текущую позицию')
        positionItem.Activated.connect(() => API.triggerServerEvent('GetPosition'))
        this.menu.AddItem(positionItem)

        let rotationItem = API.createMenuItem('Текущий поворот', 'Вывести в чат информацию об угле поворота')
        rotationItem.Activated.connect(() => API.triggerServerEvent('GetRotation'))
        this.menu.AddItem(rotationItem)

        let carItem = API.createMenuItem('Машина', 'Присуммонить машину супер-класса')
        carItem.Activated.connect(() => API.triggerServerEvent('GetCar'))
        this.menu.AddItem(carItem)

        let weaponItem = API.createMenuItem('Оружие', 'Взять случайное оружие')
        weaponItem.Activated.connect(() => API.triggerServerEvent('GetWeapon'))
        this.menu.AddItem(weaponItem)
    }
}