import { Cef } from '../browser/Cef'
import { ServerEventHandler } from '../event-handlers/ServerEventHandler'

export class Help {
    private static instance: Help
    private cef: Cef
    private isOpen: boolean = false
    private path: string = 'ui/interface/help/index.html'

    private constructor() { 
        this.cef = new Cef('Help', this.path)       
        this.cef.on('close', this.hide)     
    }

    public static getInstance (): Help {
        if (!Help.instance) {
            Help.instance = new Help()
        }
        return Help.instance
    }

    public trigger() {
        if (!Help.instance.isOpen) {            
            Help.instance.isOpen = true
            Help.instance.loadBrowser()
        }
        else {
            Help.instance.isOpen = false
            Help.instance.cef.destroy()
        }
    }

    public hide() {
        if (Help.instance.isOpen) {
            Help.instance.isOpen = false
            Help.instance.cef.destroy()
        }        
    }

    private loadBrowser() {
        Help.instance.cef = new Cef('Help', Help.instance.path)
        Help.instance.cef.setCursorVisible(true)
        Help.instance.cef.setChatVisible(true)
        Help.instance.cef.load()
    }
}