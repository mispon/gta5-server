/// <reference path='../../../types-gt-mp/index.d.ts' />

import { Menu } from '../Menu'
import addCloseItem from '../../utils/CloseItem'

export class PoliceDepartmentMenu extends Menu {
    constructor() {
        super('Деп. полиции', 'Выберите работу или получите награду:')        
    }

    public fillMenu(hasQuest: boolean) {
        this.menu.Clear()
        this.addWorkMenu()

        let payPenaltyItem = API.createMenuItem('Оплатить штраф', 'Оплата штрафа и снятие с розыска')        
        payPenaltyItem.Activated.connect(() => API.triggerServerEvent('PayPenalty'))
        this.menu.AddItem(payPenaltyItem)

        const price = 2000
        let licenseItem = API.createMenuItem('Приобрести лицензию', `Купить лицензию на оружие за ${price}`)        
        licenseItem.Activated.connect(() => API.triggerServerEvent('BuyWeaponLicense', price))
        this.menu.AddItem(licenseItem)

        if (hasQuest) {
            let questKeyItem = API.createMenuItem('~g~Попросить ключ', 'Получить ключ от хранилища документов')
            questKeyItem.SetRightBadge(BadgeStyle.Crown)            
            questKeyItem.Activated.connect(() => API.triggerServerEvent('AskClanQuestKey'))
            this.menu.AddItem(questKeyItem)
        }
        addCloseItem(this.menu, 'Закрыть')
    }

    private addWorkMenu() {
        let workMenu = API.addSubMenu(this.menu, 'Работа', 'Работать полицейским', true)

        let workItem = API.createMenuItem('Начать работу', 'Охранять порядок в городе')        
        workItem.Activated.connect(() => API.triggerServerEvent('WorkInPolice'))
        workMenu.AddItem(workItem)

        let salaryItem = API.createMenuItem('~b~Получить деньги', 'Закончить работу и получить деньги')        
        salaryItem.Activated.connect(() => API.triggerServerEvent('PoliceSalary'))
        workMenu.AddItem(salaryItem)

        addCloseItem(workMenu)
    }

    protected fillMenuByDefault(): void {}
}