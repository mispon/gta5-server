import { Cef } from '../browser/Cef'
import { ServerEventHandler } from '../event-handlers/ServerEventHandler'

export class PlayerInfo {
    private static instance: PlayerInfo
    private cef: Cef
    private isOpen: boolean
    private path: string = 'ui/interface/info/index.html'

    private constructor() {
        this.cef = new Cef('PlayerInfo', this.path)
        this.cef.on('getPlayerInfo', () => API.triggerServerEvent('GetPlayerInfo'))
        this.cef.on('close', this.hide)
        this.cef.on('changeSettings', (args) => API.triggerServerEvent('SaveSettings', args[0]))
        this.cef.on('eventParticipation', (args) => API.triggerServerEvent('EventParticipation', args[0]))
        ServerEventHandler.getInstance().on('ShowPlayerInfo', this.showPlayerInfo)        
    }

    public static getInstance (): PlayerInfo {
        if (!PlayerInfo.instance) {
            PlayerInfo.instance = new PlayerInfo()
        }
        return PlayerInfo.instance
    }

    public trigger() {
        if (!PlayerInfo.instance.isOpen) {  
            PlayerInfo.instance.isOpen = true            
            PlayerInfo.instance.loadBrowser()
        } else {
            PlayerInfo.instance.hide()
        }
    }

    private loadBrowser() {
        PlayerInfo.instance.cef = new Cef('PlayerInfo', PlayerInfo.instance.path)     
        PlayerInfo.instance.cef.setCursorVisible(true)
        PlayerInfo.instance.cef.setChatVisible(true)            
        PlayerInfo.instance.cef.load()        
    }

    public hide() {
        if (PlayerInfo.instance.isOpen) {
            PlayerInfo.instance.isOpen = false
            PlayerInfo.instance.cef.destroy()
        }        
    }

    private showPlayerInfo(args: any[]) {
        let data = args[0] as string
        let events = args[1] as string
        PlayerInfo.instance.cef.eval(`showInfo(${data}, ${events})`)
    }
}