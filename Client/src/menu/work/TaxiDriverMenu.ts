import { Menu } from '../Menu'

export class TaxiDriverMenu extends Menu {
    constructor() {
        super('Водитель такси', 'Выберите работу или получите награду:')        
    }

    protected fillMenuByDefault(): void {
        let workItem = API.createMenuItem('Начать работу', 'Заняться перевозкой людей');
        this.menu.AddItem(workItem)
        workItem.Activated.connect(() => API.triggerServerEvent('WorkInTaxi'))

        let salaryItem = API.createMenuItem('~b~Получить деньги', 'Закончить работу и получить деньги');
        this.menu.AddItem(salaryItem)
        salaryItem.Activated.connect(() => API.triggerServerEvent('TaxiDriverSalary'))
    }
}