import { Menu } from '../Menu'

export class LoaderMenu extends Menu {
    constructor() {
        super('Работа грузчикa', 'Выберите работу или получите награду:')        
    }

    protected fillMenuByDefault(): void {
        let loaderItem = API.createMenuItem('Начать работу', 'Переносить груз c точки на точку');
        this.menu.AddItem(loaderItem)
        loaderItem.Activated.connect(() => API.triggerServerEvent('LoaderWork'))        

        let salaryItem = API.createMenuItem('~b~Получить деньги', 'Закончить работу и получить деньги');
        this.menu.AddItem(salaryItem)
        salaryItem.Activated.connect(() => API.triggerServerEvent('LoaderSalary'))
    }
}