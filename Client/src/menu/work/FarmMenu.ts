import { Menu } from '../Menu'

export class FarmMenu extends Menu {
    constructor() {
        super('Ферма', 'Выберите работу или получите награду:')        
    }

    protected fillMenuByDefault(): void {        
        let farmerItem = API.createMenuItem('Работать фермером', 'Заняться сбором урожая')
        farmerItem.Activated.connect(() => API.triggerServerEvent('WorkAsFarmer'))
        this.menu.AddItem(farmerItem)

        let tractorItem = API.createMenuItem('Работать трактористом', 'Заняться рыхлением почвы и перевозкой урожая')
        tractorItem.Activated.connect(() => API.triggerServerEvent('WorkAsTractorDriver'))
        this.menu.AddItem(tractorItem)    
        
        let salaryItem = API.createMenuItem('~b~Получить деньги', 'Закончить работу и получить деньги')
        salaryItem.Activated.connect(() => API.triggerServerEvent('FarmerSalary'))
        this.menu.AddItem(salaryItem)        
    }
}