/// <reference path='../../../types-gt-mp/index.d.ts' />

import { Menu } from '../Menu'
import addCloseItem from '../../utils/CloseItem'

export class FillingMenu extends Menu {
    private vehLitersCount: number = 1
    private canisterLitersCount: number = 1

    constructor() {        
        super(' ')
        FillingMenu.setMenuBanner(this.menu)
    }

    public fillMenu(stationId: number, district: number) {
        this.menu.Clear()
        this.createFillingMenu(stationId, district)
        this.createFoodMenu(district)
        addCloseItem(this.menu, 'Закрыть')
        this.menu.RefreshIndex()
    }

    private createFillingMenu(stationId: number, district: number) {
        let fillingMenu = API.addSubMenu(this.menu, 'Бензин', '', true)
        FillingMenu.setMenuBanner(fillingMenu)
        let fillVehicleItem = API.createListItem('Заправить транспорт', 'Cтоимость одного литра 1$', this.getLitersList(150), 0)
        fillVehicleItem.OnListChanged.connect((sender, index) => this.vehLitersCount = index + 1)
        fillVehicleItem.Activated.connect(() => {
            fillingMenu.GoBack()
            API.triggerServerEvent('FillVehicle', stationId, this.vehLitersCount, district)
        })
        fillingMenu.AddItem(fillVehicleItem)

        let fillCanisterItem = API.createListItem('Заполнить канистру', 'Cтоимость одного литра 1$', this.getLitersList(50), 0)
        fillCanisterItem.OnListChanged.connect((sender, index) => this.canisterLitersCount = index + 1)
        fillCanisterItem.Activated.connect(() => {
            fillingMenu.GoBack()
            API.triggerServerEvent('FillCanister', this.canisterLitersCount, district)
        })
        fillingMenu.AddItem(fillCanisterItem)

        let canisterItem = API.createMenuItem('Канистра для бензина - 20$', 'Позволяет заправлять транспорт')
        canisterItem.Activated.connect(() => API.triggerServerEvent('BuyCanister', 20, district)) 
        fillingMenu.AddItem(canisterItem)       

        addCloseItem(fillingMenu)
    }

    private getLitersList(count: number): System.Collections.Generic.List<any> {
        let result = new List(String)
        for (let i = 1; i <= count; i++) {
            result.Add(`${i} / ${count}`)            
        }
        return result
    }

    private createFoodMenu(district: number) {
        let foodMenu = API.addSubMenu(this.menu, 'Еда', '', true)
        FillingMenu.setMenuBanner(foodMenu)
        let chipsItem = API.createMenuItem('Пачка чипсов - 2$', 'Восстанавливает сытость на 10 ед.')
        chipsItem.Activated.connect(() => API.triggerServerEvent('BuyFood', 10, 2, district))
        foodMenu.AddItem(chipsItem)

        let hotDogItem = API.createMenuItem('Датский хот-дог - 6$', 'Восстанавливает сытость на 20 ед.')
        hotDogItem.Activated.connect(() => API.triggerServerEvent('BuyFood', 20, 6, district)) 
        foodMenu.AddItem(hotDogItem)

        let prepackItem = API.createMenuItem('Бизнес ланч - 10$', 'Восстанавливает сытость на 30 ед.')
        prepackItem.Activated.connect(() => API.triggerServerEvent('BuyFood', 30, 10, district))
        foodMenu.AddItem(prepackItem)

        addCloseItem(foodMenu)
    }

    private addBackItem(menu: NativeUI.UIMenu, name: string = 'Назад') {
        let backItem = API.createMenuItem(`~r~${name}`, '')
        backItem.Activated.connect(() => menu.GoBack())       
        menu.AddItem(backItem)
    }

    private static setMenuBanner(menu: NativeUI.UIMenu) {
        API.setMenuBannerSprite(menu, 'shopui_title_gasstation', 'shopui_title_gasstation');
    }

    protected fillMenuByDefault(): void {}
}