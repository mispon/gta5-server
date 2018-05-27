/// <reference path='../../types-gt-mp/index.d.ts' />

import { Menu } from './Menu'
import addCloseItem from '../utils/CloseItem'

export class GPS extends Menu {
    constructor() {
        super('GPS', 'Выберите точку назначения:')        
    }

    protected fillMenuByDefault(): void {
        this.addWorksMenu()
        this.addServicesMenu()
        this.addEventsMenu()
        addCloseItem(this.menu, 'Закрыть')
    }

    private addWorksMenu() {
        let worksMenu = API.addSubMenu(this.menu, 'Работа', '', true)
        this.fillSubMenu(worksMenu, this.getWorkItems())
    }

    private addServicesMenu() {
        let servicesMenu = API.addSubMenu(this.menu, 'Услуги', '', true)
        this.fillSubMenu(servicesMenu, this.getServicesItems())
    }

    private addEventsMenu() {
        let eventMenu = API.addSubMenu(this.menu, 'Эвенты', '', true)
        this.fillSubMenu(eventMenu, this.getEventItems())
    }

    private fillSubMenu(menu: NativeUI.UIMenu, items: any[]) {
        items.forEach(item => {
            let index = parseInt(Object.keys(item)[0])
            item[index].Activated.connect(() => {
                menu.GoBack()
                API.triggerServerEvent('GPSPoint', index)
            })
            menu.AddItem(item[index])
        });
        addCloseItem(menu)
    }

    private getWorkItems() : any[] {
        return [            
            {2 : API.createMenuItem('Свалка металлолома', '~b~Работа грузчиком (1 ур.)')},
            {1 : API.createMenuItem('Текстильная фабрика', '~b~Работа водителем погрузчика (2 ур.)')},
            {17 : API.createMenuItem('Ферма', '~b~Работа фермером(1 ур.) и трактористом (2 ур.)')},
            {3 : API.createMenuItem('Стройка', '~b~Работа строителем (3 ур.)')},
            {9 : API.createMenuItem('Таксопарк', '~b~Работа в такси (3 ур.)')}, 
            {16 : API.createMenuItem('Рыбацкое поселение', '~b~Работа рыбаком (4 ур.)')},         
            {4 : API.createMenuItem('Автобусное депо', '~b~Работа водителем автобуса (4 ур.)')},
            {7 : API.createMenuItem('Закусочная', '~b~Работа курьером и водителем фургона с едой (5 ур.)')},
            {6 : API.createMenuItem('Штрафстоянка', '~b~Работа на эвакуаторе (6 ур.)')},            
            {11 : API.createMenuItem('Порт', '~b~Работа дальнобойщиком (7 ур.)')},
            {15 : API.createMenuItem('Аэропорт', '~b~Работа летчиком (8 ур.)')},
            {10 : API.createMenuItem('Департамент полиции', '~b~Работа и услуги полиции (9 ур.)')}
        ]
    }

    private getServicesItems() : any[] {
        return [
            {0 : API.createMenuItem('Автошкола', '~b~Получение водительской лицензии')},
            {5 : API.createMenuItem('Главная парковка', '~b~Личные транспортные средства')},
            {6 : API.createMenuItem('Штрафстоянка', '~b~Получение эвакуированного транспорта')},
            {7 : API.createMenuItem('Закусочная', '~b~Вкусная и качественная еда')},
            {8 : API.createMenuItem('Госпиталь', '~b~Восстановление здоровья')},
            {10 : API.createMenuItem('Департамент полиции', '~b~Оплата штрафов')},
            {14 : API.createMenuItem('Автомастерская', '~b~Улучшение личного транспорта')}
        ]
    }

    private getEventItems() : any[] {
        return [
            {12 : API.createMenuItem('Гонки', '~b~Соревнование с другими игроками')},
            {13 : API.createMenuItem('Уличные драки', '~b~Соревнование с другими игроками')}
        ]
    }
}