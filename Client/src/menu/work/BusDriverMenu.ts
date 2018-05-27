import { Menu } from '../Menu'

export class BusDriverMenu extends Menu {
    constructor() {
        super('Водитель авт.', 'Выберите работу или получите награду:')        
    }

    protected fillMenuByDefault(): void {
        let redRouteItem = API.createMenuItem('Начать работу', 'Работать водителем автобуса')
        this.menu.AddItem(redRouteItem)
        redRouteItem.Activated.connect(() => API.triggerServerEvent('WorkOnBus'))

        let salaryItem = API.createMenuItem('~b~Получить деньги', 'Закончить работу и получить деньги')
        this.menu.AddItem(salaryItem)
        salaryItem.Activated.connect(() => API.triggerServerEvent('BusDriverSalary'))
    }
}