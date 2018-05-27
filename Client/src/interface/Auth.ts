/// <reference path='../../types-gt-mp/index.d.ts' />

import { Cef } from '../browser/Cef'
import { ServerEventHandler } from '../event-handlers/ServerEventHandler'
import { ActiveCamera } from '../utils/ActiveCamera'

export class Auth {
    private static cef: Cef
    private static isLogin: boolean = true

    public initHandlers() {
        Auth.cef = new Cef('Auth', 'ui/auth/auth.html')
        Auth.cef.on('login', this.login) 
        Auth.cef.on('register', this.register)
        ServerEventHandler.getInstance().on('ShowLogin', this.showLogin)
        ServerEventHandler.getInstance().on('HideAuth', this.hide)
        ServerEventHandler.getInstance().on('BadLogin', this.badLogin)
        ServerEventHandler.getInstance().on('BadRegister', this.badRegister)
    }

    private showLogin() {
        ActiveCamera.setActiveCamera(new Vector3(1569.68, -367.94, 226.57), new Vector3(0.00, 0.00, 132.63))
        Auth.cef.setCursorVisible(true)
        Auth.cef.load()
    }

    private hide() {
        Auth.cef.destroy()
        API.setGameplayCameraActive()
    }

    private badLogin() {
        Auth.cef.eval('badLogin()')
    }

    private badRegister() {
        Auth.cef.eval('badRegister()') 
    }

    private login(args: any[]) {
        let email = args[0] as string
        let password = args[1] as string
        API.triggerServerEvent('Login', email, password)
    }

    private register(args: any[]) {
        let email = args[0] as string
        let password = args[1] as string
        let friendReferal = args[2] as string        
        // let youtuberCode = args[3] as string
        API.triggerServerEvent('Register', email, password, friendReferal)
    }
}