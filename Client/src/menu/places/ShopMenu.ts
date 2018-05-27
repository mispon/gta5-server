/// <reference path='../../../types-gt-mp/index.d.ts' />

import { Menu } from '../Menu'
import addCloseItem from '../../utils/CloseItem'

export class ShopMenu extends Menu {
    private medicineCount: number = 1;
    private packedLunchCount: number = 1;

    constructor() {        
        super(' ')
        ShopMenu.setMenuBanner(this.menu)
    }

    public fillMenu(district: number) {
        this.menu.Clear()        
        this.createFoodMenu(district)
        this.createThingsMenu(district)
        this.createPhoneMenu(district)
        addCloseItem(this.menu, 'Закрыть')
        this.menu.RefreshIndex()
    }

    private createFoodMenu(district: number) {
        let foodMenu = API.addSubMenu(this.menu, 'Еда', '', true)
        ShopMenu.setMenuBanner(foodMenu)

        let chocolateItem = API.createMenuItem('Шоколадный батончик - 2$', 'Восстанавливает сытость на 10 ед.')
        chocolateItem.Activated.connect(() => API.triggerServerEvent('BuyFood', 10, 2, district))
        foodMenu.AddItem(chocolateItem)

        let sandwichItem = API.createMenuItem('Сэндвич с индейкой - 6$', 'Восстанавливает сытость на 20 ед.')
        sandwichItem.Activated.connect(() => API.triggerServerEvent('BuyFood', 20, 6, district))
        foodMenu.AddItem(sandwichItem)

        let pizzaItem = API.createMenuItem('Пицца 24 сыра - 10$', 'Восстанавливает сытость на 30 ед.')
        pizzaItem.Activated.connect(() => API.triggerServerEvent('BuyFood', 30, 10, district))
        foodMenu.AddItem(pizzaItem)  

        addCloseItem(foodMenu)
    }

    private createThingsMenu(district: number) {
        let thingsMenu = API.addSubMenu(this.menu, 'Предметы', '', true)
        ShopMenu.setMenuBanner(thingsMenu)

        const packedLunchPrice = 15;
        this.addListItem(thingsMenu, `Сухой паек - ${packedLunchPrice}$`, 'Восстанавливает сытость на 25 ед.', packedLunchPrice, 2, district)

        const medicinePrice = 100
        this.addListItem(thingsMenu, `Аптечка - ${medicinePrice}$`, 'Восстанавливает здоровье на 30 ед.', medicinePrice, 1, district)

        addCloseItem(thingsMenu)
    }

    private createPhoneMenu(district: number) {
        let phoneMenu = API.addSubMenu(this.menu, 'Телефон', '', true)
        const phonePrice = 2000;

        let replenishBalanceItem = API.createMenuItem('Пополнить баланс', '')
        replenishBalanceItem.Activated.connect(() => {
            let amount = parseInt(API.getUserInput('', 3))
            if (isNaN(amount)) {
                API.sendNotification('~r~Некорректный ввод')
                return
            }
            API.triggerServerEvent('ReplenishPhoneBalance', amount)
        })
        phoneMenu.AddItem(replenishBalanceItem)

        let buyPhoneItem = API.createMenuItem('Купить телефон', `Стоимость ${phonePrice}$`)
        buyPhoneItem.Activated.connect(() => API.triggerServerEvent('BuyPhone', phonePrice))
        phoneMenu.AddItem(buyPhoneItem)

        addCloseItem(phoneMenu)
    }

    private addListItem(menu: NativeUI.UIMenu, name: string, desc: string, price: number, type: number, street: number) {
        let count = new List(String)
        for (let i = 1; i <= 10; i++) {
            count.Add(`${i} / 10`)
        }
        let item = API.createListItem(name, desc, count, 0)
        item.OnListChanged.connect((sender, index) => {
            this.changeCount(type, index + 1)
        })
        item.Activated.connect(() => {
            menu.GoBack()
            API.triggerServerEvent('BuyThing', type, price, this.getCount(type), street)
        })
        menu.AddItem(item)
    }

    private changeCount(type: number, count: number) {
        switch (type) {
            // аптечки
            case 1:
                this.medicineCount = count
            break
            // сухие пайки
            case 2:
                this.packedLunchCount = count
            break
        }
    }

    private getCount(type: number): number {
        switch (type) {
            // аптечки
            case 1:
                return this.medicineCount
            // сухие пайки
            case 2:
                return this.packedLunchCount
            default:
                return 0
        }
    }

    private static setMenuBanner(menu: NativeUI.UIMenu) {
        API.setMenuBannerSprite(menu, 'shopui_title_conveniencestore', 'shopui_title_conveniencestore')
    }

    private addBackItem(menu: NativeUI.UIMenu, name: string = 'Назад') {
        let backItem = API.createMenuItem(`~r~${name}`, '')
        backItem.Activated.connect(() => menu.GoBack())       
        menu.AddItem(backItem)
    }

    protected fillMenuByDefault(): void {}
}