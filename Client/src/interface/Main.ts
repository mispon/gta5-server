/// <reference path='../../types-gt-mp/index.d.ts' />

import { Cef } from '../browser/Cef'
import { ServerEventHandler } from '../event-handlers/ServerEventHandler'

export class Interface {
    private static cef: Cef

    constructor() {
        Interface.cef = new Cef('Interface', 'ui/interface/index.html')
        Interface.cef.setChatVisible(true)
        Interface.cef.setHeadless(true)
        Interface.cef.load()
    }

    public initHandlers() {
        ServerEventHandler.getInstance().on('ShowInterface', () => Interface.cef.setHeadless(false))
        ServerEventHandler.getInstance().on('SetTimer', this.setTimer)
        ServerEventHandler.getInstance().on('StopTimer', this.stopTimer)
        ServerEventHandler.getInstance().on('SetProgressAction', this.setProgressAction)
        ServerEventHandler.getInstance().on('StopProgressAction', this.stopProgressAction)
        ServerEventHandler.getInstance().on('UpdateInfo', this.updateInfo)
        ServerEventHandler.getInstance().on('ShowHint', this.showHint)
        ServerEventHandler.getInstance().on('HideHint', this.hideHint)
        ServerEventHandler.getInstance().on('ShowSubtitle', this.showSubtitle)
        ServerEventHandler.getInstance().on('SetGiftsTimer', this.setGiftsTimer)
        Interface.cef.on('TimeOfExamWas', this.timeOfExamWas)
        Interface.cef.on('SetDeliveryFail', this.setDeliverySuccess)
        Interface.cef.on('StartEvent', () => API.triggerServerEvent('StartEvent'))
        Interface.cef.on('StartCarRace', () => API.triggerServerEvent('StartCarRace'))
        Interface.cef.on('StartMotoRace', () => API.triggerServerEvent('StartMotoRace'))
        Interface.cef.on('StartRally', () => API.triggerServerEvent('StartRally'))
        Interface.cef.on('StartMountainRace', () => API.triggerServerEvent('StartMountainRace'))
        Interface.cef.on('StartFight', () => API.triggerServerEvent('StartFight'))
        Interface.cef.on('CaptureDistrict', () => API.triggerServerEvent('CaptureDistrict'))
        Interface.cef.on('GiveOnlineGift', () => API.triggerServerEvent('GiveOnlineGift'))        
        Interface.cef.on('FinishBuilderPoint', () => API.triggerServerEvent('FinishBuilderPoint'))

        Interface.cef.on('StartRacecourseRace', () => API.triggerServerEvent('StartRacecourseRace'))
    }
    
    private setTimer(args: any[]) {           
        Interface.cef.eval(`startTimer(${args[0]}, '${args[1]}')`)
    }

    private stopTimer(args: any[]) {
        Interface.cef.eval('stopTimer()')
    }

    private setProgressAction(args: any[]) {           
        Interface.cef.eval(`setProgressAction(${args[0]}, '${args[1]}')`)
    }

    private stopProgressAction(args: any[]) {
        Interface.cef.eval('stopProgressAction()')
    }

    private timeOfExamWas() {
        API.triggerServerEvent('TimeOfExamWas')
    }

    private setDeliverySuccess() {
        API.triggerServerEvent('SetDeliveryFail')
    }

    private updateInfo(args: any[]) {        
        let satiety = args[0] as number
        let balance = args[1] as number
        let exp = args[2] as number   
        Interface.cef.eval(`updateInfo(${satiety},${balance},${exp})`)
    }

    private showHint(args: any[]) {
        let message = args[0] as string
        let height = args[1] as number
        Interface.cef.eval(`showHint('${message}', ${height})`)
    }

    private hideHint() {
        Interface.cef.eval('hideHint()')
    }

    private showSubtitle(args: any[]) {
        let message = args[0] as string
        let time = 5000
        if (args.length > 1) {
            time = args[1] as number
        }
        API.displaySubtitle(message, time)
    }

    private setGiftsTimer(args: any[]) {
        Interface.cef.eval(`setGiftsTimer(${args[0]}, '${args[1]}')`)
    }
}