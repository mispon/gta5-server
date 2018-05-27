import { Menu } from '../Menu'
import addCloseItem from '../../utils/CloseItem'

export class FishermansMenu extends Menu {
    constructor() {
        super('Рыбная ловля', 'Выберите работу или получите награду:')        
    }

    protected fillMenuByDefault(): void {
        let workSubMenu = API.addSubMenu(this.menu, 'Начать работу', 'Заняться рыбной ловлей', true)
        let workOnPier = API.createMenuItem('Рыбачить на побережье', '')
        workOnPier.Activated.connect(() => {
            workSubMenu.GoBack()
            API.triggerServerEvent('WorkAsFisherman', false)
        })
        workSubMenu.AddItem(workOnPier)
        let workOnBoat = API.createMenuItem('Рыбачить с лодки', 'Необходим 3 ур. работы')
        workOnBoat.Activated.connect(() => {
            workSubMenu.GoBack()
            API.triggerServerEvent('WorkAsFisherman', true)
        })
        workSubMenu.AddItem(workOnBoat)
        addCloseItem(workSubMenu)

        const price = 10
        let baitsItem = API.createMenuItem('Купить наживку', `Приобрести 50шт. наживки за ${price}$`)
        baitsItem.Activated.connect(() => API.triggerServerEvent('BuyFishBaits', price))
        this.menu.AddItem(baitsItem)        
        
        let salaryItem = API.createMenuItem('~b~Получить деньги', 'Закончить работу и получить деньги')
        salaryItem.Activated.connect(() => API.triggerServerEvent('FishermanSalary'))
        this.menu.AddItem(salaryItem)        
    }
}