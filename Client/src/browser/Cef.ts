/// <reference path="../../types-gt-mp/index.d.ts" />

import { CefEventHandlerSingleton } from './CefEventHandlerSingleton'
import { CefLoadHandlerSingleton } from './CefLoadHandlerSingleton'
import { EventListener } from '../event-handlers/EventListener'

export class Cef {
    public browser: GrandTheftMultiplayer.Client.GUI.CEF.Browser
    private cursor: boolean = false
    private open: boolean = false
    private external: boolean = false
    private headless: boolean = false
    private name: string
    private path: string
    private cefEventHandler: CefEventHandlerSingleton = CefEventHandlerSingleton.getInstance()
    private chat: boolean = false
    private hud: boolean = true

    constructor(name: string, path: string) {
        this.name = name
        this.path = path

        var resolution: Size = API.getScreenResolution()
        this.browser = API.createCefBrowser(resolution.Width, resolution.Height, !this.external)
        API.setCefBrowserPosition(this.browser, 0, 0)
        API.waitUntilCefBrowserInit(this.browser)
    }

    public async load(path?: string) : Promise<void> {
        if (path) { this.path = path }
        if (this.open) { return }

        API.loadPageCefBrowser(this.browser, this.path)

        // почему-то не возвращаемся из этого метода
        //await CefLoadHandlerSingleton.getInstance().finishedLoading(this.name)

        if (!this.chat) { API.setCanOpenChat(false) }
        if (!this.hud) { API.setHudVisible(false) }
        if (this.cursor) { API.showCursor(true) }
        this.setOpen(true)        
    }

    public destroy(): void {
        API.destroyCefBrowser(this.browser)    
        if (!this.chat) { API.setCanOpenChat(true) }
        if (!this.hud) { API.setHudVisible(true) }
        if (this.cursor) { API.showCursor(false) }    
        this.setOpen(false)
    }

    public eval(evalString: string): void {
        this.browser.eval(evalString)
    }

    public call(methodName: string, ...args: any[]): void {
        try {            
            this.browser.call(methodName, args)
        } catch (error) {
            API.sendChatMessage(`~r~${error}`)
        }        
    }

    public on(eventName: string, cb: (args: any[]) => void): void {
        this.cefEventHandler.on(`${this.name}.${eventName}`, cb)
    }

    public add(listener: EventListener): void {
        this.cefEventHandler.add(listener)
    }

    public remove(listener: EventListener): void {
        this.cefEventHandler.remove(listener)
    }

    public removeAll(eventName: string): void {
        this.cefEventHandler.removeAll(`${this.name}.${eventName}`)
    }

    private setOpen (newValue: boolean): void { this.open = newValue }
    public setExternal (newValue: boolean): void { this.external = newValue }    
    public setCursorVisible (newValue: boolean): void { this.cursor = newValue }
    public setChatVisible (newValue: boolean): void { this.chat = newValue }
    public setHudVisible (newValue: boolean): void { this.hud = newValue }
    public setHeadless (newValue: boolean): void { 
        this.headless = newValue; 
        API.setCefBrowserHeadless(this.browser, this.headless) 
    }
}