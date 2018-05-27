/// <reference path='../../types-gt-mp/index.d.ts' />

import { Cef } from '../browser/Cef'
import { ServerEventHandler } from '../event-handlers/ServerEventHandler'

export class Phone {
    private static instance: Phone
    private cef: Cef
    private isOpen: boolean = false

    private constructor() {
        this.cef = new Cef('Phone', 'ui/interface/phone/index.html')
        this.cef.on('getPhoneInfo', () => API.triggerServerEvent('GetPhoneInfo'))
        this.cef.on('startCall', (args: any[]) => API.triggerServerEvent('StartCall', args[0]))
        this.cef.on('addContact', (args: any[]) => API.triggerServerEvent('AddPhoneContact', args[0], args[1]))           
        this.cef.on('answerCall', (args: any[]) => API.triggerServerEvent('AnswerCall', args[0]))
        this.cef.on('hangupCall', () => API.triggerServerEvent('HangupCall'))
        this.cef.on('hidePhone', this.hidePhone)

        ServerEventHandler.getInstance().on('SetPhoneInfo', this.setPhoneInfo)
        ServerEventHandler.getInstance().on('ShowMainDisplay', this.showMainDisplay)
        ServerEventHandler.getInstance().on('HidePhone', this.hidePhone)
    }

    public static getInstance (): Phone {
        if (!Phone.instance) {
            Phone.instance = new Phone()
        }
        return Phone.instance
    }

    public triggerPhone() {      
        if (this.isOpen) {
            this.hidePhone()
        }
        else {            
            Phone.instance.isOpen = true
            Phone.instance.cef = new Cef('Phone', 'ui/interface/phone/index.html')
            Phone.instance.cef.setCursorVisible(true)
            Phone.instance.cef.setChatVisible(false)            
            Phone.instance.cef.load()
            API.setEntitySyncedData(API.getLocalPlayer(), 'DisableHotkeys', true)
            API.triggerServerEvent('TriggerPhoneVisible', true)
        }
    }

    private setPhoneInfo(args: any[]) {
        let contacts = args[0] as string        
        let caller = args[1] as string        
        if (caller == null) {
            Phone.instance.cef.eval(`setInfo(${contacts})`)
        }
        else {
            let callerNumber = args[2] as number
            Phone.instance.cef.eval(`setInfo(${contacts}, \"${caller}\", ${callerNumber})`)
        }        
    }

    private hidePhone() {
        Phone.instance.isOpen = false
        Phone.instance.cef.destroy() 
        API.resetEntitySyncedData(API.getLocalPlayer(), 'DisableHotkeys')
        API.triggerServerEvent('TriggerPhoneVisible', false)
    }

    private showMainDisplay() {
        Phone.instance.cef.eval(`showMainDisplay()`)
    }
}