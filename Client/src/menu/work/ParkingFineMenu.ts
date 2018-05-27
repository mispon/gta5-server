import { Menu } from '../Menu'

export class ParkingFineMenu extends Menu {
    constructor() {
        super('Штрафстоянка')
    }

    public fillMenu(vehicles: any[]) {
        this.menu.Clear()
        this.createSubMenu(vehicles)
        let workItem = API.createMenuItem('Работать на эвакуаторе', '')
        workItem.Activated.connect(() => API.triggerServerEvent('Wrecker'))
        this.menu.AddItem(workItem)
        let salaryItem = API.createMenuItem('~b~Получить деньги', '')
        salaryItem.Activated.connect(() => API.triggerServerEvent('WreckerSalary'))
        this.menu.AddItem(salaryItem)
    }

    private createSubMenu(vehicles: any[]): NativeUI.UIMenu {
        const price = 200
        let vehiclesMenu = API.addSubMenu(this.menu, 'Забрать транспорт', `Оплата штрафстоянки - ${price}$`, true)
        let backItem = API.createMenuItem('~r~Назад', '')
        backItem.Activated.connect(() => vehiclesMenu.GoBack())
        vehiclesMenu.AddItem(backItem)
        for (let index = 0; index < vehicles.length; index++) {
            const vehicle = vehicles[index];
            let name = API.getVehicleDisplayName(vehicle.Hash)
            let vehicleItem = API.createMenuItem(name, '')
            vehicleItem.Activated.connect(() => {
                API.triggerServerEvent('GetVehicleFromParkingFine', JSON.stringify(vehicle), price)
                vehiclesMenu.GoBack()
            })
            vehiclesMenu.AddItem(vehicleItem)
        }
        return vehiclesMenu
    }

    protected fillMenuByDefault() {}
}