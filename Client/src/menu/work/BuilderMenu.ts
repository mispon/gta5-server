import { Menu } from '../Menu'

export class BuilderMenu extends Menu {
    constructor() {
        super('Стройка', 'Выберите работу или получите награду:')        
    }

    protected fillMenuByDefault(): void {
        let builderItem = API.createMenuItem('Начать работу', 'Перевозить груз на погрузчике');
        this.menu.AddItem(builderItem)
        builderItem.Activated.connect(() => API.triggerServerEvent('BuilderWork'))

        let salaryItem = API.createMenuItem('~b~Получить деньги', 'Закончить работу и получить деньги');
        this.menu.AddItem(salaryItem)
        salaryItem.Activated.connect(() => API.triggerServerEvent('BuilderSalary'))
    }
}