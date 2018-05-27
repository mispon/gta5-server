/// <reference path='../../../types-gt-mp/index.d.ts' />

import { Menu } from '../Menu'
import addCloseItem from '../../utils/CloseItem'

export class HouseMenu extends Menu {
    private enterItem: NativeUI.UIMenuItem
    private exitItem: NativeUI.UIMenuItem
    private lockItem: NativeUI.UIMenuItem
    private garageEnterItem: NativeUI.UIMenuItem
    private garageExitItem: NativeUI.UIMenuItem
    
    private daysRent: number[] = [1, 3, 7]

    constructor() {
        super('Дом')
    }

    public show(args: any[]) {
        let type = args[0] as number
        let houseData = args[1] as string        
        this.fillMenuByType(type, houseData)
        super.show()
    }

    public hide() {
        super.hide()
    }

    private fillMenuByType(type: number, houseData: string) {        
        this.menu.Clear()
        let house = JSON.parse(houseData)
        this.createItems(house)
        switch (type) {
            // дом свободен
            case 1:                
                this.createRentItems(this.menu, house)
                break
            // хозяин дома
            case 2:                
                this.menu.AddItem(this.enterItem)
                this.menu.AddItem(this.lockItem)
                this.addRentExtensionSubmenu(house)
                break
            // чужой дом
            case 3:
                this.menu.AddItem(this.enterItem)
                break
            // выход из дома
            case 4:                
                this.menu.AddItem(this.exitItem)
                this.menu.AddItem(this.lockItem)
                break
            // вход в гараж
            case 5:
                this.menu.AddItem(this.garageEnterItem)
                break
            // выход из гаража
            case 6:
                this.menu.AddItem(this.garageExitItem)
                break
        }        
        addCloseItem(this.menu, 'Закрыть') 
    }

    private createRentItems(menu: NativeUI.UIMenu, house: any, isRent: boolean = true) {
        let houseData = JSON.stringify(house)
        this.daysRent.forEach(days => {
            let label = isRent
                ? `Арендовать дом на ${days} ${this.getWord(days)}`
                : `Продлить аренду на ${days} ${this.getWord(days)}`
            let item = API.createMenuItem(label, `Стоимость аренды одного дня: ${house.DailyRent}$`)
            item.Activated.connect(() => API.triggerServerEvent('GetHouseRent', houseData, days))
            menu.AddItem(item)
        })
    }

    private createItems(house: any) {
        let houseData = JSON.stringify(house)

        this.enterItem = API.createMenuItem('Войти', '')
        this.enterItem.Activated.connect(() => API.triggerServerEvent('EnterHouse', houseData))

        this.exitItem = API.createMenuItem('Выйти', '')
        this.exitItem.Activated.connect(() => API.triggerServerEvent('ExitHouse', houseData))

        let lockName = house.Lock ? 'Отпереть дверь' : 'Запереть дверь'
        this.lockItem = API.createMenuItem(lockName, 'В открытый дом может войти любой игрок')
        this.lockItem.Activated.connect(() => API.triggerServerEvent('LockHouse', houseData))

        this.garageEnterItem = API.createMenuItem('Войти в гараж', '')
        this.garageEnterItem.Activated.connect(() => API.triggerServerEvent('EnterGarage', houseData))

        this.garageExitItem = API.createMenuItem('Выйти из гаража', '')
        this.garageExitItem.Activated.connect(() => API.triggerServerEvent('ExitGarage', houseData))
    }

    private addRentExtensionSubmenu(house: any) {
        let rentExtMenu = API.addSubMenu(this.menu, 'Продление аренды', '', true)
        this.createRentItems(rentExtMenu, house, false)
        addCloseItem(rentExtMenu)
    }

    private getWord(days: number) {
        switch (days) {
            case 1:
                return 'день'
            case 3:
                return 'дня'
            case 7:
                return 'дней'
            default:
                return '';
        }
    }

    protected fillMenuByDefault(): void {}
}