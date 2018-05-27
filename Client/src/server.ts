/// <reference path='../types-gt-mp/index.d.ts' />

// интерфейс
import { Auth } from './interface/Auth'
import { Interface } from './interface/Main'
import { Speedometer } from './interface/Speedometer'
import { PlayerInfo } from './interface/PlayerInfo'
import { CharCreator } from './interface/CharCreator'
import { Help } from './interface/Help'
import { VoiceManager } from './managers/voice/VoiceManager'
import { Phone } from './interface/Phone'

// меню управления
import { AdminMenu } from './menu/AdminMenu'
import { TestMenu } from './menu/TestMenu'
import { RacecourseMenu } from './menu/RacecourseMenu'

import { GtaCharacterManager } from './managers/GtaCharacterManager'

let adminMenu: AdminMenu
let testMenu: TestMenu

let playerActionsMenuOpen: boolean = false
let vehicleActionsMenuOpen: boolean = false

// запуск сервера
API.onResourceStart.connect(() => {
    new Auth().initHandlers()    
    CharCreator.getInstance()
    new Interface().initHandlers()
    adminMenu = new AdminMenu()
    testMenu = new TestMenu()
    VoiceManager.getInstance()

    API.toggleAIPedsSpawning(true)

    let players = API.getStreamedPlayers();
    for (let i = 0; i < players.Length - 1; i++) {
        let player = players[i] as LocalHandle
        GtaCharacterManager.setPlayerAppearance(player)
    }
})

// основной апдейт
API.onUpdate.connect(() => {
    Speedometer.getInstance().update()
})

// нажатие на клавишу
API.onKeyDown.connect((entity, args) => {
    let player = API.getLocalPlayer()
    if (API.getEntitySyncedData(player, 'DisableHotkeys')) {        
        return
    }
    switch (args.KeyCode) {
        // информация об игроке
        case Keys.U:
            Help.getInstance().hide()
            PlayerInfo.getInstance().trigger()
            break
        // окно помощи
        case Keys.J:
            PlayerInfo.getInstance().hide()
            Help.getInstance().trigger()
            break
        // телефон
        case Keys.P:
            if (API.hasEntitySyncedData(player, 'HasPhone')) {
                Phone.getInstance().triggerPhone()
            }
            else {
                API.sendNotification('~r~У вас нет мобильного телефона')
            }            
            break
        // free
        case Keys.Y:
            API.sendNotification('Меню игрока перенесено на ~b~F1')
            break
        // действия игрока
        case Keys.F1:
            playerActionsMenuOpen = !playerActionsMenuOpen
            API.triggerServerEvent('TriggerPlayerActionMenu', playerActionsMenuOpen)
            break
        // управление транспортом
        case Keys.F2:
            vehicleActionsMenuOpen = !vehicleActionsMenuOpen
            API.triggerServerEvent('TriggerVehicleActionMenu', vehicleActionsMenuOpen)
            break
        // поднять руки вверх
        case Keys.Z:
            let flags = 1 << 0 | 1 << 4 | 1 << 5 | 1 << 7
            API.playPlayerAnimation('missminuteman_1ig_2', 'handsup_base', flags)
            break
        // меню управления ипподромом
        case Keys.F9:
            if (API.hasEntitySyncedData(player, 'IsRacecourseAdmin')) {
                new RacecourseMenu().triggerVisible()
            }
            break
        // тестовое меню
        case Keys.F11:
            if (API.hasEntitySyncedData(player, 'IsAdmin')) {
                testMenu.triggerVisible()
            }
            break
        // меню админа
        case Keys.F12:
            if (API.hasEntitySyncedData(player, 'IsAdmin')) {
                adminMenu.triggerVisible()
            }            
            break
    }
})

// остановка сервера
API.onResourceStop.connect(() => {
    API.removeWaypoint()
})