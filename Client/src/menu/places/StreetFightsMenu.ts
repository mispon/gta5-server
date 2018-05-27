import { Menu } from '../Menu'

export class StreetFightsMenu extends Menu {
    constructor() {
        super('Уличные драки')        
    }

    public fillMenu(members: number) {
        this.menu.Clear()
        let registerItem = API.createMenuItem('Зарегистрироваться (100$)', `Количество участников: ${members} шт.`)
        registerItem.Activated.connect(() => API.triggerServerEvent('RegisterOnFighting'))
        this.menu.AddItem(registerItem)

        let cancelItem = API.createMenuItem('Отменить участие', 'Отказаться и забрать свою ставку')
        cancelItem.Activated.connect(() => API.triggerServerEvent('CancelRegisterOnFighting'))
        this.menu.AddItem(cancelItem)

        let clotheItem = API.createMenuItem('~r~Закрыть', '')
        clotheItem.Activated.connect(() => this.menu.GoBack())
        this.menu.AddItem(clotheItem)
    }

    protected fillMenuByDefault(): void {}
}