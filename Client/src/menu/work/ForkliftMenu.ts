import { Menu } from '../Menu'

export class ForkliftMenu extends Menu {
    constructor() {
        super('Работа грузчикa', 'Выберите работу или получите награду:')        
    }

    protected fillMenuByDefault(): void {
        let loaderDriverItem = API.createMenuItem('Начать работу', 'Перевозить груз на погрузчике');
        this.menu.AddItem(loaderDriverItem)
        loaderDriverItem.Activated.connect(() => API.triggerServerEvent('ForkliftWork'))

        let salaryItem = API.createMenuItem('~b~Получить деньги', 'Закончить работу и получить деньги');
        this.menu.AddItem(salaryItem)
        salaryItem.Activated.connect(() => API.triggerServerEvent('ForkliftSalary'))
    }
}