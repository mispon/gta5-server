/// <reference path='../../types-gt-mp/index.d.ts' />

import { Menu } from './Menu'
import { RaceType } from '../enums/RaceType'

export class RaceMenu extends Menu {
    constructor() {
        super('Гонки', 'Выберите тип гонки:')
    }

    public fillMenu(cars: any[], motos: any[], membersInfo: any) {
        this.menu.Clear()
        this.createCarRaceMenu(cars, RaceType.Cars, membersInfo.cars)
        this.createMotoRaceMenu(motos, RaceType.Moto, membersInfo.moto)

        let rallyItem = API.createMenuItem(`Ралли (${membersInfo.rally} из 10)`, 'Гонка по бездорожью')
        rallyItem.Activated.connect(() => API.triggerServerEvent('RegisterOnRace', RaceType.Rally))
        this.menu.AddItem(rallyItem)

        let mountainItem = API.createMenuItem(`Горный трек (${membersInfo.mountain} из 10)`, 'Гонка на горных байках')
        mountainItem.Activated.connect(() => API.triggerServerEvent('RegisterOnRace', RaceType.Mountain))
        this.menu.AddItem(mountainItem)

        let cancelRaceItem = API.createMenuItem('Отменить участие', 'Снять свою кандидатуру с гонки')
        cancelRaceItem.Activated.connect(() => API.triggerServerEvent('CancelRace'))
        this.menu.AddItem(cancelRaceItem)

        this.addBackItem(this.menu, 'Закрыть')
    }

    // создает подменю гонок на машинах
    private createCarRaceMenu(cars: any[], type: RaceType, members: any) {
        let carMenu = API.addSubMenu(this.menu, `Автогонка (${members} из 10)`, 'Классическая гонка на машинах', true)
        this.createVehiclesSelectionMenu(carMenu, cars, type)
        const hashes = [482197771, -1297672541, 1392481335]
        this.createVehiclesRentMenu(carMenu, hashes, type)
        this.addBackItem(carMenu, 'Назад')
    }    

    // создает подменю гонок на мотоциклах
    private createMotoRaceMenu(motos: any[], type: RaceType, members: any) {
        let motoMenu = API.addSubMenu(this.menu, `Мотогонка (${members} из 10)`, 'Классическая гонка на мотоциклах', true)
        this.createVehiclesSelectionMenu(motoMenu, motos, type)
        const hashes = [-114291515, -1670998136, 1265391242]
        this.createVehiclesRentMenu(motoMenu, hashes, type)
        this.addBackItem(motoMenu, 'Назад')
    }

    // создает подменю выбора транспорта для гонок
    private createVehiclesSelectionMenu(raceMenu: NativeUI.UIMenu, vehicles: any[], type: RaceType) {
        let chooseVehicle = API.addSubMenu(raceMenu, 'Выбрать транспорт', 'Выбрать из списка собственного транспорта', true)        
        vehicles.forEach(vehicle => {
            let vehicleName = API.getVehicleDisplayName(vehicle.Hash)
            let maxSpeed = API.getVehicleMaxSpeed(vehicle.Hash) * 3.6
            let vehicleItem = API.createMenuItem(vehicleName, `~b~speed: ${Math.floor(maxSpeed)} km/h`)
            vehicleItem.Activated.connect(() => {
                chooseVehicle.GoBack()
                raceMenu.GoBack()
                API.triggerServerEvent('RegisterOnRace', type, JSON.stringify(vehicle))
            })
            chooseVehicle.AddItem(vehicleItem)
        })
        this.addBackItem(chooseVehicle, 'Назад')
    }

    // создает подменю для аренды гоночного транспорта
    private createVehiclesRentMenu(raceMenu: NativeUI.UIMenu, vehicleHashes: any[], type: RaceType) {
        const cost = 200;
        let rentVehicle = API.addSubMenu(raceMenu, 'Арендовать транспорт', `Арендовать транспорт для гонки (${cost}$)`, true)        
        vehicleHashes.forEach(vehicleHash => {
            let vehicleName = API.getVehicleDisplayName(vehicleHash)
            let maxSpeed = API.getVehicleMaxSpeed(vehicleHash) * 3.6
            let vehicleItem = API.createMenuItem(vehicleName, `~b~speed: ${Math.floor(maxSpeed)} km/h`)
            vehicleItem.Activated.connect(() => {
                rentVehicle.GoBack()
                raceMenu.GoBack()
                API.triggerServerEvent('RegisterOnRaceWithRent', type, vehicleHash, cost)
            })
            rentVehicle.AddItem(vehicleItem)
        })
        this.addBackItem(rentVehicle, 'Назад')
    }

    private addBackItem(menu: NativeUI.UIMenu, name: string = 'Назад') {
        let backItem = API.createMenuItem(`~r~${name}`, '')
        backItem.Activated.connect(() => menu.GoBack())       
        menu.AddItem(backItem)
    }

    protected fillMenuByDefault() {}
}