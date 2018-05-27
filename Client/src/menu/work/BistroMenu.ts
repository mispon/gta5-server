import { Menu } from '../Menu'
import { BistroFoodMenu } from '../../menu/work/BistroFoodMenu'

export class BistroMenu extends Menu {
    constructor() {
        super('Закусочная', 'Выберите работу или получите награду:')        
    }

    protected fillMenuByDefault(): void {
        var foodMenu = API.addSubMenu(this.menu, 'Купить еду', '', true)
        let items = BistroFoodMenu.createItems('', 0)
        for (let i = 0; i < items.length; i++) {
            foodMenu.AddItem(items[i])
        }
        foodMenu.AddItem(API.createMenuItem('~r~Назад', '')) 
        foodMenu.OnItemSelect.connect(() => foodMenu.GoBack())

        const rentCost = 300;
        let loaderItem = API.createMenuItem(`Аренда фургона (${rentCost}$)`, 'Продажа еды в людных местах');
        this.menu.AddItem(loaderItem)
        loaderItem.Activated.connect(() => API.triggerServerEvent('FoodTrunkDriver', rentCost))

        let loaderDriverItem = API.createMenuItem('Доставка еды', 'Доставка заказов на время');
        this.menu.AddItem(loaderDriverItem)
        loaderDriverItem.Activated.connect(() => API.triggerServerEvent('FoodDeliveryman'))

        let salaryItem = API.createMenuItem('~b~Получить деньги', 'Закончить работу и получить деньги');
        this.menu.AddItem(salaryItem)
        salaryItem.Activated.connect(() => API.triggerServerEvent('BistroSalary'))
    }
}