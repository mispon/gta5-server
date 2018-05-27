/// <reference path='../../types-gt-mp/index.d.ts' />

import { Menu } from './Menu'
import {VectorConverter} from '../utils/VectorConverter'
import addCloseItem from '../utils/CloseItem'

export class PlayerActionsMenu extends Menu {
    private housesMenu: NativeUI.UIMenu

    constructor() {
        super('Действия')
        PlayerActionsMenu.setMenuBanner(this.menu)
    } 
    
    public fillHouseMenu(housesPositions: Vector3[]) {
        this.housesMenu.Clear();
        housesPositions.forEach(pos => {
            let position = VectorConverter.convert(pos)
            let streetName = API.getStreetName(position)
            let item = API.createMenuItem(streetName, 'Переместиться к выбранному дому')
            item.Activated.connect(() => API.triggerServerEvent('TeleportToHouse', position))
            this.housesMenu.AddItem(item)
        });
        addCloseItem(this.housesMenu)
    }

    protected fillMenuByDefault() {
        this.createSendMoneyItem()
        this.createAnimMenu()
        this.createHousesMenu()
        addCloseItem(this.menu, 'Закрыть')
    }

    private createSendMoneyItem() {
        let item = API.createMenuItem('Передать деньги', 'Передать деньги другому игроку')
        item.Activated.connect(() => {
            let playerName = API.getUserInput('Имя игрока: ', 60).substring(11)
            if (playerName.length == 0) {
                API.sendNotification('~r~Имя не может быть пустым')
                return
            }
            let amount = parseInt(API.getUserInput('Сумма: ', 12).substring(6))
            if (isNaN(amount)) {
                API.sendNotification('~r~Некорректный ввод')
                return
            }
            API.triggerServerEvent('SendMoneyToPlayer', playerName, amount)
        })
        this.menu.AddItem(item)
    }

    private createAnimMenu() {
        let animMenu = API.addSubMenu(this.menu, 'Анимации', 'Различные анимации персонажа', true)
        PlayerActionsMenu.setMenuBanner(animMenu)

        let stopAnimItem = API.createMenuItem('~b~Остановить анимацию', '')
        stopAnimItem.Activated.connect(() => API.triggerServerEvent('StopPlayerAnimation'))
        animMenu.AddItem(stopAnimItem)

        this.getAnimationItems().forEach(item => {
            let scenarioName = Object.keys(item)[0] as string
            item[scenarioName].Activated.connect(() => API.triggerServerEvent('PlayPlayerAnimation', scenarioName))
            animMenu.AddItem(item[scenarioName])
        })

        addCloseItem(animMenu)
    }

    private getAnimationItems(): any[] {
        return [
            {'WORLD_HUMAN_AA_COFFEE' : API.createMenuItem('Пить кофе', '')},
            {'WORLD_HUMAN_MUSICIAN' : API.createMenuItem('Музыкант', 'Играть мелодию')},            
            {'WORLD_HUMAN_CHEERING' : API.createMenuItem('Аплодировать', '')},
            {'WORLD_HUMAN_HUMAN_STATUE' : API.createMenuItem('Статуя', '')},
            {'WORLD_HUMAN_MUSCLE_FLEX' : API.createMenuItem('Позировать', 'Играть мускулами')},
            {'WORLD_HUMAN_PARTYING' : API.createMenuItem('Вечеринка', '')},
            {'WORLD_HUMAN_PICNIC' : API.createMenuItem('Пикник', '')},
            {'WORLD_HUMAN_YOGA' : API.createMenuItem('Йога', '')},
            {'WORLD_HUMAN_PUSH_UPS' : API.createMenuItem('Отжиматься', '')},
            {'WORLD_HUMAN_SIT_UPS' : API.createMenuItem('Качать пресс', '')},
            {'WORLD_HUMAN_AA_SMOKE' : API.createMenuItem('Закурить', 'Курение вредит вашему здоровью!')},
            {'WORLD_HUMAN_BUM_FREEWAY' : API.createMenuItem('Побираться', '')},
            {'WORLD_HUMAN_BUM_SLUMPED' : API.createMenuItem('Валяться', '')}
        ]
    }

    private createHousesMenu() {
        this.housesMenu = API.addSubMenu(this.menu, 'Перемещение к дому', 'Список улиц ваших домов для перемещения', true)
    }

    private static setMenuBanner(menu: NativeUI.UIMenu) {
        API.setMenuBannerSprite(menu, 'shopui_title_ie_modgarage', 'shopui_title_ie_modgarage')
    }
}