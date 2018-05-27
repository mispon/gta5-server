import { Menu } from '../Menu'

export class BistroFoodMenu extends Menu {
    constructor() {
        super('Меню закусочной', 'Выберите еду:')        
    }

    public fillMenu(driverName: string) {
        this.menu.Clear()
        let items = BistroFoodMenu.createItems(driverName, -1)
        for (let i = 0; i < items.length; i++) {
            this.menu.AddItem(items[i])
        }
    }

    public static createItems(sellerName: string, street: number): NativeUI.UIMenuItem[] {
        const takoPrice = 5
        const takoSatiety = 30
        let tacoItem = API.createMenuItem(`Тако (${takoPrice}$)`, `Восстанавливает сытость на ${takoSatiety} ед.`)
        tacoItem.Activated.connect(() => API.triggerServerEvent('BuyBistroFood', takoPrice, takoSatiety, sellerName, street))
        const burgerPrice = 10
        const burgerSatiety = 50
        let burgerItem = API.createMenuItem(`Острый бургер (${burgerPrice}$)`, `Восстанавливает сытость на ${burgerSatiety} ед.`)
        burgerItem.Activated.connect(() => API.triggerServerEvent('BuyBistroFood', burgerPrice, burgerSatiety, sellerName, street))
        const burritoPrice = 15
        const burritoSatiety = 80
        let burritoItem = API.createMenuItem(`Буррито (${burritoPrice}$)`, `Восстанавливает сытость на ${burritoSatiety} ед.`)
        burritoItem.Activated.connect(() => API.triggerServerEvent('BuyBistroFood', burritoPrice, burritoSatiety, sellerName, street))
        return [tacoItem, burgerItem, burritoItem]
    }

    protected fillMenuByDefault(): void {}
}